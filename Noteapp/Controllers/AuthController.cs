using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Noteapp.Dto;
using Noteapp.Exceptions;
using Noteapp.Security;
using Noteapp.Services;
using NoteApp.Models;
using NoteApp.Services;
using System.Security.Claims;

namespace Noteapp.Controllers
{
    /// <summary>
    /// Controller responsible for user authentication operations such as sign-in and sign-up.
    /// </summary>
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthController"/> class.
        /// </summary>
        /// <param name="authService">The authentication service to be used by this controller.</param>
        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Authenticates a user and returns a JWT token if the authentication is successful.
        /// </summary>
        /// <param name="credentials">The user's credentials (email and password).</param>
        /// <returns>An action result containing the JWT token or an error message.</returns>
        [HttpPost("signin")]
        public async Task<IActionResult> SignIn([FromBody] UserCredentials credentials)
        {
            try
            {
                var token = await _authService.Login(credentials.Email, credentials.Password);
                return Ok(new { token });
            }
            catch (UserNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidCredentialsException ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        /// <summary>
        /// Registers a new user and returns the created user's email.
        /// </summary>
        /// <param name="credentials">The user's credentials (email and password).</param>
        /// <returns>An action result containing the user's email or an error message.</returns>
        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] UserCredentials credentials)
        {
            try
            {
                var user = await _authService.Register(credentials);
                return Created($"api/users?email={user.Email}", new { user.Email });
            }
            catch (UserAlreadyExistsException ex)
            {
                return Conflict(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
