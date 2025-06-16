namespace BookStoreManagement_1.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = null!;

        public ICollection<Book> Books { get; set; } = new List<Book>();

    }
}
