using System.Security.Claims;
using BookStoreManagement_1.Data;
using BookStoreManagement_1.Models;
using Microsoft.EntityFrameworkCore;

public class CartService
{
    private readonly BookStoreDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CartService(BookStoreDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    private int GetCustomerId()
    {
        return int.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    }

    public Cart GetOrCreateCart()
    {
        int customerId = GetCustomerId();
        var cart = _context.Carts
            .Include(c => c.Items)
            .ThenInclude(i => i.Book)
            .FirstOrDefault(c => c.CustomerId == customerId);

        if (cart == null)
        {
            cart = new Cart { CustomerId = customerId, Items = new List<CartItem>() };
            _context.Carts.Add(cart);
            _context.SaveChanges();
        }

        return cart;
    }

    public void AddToCart(int bookId)
    {
        var book = _context.Books.FirstOrDefault(b => b.BookId == bookId);
        if (book == null) return;

        var cart = GetOrCreateCart();
        var item = cart.Items.FirstOrDefault(i => i.BookId == bookId);

        if (item != null)
        {
            if (item.Quantity < book.QuantityInStock)
            {
                item.Quantity++;
            }
        }
        else
        {
            cart.Items.Add(new CartItem
            {
                BookId = bookId,
                Quantity = 1,
                CartId = cart.CartId
            });
        }

        _context.SaveChanges();
    }

    public string? UpdateQuantity(int cartItemId, int quantity)
    {
        var item = _context.CartItems.Include(ci => ci.Book).FirstOrDefault(ci => ci.Id == cartItemId);
        if (item != null && item.Book != null)
        {
            if (quantity <= 0)
            {
                _context.CartItems.Remove(item);
            }
            else if (quantity > item.Book.QuantityInStock)
            {
                return $"Only {item.Book.QuantityInStock} items in stock.";
            }
            else
            {
                item.Quantity = Math.Min(quantity, item.Book.QuantityInStock);
            }
            _context.SaveChanges();
        }
        return null;
    }

    public void RemoveFromCart(int cartItemId)
    {
        var item = _context.CartItems.FirstOrDefault(ci => ci.Id == cartItemId);
        if (item != null)
        {
            _context.CartItems.Remove(item);
            _context.SaveChanges();
        }
    }

    public void ClearCart()
    {
        var cart = GetOrCreateCart();
        if (cart != null)
        {
            _context.CartItems.RemoveRange(cart.Items);
            _context.Carts.Remove(cart);
            _context.SaveChanges();
        }
    }
    public CheckoutResult Checkout()
    {
        var result = new CheckoutResult();

        var cart = GetOrCreateCart();

        if (!cart.Items.Any())
        {
            result.ErrorMessage = "Cart is empty.";
            return result;
        }

        foreach (var item in cart.Items)
        {
            var book = _context.Books.FirstOrDefault(b => b.BookId == item.BookId);
            if (book == null)
            {
                result.ErrorMessage = $"Book with ID {item.BookId} not found.";
                return result;
            }

            if (item.Quantity > book.QuantityInStock)
            {
                result.ErrorMessage = $"Not enough stock for {book.Title}.";
                return result;
            }
        }

        var order = new Order
        {
            CustomerId = cart.CustomerId,
            OrderDate = DateTime.Now
        };
        _context.Orders.Add(order);
        _context.SaveChanges();

        foreach (var item in cart.Items)
        {
            var book = _context.Books.First(b => b.BookId == item.BookId);
            book.QuantityInStock -= item.Quantity;

            var orderDetail = new OrderDetail
            {
                OrderId = order.OrderId,
                BookId = book.BookId,
                Quantity = item.Quantity
            };
            _context.Orderdetails.Add(orderDetail);
        }

        _context.CartItems.RemoveRange(cart.Items);
        _context.Carts.Remove(cart);

        _context.SaveChanges();

        result.OrderId = order.OrderId;
        return result;
    }


}
