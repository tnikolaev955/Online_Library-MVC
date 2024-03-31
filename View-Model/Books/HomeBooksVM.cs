using Online_Library.Models;

namespace Online_Library.View_Model.Books
{
    public class HomeBooksVM
    {
        public List<Book> Top5Books { get; set; }
        public List<Book>? RecommendedBooks { get; set; }

    }
}
