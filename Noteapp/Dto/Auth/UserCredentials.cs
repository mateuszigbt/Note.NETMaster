using System.ComponentModel.DataAnnotations;

namespace Noteapp.Dto
{
    /// <summary>
    /// Represents the credentials required for user authentication.
    /// </summary>
    public class UserCredentials
    {
        /// <summary>
        /// Gets or sets the email address of the user.
        /// </summary>
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the password of the user.
        /// </summary>
        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }

        /// <summary>
        /// Default constructor for UserCredentials.
        /// </summary>
        public UserCredentials() { }

        /// <summary>
        /// Initializes a new instance of the UserCredentials class with the specified email and password.
        /// </summary>
        /// <param name="email">The email address of the user.</param>
        /// <param name="password">The password of the user.</param>
        public UserCredentials(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }
}