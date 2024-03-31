using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Microsoft.EntityFrameworkCore;

namespace Online_Library.View_Model.Users
{
    [Keyless]
    public class LoginVM
    {
        [Required(ErrorMessage = "Потребителското име е задължително поле")]
        [DisplayName("Потребителско име")]
        public required string Username { get; set; }

        [Required(ErrorMessage = "Паролата е задължителна")]
        [DisplayName("Парола")]
        [DataType(DataType.Password)]
        public required string Password { get; set; }
    }
}
