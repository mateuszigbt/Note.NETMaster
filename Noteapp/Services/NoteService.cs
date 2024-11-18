using NoteApp.Models;
using NoteApp.Repositories;
using Noteapp.Exceptions;
using Noteapp.Dto;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Xml.Serialization;
using Noteapp.Models;
using System.Text;
using Noteapp.Dto.Notes;
using System.Net.Mime;

namespace NoteApp.Services
{
    /// <summary>
    /// Service class responsible for managing notes.
    /// </summary>
    public class NoteService
    {
        private readonly UserService _userService;
        private readonly INoteRepository _noteRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="NoteService"/> class.
        /// </summary>
        /// <param name="userService">The user service instance.</param>
        /// <param name="noteRepository">The note repository instance.</param>
        public NoteService(UserService userService, INoteRepository noteRepository)
        {
            _userService = userService;
            _noteRepository = noteRepository;
        }

        /// <summary>
        /// Retrieves notes belonging to the currently authenticated user.
        /// </summary>
        /// <param name="email">The email of the user.</param>
        /// <returns>A list of note DTOs.</returns>
        public async Task<List<NoteDTO>> GetUserNotes(string email)
        {
            var user = await _userService.GetCurrentUser();
            var notes = await _noteRepository.GetAllNotesByUserAsync(user.Id);
            return notes.Select(NoteDTO.From).ToList();
        }
        //DO DOKONCZENIA!!!!
        /// <summary>
        /// Download note as file in extension like txt, json and xml for authenticated user dependent on extension of file.
        /// </summary>
        /// <param name="file">The file containing title and content</param>
        /// <param name="email">The email of the user.</param>
        /// <returns>The upload note DTO.</returns>
        public async Task<FileDTO> DownloadUserNoteAsFile(string email, int id, string format)
        {
            var user = await _userService.GetCurrentUser();
            var note = await _noteRepository.GetNoteByIdAsync(id);
            var fileNote = new FileDTO();
            string fileContent;

            if (note == null || note.UserId != user.Id)
            {
                throw new Exception("Note not found or access denied.");
            }

            var noteDto = new XmlNoteDTO
            {
                Title = note.Title,
                Content = note.Content
            };

            switch (format)
            {
                case "txt":
                    fileContent = $"{note.Title}\n{note.Content}";
                    fileNote = new FileDTO()
                    {
                        FileByte = Encoding.UTF8.GetBytes(fileContent),
                        FileName = $"{DateTime.UtcNow}.txt",
                        ContentType = "text/plain"
                    };
                    break;
                case "json":
                    fileContent = JsonSerializer.Serialize(noteDto);
                    fileNote = new FileDTO()
                    {
                        FileByte = Encoding.UTF8.GetBytes(fileContent),
                        FileName = $"{DateTime.UtcNow}.json",
                        ContentType = "application/json"
                    };
                    break;
                case "xml":
                    var xmlSerializer = new XmlSerializer(typeof(XmlNoteDTO));
                    using (var stringWriter = new StringWriter())
                    {
                        xmlSerializer.Serialize(stringWriter, noteDto);
                        var xmlContent = stringWriter.ToString();
                        fileNote = new FileDTO()
                        {
                            FileByte = Encoding.UTF8.GetBytes(xmlContent),
                            FileName = $"{DateTime.UtcNow}.xml",
                            ContentType = "application/xml"
                        };
                    }
                    break;
            }

            return fileNote;
        }

        /// <summary>
        /// Creates a new note for the authenticated user.
        /// </summary>
        /// <param name="note">The note DTO containing title and content.</param>
        /// <param name="email">The email of the user.</param>
        /// <returns>The created note DTO.</returns>
        public async Task<NoteDTO> CreateNote(CreateNoteDTO note, string email)
        {
            var user = await _userService.GetCurrentUser();
            var newNote = new Note
            {
                Title = note.Title,
                Content = note.Content,
                UserId = user.Id,
                CreationDate = DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow
            };
            var savedNote = await _noteRepository.SaveNoteAsync(newNote);
            return NoteDTO.From(savedNote);
        }

        /// <summary>
        /// Upload a file to note for authenticated user dependent on extension of file.
        /// </summary>
        /// <param name="file">The file containing title and content</param>
        /// <param name="email">The email of the user.</param>
        /// <returns>The upload note DTO.</returns>
        public async Task<NoteDTO> UploadFiles(string email, IFormFile file)
        {
            var user = await _userService.GetCurrentUser();
            var note = new Note();
            var extensionFile = Path.GetExtension(file.FileName)?.ToLower();

            if (file.Length > 0)
            {
                switch (extensionFile)
                {
                    case ".txt":
                        using (var reader = new StreamReader(file.OpenReadStream()))
                        {
                            note = new Note()
                            {
                                UserId = user.Id,
                                Title = await reader.ReadLineAsync(),
                                Content = await reader.ReadToEndAsync(),
                                CreationDate = DateTime.UtcNow,
                                ModifiedDate = DateTime.UtcNow
                            };
                        }
                        break;
                    case ".json":
                        using (var reader = new StreamReader(file.OpenReadStream()))
                        {
                            string jsonString = await reader.ReadToEndAsync();
                            var tempNote = JsonSerializer.Deserialize<NoteDTO>(jsonString);
                            note = new Note()
                            {
                                UserId = user.Id,
                                Title = tempNote.Title,
                                Content = tempNote.Content,
                                CreationDate = DateTime.UtcNow,
                                ModifiedDate = DateTime.UtcNow
                            };
                        }
                        break;
                    case ".xml":
                        using (var reader = new StreamReader(file.OpenReadStream()))
                        {
                            var serializer = new XmlSerializer(typeof(XmlNoteDTO));
                            var tempNote = (XmlNoteDTO)serializer.Deserialize(reader);

                            note = new Note()
                            {
                                UserId = user.Id,
                                Title = tempNote.Title,
                                Content = tempNote.Content,
                                CreationDate = DateTime.UtcNow,
                                ModifiedDate = DateTime.UtcNow
                            };
                        }
                        break;
                }
            }
            
            var saveUploadedNote = await _noteRepository.SaveUploadedNotesAsync(note);
            return NoteDTO.From(saveUploadedNote);
        }

        /// <summary>
        /// Updates an existing note for the authenticated user.
        /// </summary>
        /// <param name="id">The ID of the note to be updated.</param>
        /// <param name="updatedNote">The updated note DTO containing new title and content.</param>
        /// <param name="email">The email of the user.</param>
        /// <returns>The updated note DTO.</returns>
        public async Task<NoteDTO> UpdateNote(long id, UpdateNoteDTO updatedNote, string email)
        {
            var user = await _userService.GetCurrentUser();
            var existingNote = await _noteRepository.GetNoteByIdAsync(id);
            
            if (existingNote == null || existingNote.UserId != user.Id)
            {
                throw new NoteNotFoundException(id);
            }

            existingNote.Title = updatedNote.Title;
            existingNote.Content = updatedNote.Content;
            existingNote.ModifiedDate = DateTime.UtcNow;
            var updated = await _noteRepository.UpdateNoteAsync(id, existingNote);
            return NoteDTO.From(updated);
        }

        /// <summary>
        /// Deletes a note belonging to the authenticated user.
        /// </summary>
        /// <param name="id">The ID of the note to be deleted.</param>
        /// <param name="email">The email of the user.</param>
        public async Task DeleteNoteById(long id, string email)
        {
            var user = await _userService.GetCurrentUser();
            var existingNote = await _noteRepository.GetNoteByIdAsync(id);
            if (existingNote == null || existingNote.UserId != user.Id)
            {
                throw new NoteNotFoundException(id);
            }

            await _noteRepository.DeleteNoteByIdAsync(id);
        }
    }
}