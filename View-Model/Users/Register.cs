using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Microsoft.EntityFrameworkCore;

namespace Online_Library.View_Model.Users
{
    [Keyless]
    public class Register
    {
        [Required(ErrorMessage = "Име е задължително поле")]
        [DisplayName("Име")]
        public required string Name { get; set; }

        [Required(ErrorMessage = "Потребителското име е задължително поле")]
        [DisplayName("Потребителско име")]
        public required string Username { get; set; }

        [Required(ErrorMessage = "Имейла е задължителен")]
        [DisplayName("Имейл")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Паролата е задължителна")]
        [DisplayName("Парола")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d).+$", ErrorMessage ="Слаба парола, паролата трябва да има поне 1 цифра, 1 малка буква и 1 голяма буква ")]
        [MinLength(8, ErrorMessage ="Слаба парола. Паролата трябва да е поне 8 символа.")]
        public required string Password { get; set; }


        [Required(ErrorMessage = "Повторете паролата е задължителна")]
        [Compare("Password", ErrorMessage = "Паролите трябва да съвпадат.")]
        [DataType(DataType.Password)]
        [DisplayName("Повторете Парола")]
        public required string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Рождената дата е задължителна")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        [DisplayName("Рождена дата")]
        public DateOnly Birthdate { get; set; }

    }
}
