using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Noteapp.Dto;
using Noteapp.Exceptions;
using NoteApp.Models;
using NoteApp.Services;
using System.Security.Claims;

namespace NoteApp.Controllers
{
    /// <summary>
    /// Controller responsible for handling user-related operations such as user retrieval, update, and deletion.
    /// </summary>
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserController"/> class.
        /// </summary>
        /// <param name="userService">The user service to be used by this controller.</param>
        public UserController(UserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Retrieves all users. Accessible only to users with the 'Admin' role.
        /// </summary>
        /// <returns>An action result containing a list of users or an error message.</returns>
        [HttpGet("all")]
        public async Task<ActionResult<List<User>>> GetAllUsers()
        {
            var users = await _userService.GetAllUsers();
            return Ok(users);
        }

        /// <summary>
        /// Retrieves the currently authenticated user.
        /// </summary>
        /// <returns>An action result containing the current user or an error message.</returns>
        [HttpGet("me")]
        public async Task<ActionResult<User>> GetCurrentUser()
        {
            try
            {
                var user = await _userService.GetCurrentUser();
                return Ok(user);
            }
            catch (UserNotAuthenticatedException)
            {
                return Unauthorized("User not authenticated.");
            }
        }

        [HttpPut("me")]
        public async Task<ActionResult<User>> UpdateCurrentUser([FromBody] UserCredentials userCredentials)
        {
            try
            {
                var updatedUser = await _userService.UpdateCurrentUser(userCredentials);
                return Ok(updatedUser);
            }
            catch (UserNotAuthenticatedException)
            {
                return Unauthorized("User not authenticated.");
            }
            catch (Exception)
            {
                // Log the exception (ex) if needed
                return StatusCode(500, "An error occurred while updating the user.");
            }
        }

        /// <summary>
        /// Retrieves a user by ID.
        /// </summary>
        /// <param name="id">The ID of the user to retrieve.</param>
        /// <returns>An action result containing the user or an error message.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUserById(string id)
        {
            try
            {
                var user = await _userService.GetUserById(id);
                return Ok(user);
            }
            catch (UserNotFoundException)
            {
                return NotFound($"User with ID {id} not found.");
            }
        }

        /// <summary>
        /// Retrieves a user by email.
        /// </summary>
        /// <param name="email">The email of the user to retrieve.</param>
        /// <returns>An action result containing the user or an error message.</returns>
        [HttpGet]
        public async Task<ActionResult<User>> GetUserByEmail([FromQuery] string email)
        {
            try
            {
                var user = await _userService.GetUserByEmail(email);
                return Ok(user);
            }
            catch (UserNotFoundException)
            {
                return NotFound($"User with email {email} not found.");
            }
        }

        /// <summary>
        /// Deletes a user by ID.
        /// </summary>
        /// <param name="id">The ID of the user to delete.</param>
        /// <returns>An action result indicating the outcome of the delete operation.</returns>
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            try
            {
                await _userService.DeleteUser(id);
                return NoContent();
            }
            catch (UserNotFoundException)
            {
                return NotFound($"User with ID {id} not found.");
            }
        }
    }
}