using Online_Library.Models;

namespace Online_Library.View_Model.Books
{
    public class BorrowVM
    {
       public List<UserBorrowBook> toReturn = new List<UserBorrowBook>();
       public List<UserBorrowBook> returned = new List<UserBorrowBook>();
    }
}
