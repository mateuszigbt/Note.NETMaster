namespace Noteapp.Exceptions.Note
{
    public class NoteWrongExtensionException : Exception
    {
        public NoteWrongExtensionException()
            : base("Note wrong extension.") { }
        /*
        public NoteWrongExtensionException(long id)
            : base($"Note with id {id} not found.") { }
        */
        public NoteWrongExtensionException(string message)
            : base(message) { }

        public NoteWrongExtensionException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
