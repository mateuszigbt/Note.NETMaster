namespace Noteapp.Dto
{
    /// <summary>
    /// Represents the data transfer object (DTO) for updating a note.
    /// </summary>
    public class UpdateNoteDTO
    {
        /// <summary>
        /// Gets or sets the updated title of the note.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the updated content of the note.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateNoteDTO"/> class with default values.
        /// </summary>
        public UpdateNoteDTO()
        {
            // Parameterless constructor
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateNoteDTO"/> class with the specified title and content.
        /// </summary>
        /// <param name="title">The updated title of the note.</param>
        /// <param name="content">The updated content of the note.</param>
        public UpdateNoteDTO(string title, string content)
        {
            Title = title;
            Content = content;
        }
    }
}