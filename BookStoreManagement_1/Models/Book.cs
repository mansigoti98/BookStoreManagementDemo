using System.ComponentModel.DataAnnotations.Schema;

namespace BookStoreManagement_1.Models;

public partial class Book
{
    public int BookId { get; set; }
    public string Title { get; set; } = null!;
    public string? Author { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public int QuantityInStock { get; set; }

    public int CategoryId { get; set; } 
    public Category? Category { get; set; }
    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
