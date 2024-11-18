using Microsoft.AspNetCore.Identity;
using Noteapp.Dto;
using Noteapp.Exceptions;
using Noteapp.Security;
using NoteApp.Models;
using System.Security.Claims;

namespace Noteapp.Services
{
    /// <summary>
    /// Service class responsible for user authentication and authorization.
    /// </summary>
    public class AuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly JwtTokenProvider _jwtTokenProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthService"/> class.
        /// </summary>
        /// <param name="userManager">The user manager instance.</param>
        /// <param name="signInManager">The sign-in manager instance.</param>
        /// <param name="jwtTokenProvider">The JWT token provider instance.</param>
        public AuthService(UserManager<User> userManager, SignInManager<User> signInManager, JwtTokenProvider jwtTokenProvider)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtTokenProvider = jwtTokenProvider;
        }

        /// <summary>
        /// Authenticates a user with the provided email and password and generates a JWT token upon successful authentication.
        /// </summary>
        /// <param name="email">The email of the user.</param>
        /// <param name="password">The password of the user.</param>
        /// <returns>The generated JWT token.</returns>
        public async Task<string> Login(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                throw new UserNotFoundException(email);

            var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
            if (!result.Succeeded)
                throw new InvalidCredentialsException(CredentialsErrorType.INVALID_PASSWORD);

            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = roles.Select(role => new Claim(ClaimTypes.Role, role)).ToList();

            return _jwtTokenProvider.GenerateToken(user, roleClaims);
        }

        /// <summary>
        /// Registers a new user with the provided credentials.
        /// </summary>
        /// <param name="userDto">The user credentials.</param>
        /// <returns>The registered user.</returns>
        public async Task<User> Register(UserCredentials userDto)
        {
            if (await _userManager.FindByEmailAsync(userDto.Email) != null)
                throw new UserAlreadyExistsException(userDto.Email);

            var user = new User { Email = userDto.Email, UserName = userDto.Email.Split('@')[0] };
            var result = await _userManager.CreateAsync(user, userDto.Password);

            if (!result.Succeeded)
            {
                string errorMessage = "User creation failed: ";
                foreach (var error in result.Errors)
                {
                    errorMessage += $"{error.Code} - {error.Description}. ";
                }
                throw new InvalidOperationException(errorMessage);
            }

            var roleResult = await _userManager.AddToRoleAsync(user, "USER");

            if (!roleResult.Succeeded)
                throw new InvalidOperationException("Role creation failed: " + string.Join(", ", roleResult.Errors));

            return user;
        }
    }
}