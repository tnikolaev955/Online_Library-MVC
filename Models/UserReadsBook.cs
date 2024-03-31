using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

//github test 

namespace Online_Library.Models
{
    public class UserReadsBook
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }

        [ForeignKey("Book")]
        public int BookId { get; set; }

        public User? User { get; set; }
        public Book? Book { get; set; }

        public bool IsRead { get; set; }

        [DisplayName("Страница")]
        public int Page { get; set; } = 1;

        public DateTime? DateOfRead { get; set; }

    }
}
