using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Online_Library.Models
{
    public class Book //Entity class 
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Заглавието е задължително")]
        [DisplayName("Заглавие")]
        public required string Title { get; set; }

        [Required(ErrorMessage = "Авторът е задължителен")]
        [DisplayName("Автор")]
        public required string Author { get; set; }


        [Required(ErrorMessage = "Брой страници е задължително поле")]
        [DisplayName("Страници")]
        public int pages { get; set; }

        [Required(ErrorMessage = "ISBN е задължителен")]
        [DisplayName("ISBN")]
        public required string ISBN { get; set; }

        [Required(ErrorMessage = "Годината на публикуване е задължителен")]
        [DisplayName("Годината на публикуване")]
        public int YearOfPublish { get; set; }

        [ForeignKey("Category")]
        [Required(ErrorMessage = "Категорията е задължителна")]
        public int CategoryId { get; set; }
        [DisplayName("Категория")]
        [Required(ErrorMessage = "Категорията е задължителна")]
        public Category Category { get; set; }

        [DisplayName("Снимка на корица")]
        public string? PictureLink { get; set; }

        [Required(ErrorMessage = "Описанието е задължително")]
        [DisplayName("Описание")]
        public string Description { get; set; }

        public string? PdfFilePath { get; set; }

        [NotMapped]
        public IFormFile? File { get; set; }

        private Book book;

        public Book GetBook()
        {
            return book;
        }

        public void SetBook(Book value)
        {
            book = value;
        }

        public string? FileName { get; set; }



        public Book()
        {
            PictureLink = "";
            Description = "";
            Ratings = new List<Rating>();
            UsersWhoRead = new List<UserReadsBook>();
        }

        public ICollection<Rating>? Ratings { get; set; }
        public double? AvgRating
        {
            get
            {
                if(Ratings == null || Ratings.Count <= 0)
                {
                    return 0;
                }
                return Ratings.Average(r => r.Rate);
            }
        }

        public ICollection<UserReadsBook> UsersWhoRead { get; set; }

    }
}
