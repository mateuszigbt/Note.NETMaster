using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NoteApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoteApp.Repositories
{
    /// <summary>
    /// Repository class for handling CRUD operations related to users.
    /// </summary>
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<User> _userManager;

        /// <summary>
        /// Constructor for UserRepository.
        /// </summary>
        /// <param name="userManager">The user manager.</param>
        public UserRepository(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        /// <summary>
        /// Finds a user by email asynchronously.
        /// </summary>
        /// <param name="email">The email of the user to find.</param>
        /// <returns>The user.</returns>
        public async Task<User> FindByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        /// <summary>
        /// Checks if a user exists by email asynchronously.
        /// </summary>
        /// <param name="email">The email to check.</param>
        /// <returns>True if the user exists, false otherwise.</returns>
        public async Task<bool> ExistsByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email) != null;
        }

        /// <summary>
        /// Saves a new user asynchronously.
        /// </summary>
        /// <param name="user">The user to save.</param>
        /// <returns>The saved user.</returns>
        public async Task<User> SaveAsync(User user)
        {
            var result = await _userManager.CreateAsync(user);
            if (result.Succeeded)
                return user;
            else
                throw new Exception($"Failed to save user: {string.Join(",", result.Errors.Select(e => e.Description))}");
        }

        /// <summary>
        /// Finds a user by ID asynchronously.
        /// </summary>
        /// <param name="id">The ID of the user to find.</param>
        /// <returns>The user.</returns>
        public async Task<User> FindByIdAsync(long id)
        {
            return await _userManager.FindByIdAsync(id.ToString());
        }

        /// <summary>
        /// Retrieves all users asynchronously.
        /// </summary>
        /// <returns>A collection of users.</returns>
        public async Task<IEnumerable<User>> FindAllAsync()
        {
            return await _userManager.Users.ToListAsync();
        }

        /// <summary>
        /// Deletes a user by ID asynchronously.
        /// </summary>
        /// <param name="id">The ID of the user to delete.</param>
        public async Task DeleteByIdAsync(long id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user != null)
            {
                var result = await _userManager.DeleteAsync(user);
                if (!result.Succeeded)
                    throw new Exception($"Failed to delete user: {string.Join(",", result.Errors.Select(e => e.Description))}");
            }
        }
    }
}