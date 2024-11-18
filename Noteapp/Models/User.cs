using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace NoteApp.Models
{
    /// <summary>
    /// Represents a user entity.
    /// </summary>
    [Table("users")]
    public class User : IdentityUser
    {
        /// <summary>
        /// Gets or sets the list of notes associated with the user.
        /// </summary>
        public List<Note> Notes { get; set; } = new List<Note>();
    }
}