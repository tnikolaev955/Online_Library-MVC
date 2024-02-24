using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Online_Library.View_Model.Users
{
    public class EditProfileVM
    {
        [Required(ErrorMessage = " Име е задължително поле")]
        [DisplayName("Име")]
        public required string Name { get; set; }

        [Required(ErrorMessage = " Потребителско име е задължително поле")]
        [DisplayName("Потребителско име")]
        public required string Username { get; set; }

        [Required(ErrorMessage = " Имейлът е задължително поле")]
        [DisplayName("Имейл")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DisplayName("Стара Парола")]
        [DataType(DataType.Password)]
        public required string OldPassword { get; set; }
        [Required]
        [DisplayName("Нова Парола")]
        [DataType(DataType.Password)]
        public required string Password { get; set; }

        [Required(ErrorMessage = "Рождената дата е задължителна")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        [DisplayName("Рождена дата")]
        public DateOnly Birthdate { get; set; }
    }
}
