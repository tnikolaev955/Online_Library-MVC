using Online_Library.Models;
using Online_Library.View_Model.Shared;
namespace Online_Library.View_Model
{
    public class CatalogModel
    {
        public List<Book> books { get; set; }
        public string? SortBy { get; set; }
        public string? SortOrder { get; set; }
        public List<Category> categories { get; set; }
        public PagerVM Pager { get; set; }
    }
}
