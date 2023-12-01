using System.ComponentModel.DataAnnotations;

namespace Online_Library.Models
{
    public class Category
    {
        //създаваме свойства - property 

        [Key] // анотация на ключовите данни 
        public int Id { get; set; } // - Основния ключ ма таблицата или първичния ключ 

        [Required]

        public string Name { get; set; } // - Име на категорията

        public int DisplayOrder { get; set; } // - Определя реда на показване на Category
    }
}
