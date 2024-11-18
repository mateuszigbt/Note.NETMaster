using Microsoft.AspNetCore.Mvc;
using Noteapp.Dto;
using Noteapp.Exceptions;
using NoteApp.Models;
using NoteApp.Services;
using System.Security.Claims;

namespace NoteApp.Controllers
{
    /// <summary>
    /// Controller responsible for handling note-related operations such as creating, updating, retrieving, and deleting notes.
    /// </summary>
    [ApiController]
    [Route("api/notes")]
    public class NoteController : ControllerBase
    {
        private readonly NoteService _noteService;

        /// <summary>
        /// Initializes a new instance of the <see cref="NoteController"/> class.
        /// </summary>
        /// <param name="noteService">The note service to be used by this controller.</param>
        public NoteController(NoteService noteService)
        {
            _noteService = noteService;
        }

        /// <summary>
        /// Retrieves all notes for the currently authenticated user.
        /// </summary>
        /// <returns>An action result containing a list of notes or an error message.</returns>
        [HttpGet]
        public async Task<ActionResult<List<NoteDTO>>> GetUserNotes()
        {
            try
            {
                var emailClaim = HttpContext.User.FindFirst(ClaimTypes.Email);

                if (emailClaim == null)
                {
                    return Unauthorized("User not authenticated or email claim is missing.");
                }

                var email = emailClaim.Value;
                var notes = await _noteService.GetUserNotes(email);
                return Ok(notes);
            }
            catch (UserNotAuthenticatedException)
            {
                return Unauthorized("User not authenticated.");
            }
        }

        /// <summary>
        /// Downloading an existing file note for the currently authenticated user.
        /// </summary>
        /// <param name="id">The ID of the note to be downloaded.</param>
        /// <param name="format">Format of downloading note.</param>
        /// <returns>An action result containing the dwonloading note file in txt, json and xml or an error message.</returns>
        [HttpGet("download/{id}")]
        public async Task<ActionResult<NoteDTO>> DownloadNote(int id, [FromQuery] string format)
        {
            try
            {
                var emailClaim = HttpContext.User.FindFirst(ClaimTypes.Email);

                if (emailClaim == null)
                {
                    return Unauthorized("User not authenticated or email claim is missing.");
                }
                if (format != "txt" && format != "json" && format != "xml")
                {
                    return Conflict("Wrong format extension file.");
                }

                var email = emailClaim.Value;
                var fileNote = await _noteService.DownloadUserNoteAsFile(email, id, format);
                return File(fileNote.FileByte, fileNote.ContentType, fileNote.FileName);
            }
            catch (UserNotAuthenticatedException)
            {
                return Unauthorized("User not authenticated.");
            }
        }

        /// <summary>
        /// Creates a new note for the currently authenticated user.
        /// </summary>
        /// <param name="note">The note to be created.</param>
        /// <returns>An action result containing the created note or an error message.</returns>
        [HttpPost]
        public async Task<ActionResult<NoteDTO>> CreateNote([FromBody] CreateNoteDTO note)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var emailClaim = HttpContext.User?.FindFirst(ClaimTypes.Email);
                if (emailClaim == null)
                {
                    return Unauthorized("User not authenticated.");
                }

                var email = emailClaim.Value;
                var savedNote = await _noteService.CreateNote(note, email);
                return CreatedAtAction(nameof(GetUserNotes), savedNote);
            }
            catch (UserNotAuthenticatedException)
            {
                return Unauthorized("User not authenticated.");
            }
        }

        /// <summary>
        /// Upload file for the currently authenticated user.
        /// </summary>
        /// <param name="file">The note will be saved by file.</param>
        /// <returns>An action result containing the upload note or an error message.</returns>
        [HttpPost("upload")]
        public async Task<ActionResult<NoteDTO>> UploadFiles(IFormFile file)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var emailClaim = HttpContext.User?.FindFirst(ClaimTypes.Email);
                var extensionFile = Path.GetExtension(file.FileName)?.ToLower();

                if (emailClaim == null)
                {
                    return Unauthorized("User not authenticated.");
                } 
                if (extensionFile != ".txt" && extensionFile != ".json" && extensionFile != ".xml")
                {
                    return Conflict("Wrong extension file.");
                }
                var email = emailClaim.Value;
                var savedUploadedNote = await _noteService.UploadFiles(email, file);
                return CreatedAtAction(nameof(GetUserNotes), savedUploadedNote);
            }
            catch (UserNotAuthenticatedException)
            {
                return Unauthorized("User not authenticated.");
            }
        }

        /// <summary>
        /// Updates an existing note for the currently authenticated user.
        /// </summary>
        /// <param name="id">The ID of the note to be updated.</param>
        /// <param name="note">The updated note data.</param>
        /// <returns>An action result containing the updated note or an error message.</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<NoteDTO>> UpdateNote(long id, [FromBody] UpdateNoteDTO note)
        {
            try
            {
                var emailClaim = HttpContext.User.FindFirst(ClaimTypes.Email);

                if (emailClaim == null)
                {
                    return Unauthorized("User not authenticated or email claim is missing.");
                }

                var email = emailClaim.Value;
                var updatedNote = await _noteService.UpdateNote(id, note, email);
                return Ok(updatedNote);
            }
            catch (NoteNotFoundException)
            {
                return NotFound($"Note with ID {id} not found.");
            }
            catch (UserNotAuthenticatedException)
            {
                return Unauthorized("User not authenticated.");
            }
        }

        /// <summary>
        /// Deletes a note by its ID for the currently authenticated user.
        /// </summary>
        /// <param name="id">The ID of the note to be deleted.</param>
        /// <returns>An action result indicating the outcome of the delete operation.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNoteById(long id)
        {
            try
            {
                var emailClaim = HttpContext.User.FindFirst(ClaimTypes.Email);

                if (emailClaim == null)
                {
                    return Unauthorized("User not authenticated or email claim is missing.");
                }

                var email = emailClaim.Value;
                await _noteService.DeleteNoteById(id, email);
                return NoContent();
            }
            catch (NoteNotFoundException)
            {
                return NotFound($"Note with ID {id} not found.");
            }
            catch (UserNotAuthenticatedException)
            {
                return Unauthorized("User not authenticated.");
            }
        }
    }
}