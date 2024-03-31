using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Online_Library.Models
{
    public class UserFavoriteBook
    {
       [Key] 
       public int Id { get; set; }

       [ForeignKey("User")]
       public int UserId { get; set; }

       [ForeignKey("Book")]
       public int BookId { get; set; }

       public User? User { get; set; }
       public Book? Book { get; set; }

    }
}
