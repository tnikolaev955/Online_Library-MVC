using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Online_Library.Models
{
    public class User //Entity class 
    {

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = " Име е задължително поле")]
        [DisplayName("Име")]
        public required string Name { get; set; }

        [Required(ErrorMessage = " Потребителско име е задължително поле")]
        [DisplayName("Потребителско име")]
        public required string Username { get; set; }

        [Required(ErrorMessage = " Имейлът е задължително поле")]
        [DisplayName("Имейл")]
        public string Email { get; set; }

        [Required]
        [DisplayName("Парола")]
        public required string Password { get; set; }

        [DisplayName("Роля")]
        public string Role { get; set; }

        [Required(ErrorMessage = "Рождената дата е задължителна")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        [DisplayName("Рождена дата")]
        public DateOnly Birthdate { get; set; }

        [Required]
        [DisplayName("Създаденa на")]

        public DateTime CreatedAt { get; set; }
        [DisplayName("Актуализиранa на")]

        public DateTime UpdatedAt { get; set; }

        [DisplayName("Последен абонамент:")] // Валидно за един месец
        public DateTime? SubscriptionDate { get; set; }

        public User()
        {
            this.Name = "";
            this.Username = "";
            this.Password = "";
            Ratings = new List<Rating>();
            BooksRead = new List<UserReadsBook>();

        }


        public User(string name, string username, string v)
        {
            this.Name = name;
            this.Username = username;
            this.Password = v;
            Ratings = new List<Rating>();
            BooksRead = new List<UserReadsBook>();
        }


        public ICollection<Rating> Ratings { get; set; }

        public ICollection<UserReadsBook> BooksRead { get; set; }

    }
}
