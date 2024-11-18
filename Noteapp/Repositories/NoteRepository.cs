using Microsoft.EntityFrameworkCore;
using NoteApp.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoteApp.Repositories
{
    /// <summary>
    /// Repository class for handling CRUD operations related to notes.
    /// </summary>
    public class NoteRepository : INoteRepository
    {
        private readonly ApiDbContext _context;

        /// <summary>
        /// Constructor for NoteRepository.
        /// </summary>
        /// <param name="context">The database context.</param>
        public NoteRepository(ApiDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all notes asynchronously.
        /// </summary>
        /// <returns>A list of notes.</returns>
        public async Task<List<Note>> GetAllNotesAsync()
        {
            return await _context.Notes.ToListAsync();
        }

        /// <summary>
        /// Retrieves a note by its ID asynchronously.
        /// </summary>
        /// <param name="id">The ID of the note.</param>
        /// <returns>The note.</returns>
        public async Task<Note> GetNoteByIdAsync(long id)
        {
            return await _context.Notes.FindAsync(id);
        }

        /// <summary>
        /// Saves a new note asynchronously.
        /// </summary>
        /// <param name="note">The note to be saved.</param>
        /// <returns>The saved note.</returns>
        public async Task<Note> SaveNoteAsync(Note note)
        {
            await _context.Notes.AddAsync(note);
            await _context.SaveChangesAsync();
            return note;
        }

        /// <summary>
        /// Saves and upload new note asynchronously.
        /// </summary>
        /// <param name="note">The note to be saved.</param>
        /// <returns>The saved uploaded note.</returns>
        public async Task<Note> SaveUploadedNotesAsync(Note note)
        {
            await _context.AddAsync(note);
            await _context.SaveChangesAsync();
            return note;
        }
        
        /// <summary>
        /// Updates an existing note asynchronously.
        /// </summary>
        /// <param name="id">The ID of the note to be updated.</param>
        /// <param name="updatedNote">The updated note data.</param>
        /// <returns>The updated note.</returns>
        public async Task<Note> UpdateNoteAsync(long id, Note updatedNote)
        {
            var existingNote = await _context.Notes.FindAsync(id);
            if (existingNote != null)
            {
                existingNote.Title = updatedNote.Title;
                existingNote.Content = updatedNote.Content;
                existingNote.ModifiedDate = updatedNote.ModifiedDate;
                await _context.SaveChangesAsync();
            }
            return existingNote;
        }

        /// <summary>
        /// Deletes a note by its ID asynchronously.
        /// </summary>
        /// <param name="id">The ID of the note to be deleted.</param>
        public async Task DeleteNoteByIdAsync(long id)
        {
            var note = await _context.Notes.FindAsync(id);
            if (note != null)
            {
                _context.Notes.Remove(note);
                await _context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Retrieves all notes belonging to a specific user asynchronously.
        /// </summary>
        /// <param name="id">The ID of the user.</param>
        /// <returns>A list of notes belonging to the user.</returns>
        public async Task<List<Note>> GetAllNotesByUserAsync(string id)
        {
            return await _context.Notes.Where(n => n.UserId == id).ToListAsync();
        }
    }
}