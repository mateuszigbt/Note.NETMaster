using Microsoft.AspNetCore.Identity;
using NoteApp.Models;
using System.Security.Cryptography;

/// <summary>
/// Class responsible for hashing and verifying passwords for users.
/// </summary>
public class PasswordHasher : IPasswordHasher<User>
{
    /// <summary>
    /// Hashes the provided password for the user.
    /// </summary>
    /// <param name="user">The user for whom the password is hashed.</param>
    /// <param name="password">The password to hash.</param>
    /// <returns>The hashed password.</returns>
    public string HashPassword(User user, string password)
    {
        // Generate a salt
        byte[] salt;
        new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);

        // Hash the password with the salt using bcrypt
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password, BCrypt.Net.BCrypt.GenerateSalt());

        return hashedPassword;
    }

    /// <summary>
    /// Verifies if the provided password matches the hashed password for the user.
    /// </summary>
    /// <param name="user">The user whose password is being verified.</param>
    /// <param name="hashedPassword">The hashed password to compare against.</param>
    /// <param name="providedPassword">The password provided by the user for verification.</param>
    /// <returns>The result of the password verification.</returns>
    public PasswordVerificationResult VerifyHashedPassword(User user, string hashedPassword, string providedPassword)
    {
        // Verify the provided password against the hashed password using bcrypt
        if (BCrypt.Net.BCrypt.Verify(providedPassword, hashedPassword))
        {
            return PasswordVerificationResult.Success;
        }
        return PasswordVerificationResult.Failed;
    }
}