using Online_Library.Models;
using System.ComponentModel.DataAnnotations;

namespace Online_Library.View_Model
{
    public class UploadFileVM
    {
        public Book Book { get; set; }

        public string? FileName { get; set; }

        [Required(ErrorMessage = "Снимка на корицата е задължително поле")]
        public IFormFile? File { get; set; } 


    }
}
