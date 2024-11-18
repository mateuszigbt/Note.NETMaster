namespace Noteapp.Dto.Notes
{
    public class FileDTO
    {
        public string FileName { get; set; }
        public string Title { get; set; }
        public string ContentType { get; set; }
        public byte[] FileByte { get; set; }
    }
}
