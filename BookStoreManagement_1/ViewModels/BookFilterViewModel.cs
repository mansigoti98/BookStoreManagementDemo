using BookStoreManagement_1.Models;

namespace BookStoreManagement_1.ViewModels
{
    public class BookFilterViewModel
    {
        public List<Book> Books { get; set; } = new();

        public string? FilterCategory { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }

        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }

        public Cart? Cart { get; set; }
    }
}
