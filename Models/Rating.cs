using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Composition.Convention;

namespace Online_Library.Models
{
    public class Rating
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("User")]
        public int? UserId { get; set; }
        public User? User { get; set; }

        [ForeignKey("Book")]
        public int BookId { get; set; }
        public Book? Book { get; set; }

        [DisplayName("Рейтинг")]
        public int Rate { get; set; }

        [DisplayName("Коментар")]
        public string Comment {  get; set; }

    }
}
