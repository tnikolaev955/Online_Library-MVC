using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Online_Library.Models
{
    public class ReaderCard
    {
      [Key]
      public int Id { get; set; }

      [ForeignKey("User")]
      public int User_id { get; set; }

      public User User { get; set; }

      [Required]
      public string Status { get; set; }

     [Required]
     public DateTime From_date { get; set; }

     [Required]
     public DateTime To_date { get; set; }

     [Required]
     public decimal Price { get; set; }

    }
}
