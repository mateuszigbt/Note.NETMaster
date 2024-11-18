using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace NoteApp.Models
{
    /// <summary>
    /// Represents a note entity.
    /// </summary>
    [Table("Notes")]
    public class Note
    {
        /// <summary>
        /// Gets or sets the unique identifier for the note.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the user associated with the note.
        /// </summary>
        [ForeignKey("UserId")]
        [JsonIgnore]
        public User User { get; set; }

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
        /// Gets or sets the user ID associated with the note.
        /// </summary>
        public string UserId { get; set; }
    }
}