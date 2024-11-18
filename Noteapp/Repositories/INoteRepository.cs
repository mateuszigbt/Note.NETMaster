using NoteApp.Models;
using System.Collections.Generic;

namespace NoteApp.Repositories
{
    public interface INoteRepository
    {
        Task<List<Note>> GetAllNotesAsync();
        Task<Note> GetNoteByIdAsync(long id);
        Task<Note> SaveNoteAsync(Note note);
        Task<Note> UpdateNoteAsync(long id, Note updatedNote);
        Task DeleteNoteByIdAsync(long id);
        Task<List<Note>> GetAllNotesByUserAsync(string id);
        Task<Note> SaveUploadedNotesAsync(Note note);
    }
}