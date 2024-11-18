using NoteApp.Models;

namespace Noteapp.Dto
{
    public class ListNoteDTO
    {
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the title of the note.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the content of the note.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Gets or sets the creation date of the note.
        /// </summary>
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// Gets or sets the last modified date of the note.
        /// </summary>
        public DateTime ModifiedDate { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="NoteDTO"/> class with the specified properties.
        /// </summary>
        /// <param name="userId">The ID of the user associated with the note.</param>
        /// <param name="title">The title of the note.</param>
        /// <param name="content">The content of the note.</param>
        /// <param name="creationDate">The creation date of the note.</param>
        /// <param name="modifiedDate">The last modified date of the note.</param>
        public ListNoteDTO(string userId, string title, string content, DateTime creationDate, DateTime modifiedDate)
        {
            UserId = userId;
            Title = title;
            Content = content;
            CreationDate = creationDate;
            ModifiedDate = modifiedDate;
        }

        /// <summary>
        /// Maps a Note entity to a NoteDTO instance.
        /// </summary>
        /// <param name="note">The Note entity to map.</param>
        /// <returns>A new instance of NoteDTO mapped from the provided Note entity.</returns>
        public static ListNoteDTO From(Note note)
        {
            return new ListNoteDTO(
                userId: note.UserId,
                title: note.Title,
                content: note.Content,
                creationDate: note.CreationDate,
                modifiedDate: note.ModifiedDate
            );
        }
    }
}

