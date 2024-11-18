namespace Noteapp.Exceptions
{
    public class UserNotAuthenticatedException : Exception
    {
        public UserNotAuthenticatedException() : base("User not authenticated.")
        {
            // Constructor
        }
    }
}
