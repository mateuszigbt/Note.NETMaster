namespace Noteapp.Dto
{
    /// <summary>
    /// Represents the response data structure for JWT authentication.
    /// </summary>
    public class JwtResponse
    {
        /// <summary>
        /// Gets or sets the username associated with the JWT token.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the email associated with the JWT token.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the JWT token.
        /// </summary>
        public string Token { get; set; }
    }
}