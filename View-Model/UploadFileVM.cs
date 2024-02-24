using Online_Library.Models;

namespace Online_Library.View_Model
{
    public class UploadFileVM
    {
        public Book Book { get; set; }

        public string FileName { get; set; }
        public IFormFile File { get; set; } 
    }
}
