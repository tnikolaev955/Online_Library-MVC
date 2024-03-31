using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Online_Library.Models
{
    public class Category //Entity class 
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Заглавието е задължително")] // Валидация задължително поле и error съобщение, ако валидацията не е успешна
        [StringLength(50, ErrorMessage = "Заглавието може да има максимум 50 символа")]

        [DisplayName("Заглавие")]
        public required string Title { get; set; }

        [Required(ErrorMessage = "Описанието е задължително")]
        [DisplayName("Описание")]
        public required string Description { get; set; }

        [ForeignKey("ParentCategory")]
        [DisplayName("Родител")]
        public int? ParentId { get; set; }

        [DisplayName("Родителска категория")]
        public Category? ParentCategory { get; set; }

        public ICollection<Book>? Books { get; set; }
    }
}
