using Online_Library.Models;

namespace Online_Library.View_Model.Books
{
    public class BookDetailsVM
    {
        public Book? Book { get; set; }
        public List<Rating>? Ratings { get; set; }

        public List<User>? Readers { get; set; }

        public Rating Rating { get; set; }
    }
}
