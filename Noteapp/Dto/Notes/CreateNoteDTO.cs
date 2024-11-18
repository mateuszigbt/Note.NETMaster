using System.ComponentModel.DataAnnotations;

namespace Noteapp.Dto
{
    /// <summary>
    /// Represents the data transfer object (DTO) used for creating a new note.
    /// </summary>
    public class CreateNoteDTO
    {
        /// <summary>
        /// Gets or sets the title of the note.
        /// </summary>
        [Required]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the content of the note.
        /// </summary>
        [MaxLength(100)]
        public string Content { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateNoteDTO"/> class.
        /// </summary>
        public CreateNoteDTO()
        {
            // Parameterless constructor
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateNoteDTO"/> class with the specified title and content.
        /// </summary>
        /// <param name="title">The title of the note.</param>
        /// <param name="content">The content of the note.</param>
        public CreateNoteDTO(string title, string content)
        {
            Title = title;
            Content = content;
        }
    }
}