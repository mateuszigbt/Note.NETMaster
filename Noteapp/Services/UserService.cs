using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Noteapp.Dto;
using Noteapp.Exceptions;
using NoteApp.Models;
using System.Security.Claims;

namespace NoteApp.Services
{
    /// <summary>
    /// Service class responsible for user-related operations.
    /// </summary>
    public class UserService
    {
        private IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<User> _userManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserService"/> class.
        /// </summary>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        /// <param name="userManager">The user manager instance.</param>
        public UserService(IHttpContextAccessor httpContextAccessor,
                            UserManager<User> userManager)
        {
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        /// <summary>
        /// Retrieves all users.
        /// </summary>
        /// <returns>A list of user objects.</returns>
        public async Task<List<User>> GetAllUsers()
        {
            return await _userManager.Users.ToListAsync();
        }

        /// <summary>
        /// Retrieves the current authenticated user.
        /// </summary>
        /// <returns>The current authenticated user.</returns>
        public async Task<User> GetCurrentUser()
        {
            var userContext = _httpContextAccessor.HttpContext.User;
            var emailClaim = userContext.FindFirst(ClaimTypes.Email);

            if (emailClaim == null)
            {
                throw new UserNotAuthenticatedException();
            }

            var email = emailClaim.Value;
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                throw new UserNotAuthenticatedException();
            }

            return user;
        }

        /// <summary>
        /// Retrieves a user by email.
        /// </summary>
        /// <param name="email">The email of the user to retrieve.</param>
        /// <returns>The user object.</returns>
        public async Task<User> GetUserByEmail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                throw new UserNotFoundException(email);
            }
            return user;
        }

        /// <summary>
        /// Retrieves a user by ID.
        /// </summary>
        /// <param name="id">The ID of the user to retrieve.</param>
        /// <returns>The user object.</returns>
        public async Task<User> GetUserById(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                throw new UserNotFoundException(id);
            }
            return user;
        }

        public async Task<User> UpdateCurrentUser(UserCredentials userCredentials)
        {
            var userContext = _httpContextAccessor.HttpContext.User;
            var emailClaim = userContext.FindFirst(ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(emailClaim))
            {
                throw new UserNotAuthenticatedException();
            }

            var user = await _userManager.FindByEmailAsync(emailClaim);
            if (user == null)
            {
                throw new UserNotAuthenticatedException();
            }

            // Update email if provided
            if (!string.IsNullOrEmpty(userCredentials.Email))
            {
                user.Email = userCredentials.Email;
                user.UserName = userCredentials.Email; // Assuming email is also the username
            }

            // Update password if provided
            if (!string.IsNullOrEmpty(userCredentials.Password))
            {
                var newPasswordHash = _userManager.PasswordHasher.HashPassword(user, userCredentials.Password);
                user.PasswordHash = newPasswordHash;
            }

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                throw new Exception($"Failed to update user: {string.Join(", ", updateResult.Errors.Select(e => e.Description))}");
            }
            return user;
        }

        /// <summary>
        /// Deletes a user by ID.
        /// </summary>
        /// <param name="id">The ID of the user to delete.</param>
        public async Task DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                throw new UserNotFoundException(id);
            }

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                throw new Exception($"Failed to delete user: {string.Join(",", result.Errors.Select(e => e.Description))}");
            }
        }
    }
}