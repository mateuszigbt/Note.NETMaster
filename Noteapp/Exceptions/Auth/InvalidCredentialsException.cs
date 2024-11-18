using System;

namespace Noteapp.Exceptions
{

    public class InvalidCredentialsException : Exception
    {
        public InvalidCredentialsException(CredentialsErrorType errorType)
            : base("Invalid credentials: " + errorType.GetMessage())
        {
        }
    }

    public enum CredentialsErrorType
    {
        EMAIL_NOT_FOUND,
        INVALID_PASSWORD
    }

    public static class CredentialsErrorTypeExtensions
    {
        public static string GetMessage(this CredentialsErrorType errorType)
        {
            switch (errorType)
            {
                case CredentialsErrorType.EMAIL_NOT_FOUND:
                    return "email not found";
                case CredentialsErrorType.INVALID_PASSWORD:
                    return "invalid password";
                default:
                    throw new ArgumentOutOfRangeException(nameof(errorType), errorType, null);
            }
        }
    }
}