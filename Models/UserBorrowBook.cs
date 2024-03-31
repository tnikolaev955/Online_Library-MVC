using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Online_Library.Models
{
    public class UserBorrowBook
    {
        [Key]
        public int Id { get; set; }

        public DateTime DateOfBorrow { get; set; }
        public DateTime? DateOfReturn { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }

        [ForeignKey("Book")]
        public int BookId { get; set; }

        public User? User { get; set; }
        public Book? Book { get; set; }

        
    }
}
