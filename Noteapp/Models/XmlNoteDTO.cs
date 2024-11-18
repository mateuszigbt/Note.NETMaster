using System.Xml.Serialization;

namespace Noteapp.Models
{
    [XmlRoot("NoteDTO")]
    public class XmlNoteDTO
    {
        public string Title { get; set; }
        public string Content { get; set; }
    }
}
