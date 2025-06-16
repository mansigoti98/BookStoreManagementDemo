using AutoMapper;
using BookStoreManagement_1.Data;
using BookStoreManagement_1.Models;
using BookStoreManagement_1.ViewModels;
using BookStoreManagement_1.ViewModels.Customer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
//using static ViewRenderer;

namespace BookStoreManagement_1.Controllers
{
    public class CustomerController : Controller
    {
        private readonly BookStoreDbContext _context;
        private readonly ILogger<CustomerController> _logger;
        private readonly CartService _cartService;
        private readonly IMapper _mapper;

        public CustomerController(BookStoreDbContext context, ILogger<CustomerController> logger, CartService cartService)
        {
            _context = context;
            _logger = logger;
            _cartService = cartService;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(CustomerCreateViewModel customerCreateViewModel)
        {
            if (ModelState.IsValid)
            {
                var existingEmail = await _context.Customers
                    .FirstOrDefaultAsync(c => c.Email == customerCreateViewModel.Email);
                var existingPhone = await _context.Customers
                    .FirstOrDefaultAsync(c => c.Phone == customerCreateViewModel.Phone);

                if (existingEmail != null || existingPhone != null)
                {
                    ModelState.AddModelError("Email", "Email/phone already exists");
                    return View(customerCreateViewModel);
                }

                PasswordHelper.CreatePasswordHash(customerCreateViewModel.Password, out string hash, out string salt);
                var customer = _mapper.Map<Customer>(customerCreateViewModel);
                customer.PasswordHash = hash;
                customer.PasswordSalt = salt;
                

                _context.Customers.Add(customer);
                await _context.SaveChangesAsync();

                return RedirectToAction("Login", "Account");
            }

            return View(customerCreateViewModel);
        }

        public IActionResult Shop()
        {
            var books = _context.Books.ToList();
            return View(books);
        }

        [HttpGet]
        public IActionResult Shop(string? filterCategory, decimal? minPrice, decimal? maxPrice, int page = 1, int pageSize = 10)
        {
            var booksQuery = _context.Books.AsQueryable();

            if (!string.IsNullOrEmpty(filterCategory))
            {
                booksQuery = booksQuery.Where(b => b.Category.CategoryName == filterCategory);
            }

            if (minPrice.HasValue)
            {
                booksQuery = booksQuery.Where(b => b.Price >= minPrice);
            }

            if (maxPrice.HasValue)
            {
                booksQuery = booksQuery.Where(b => b.Price <= maxPrice);
            }

            int totalBooks = booksQuery.Count();

            var books = booksQuery
                .Include(b => b.Category)
                .OrderBy(b => b.Title)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var viewModel = new BookFilterViewModel
            {
                Books = books,
                FilterCategory = filterCategory,
                MinPrice = minPrice,
                MaxPrice = maxPrice,
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling((decimal)totalBooks / pageSize),
                PageSize = pageSize
            };

            return View(viewModel);
        }


        [HttpPost]
        public IActionResult AddToCart(int bookId)
        {
            _cartService.AddToCart(bookId);
            return RedirectToAction("Cart");
        }

        public IActionResult Cart()
        {
            var cart = _cartService.GetOrCreateCart();
            return View(cart);
        }

        [HttpPost]
        public IActionResult RemoveFromCart(int cartItemId)
        {
            _cartService.RemoveFromCart(cartItemId);
            return RedirectToAction("Cart");
        }

        [HttpPost]
        public IActionResult UpdateQuantity(int cartItemId, int quantity)
        {
            var error = _cartService.UpdateQuantity(cartItemId, quantity);

            if (error != null)
            {
                ViewData["Error"] = error;
                var cart = _cartService.GetOrCreateCart();
                return View("Cart", cart);
            }
            return RedirectToAction("Cart");
        }

        public IActionResult ResetCart()
        {
            _cartService.ClearCart();
            return RedirectToAction("Cart");
        }

        public IActionResult Checkout()
        {
            var result = _cartService.Checkout();

            if (!string.IsNullOrEmpty(result.ErrorMessage))
            {
                TempData["Error"] = result.ErrorMessage;
                return RedirectToAction("Cart");
            }
            // GenerateInvoice(result.OrderId);

            TempData["Success"] = "Order placed successfully!";
            return RedirectToAction("GenerateInvoice", new { orderId = result.OrderId });
        }

        public IActionResult GenerateInvoice(int orderId)
        {
            var order = _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Book)
                .FirstOrDefault(o => o.OrderId == orderId);

            if (order == null)
                return NotFound("Order not found.");

            //string html = RenderViewToString(this, "Invoice", order);

            //var renderer = new ChromePdfRenderer();
            //var pdf = renderer.RenderHtmlAsPdf(html);

            var fileName = $"Invoice_Order_{orderId}.pdf";
            var invoicesPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "invoices");
            if (!Directory.Exists(invoicesPath))
                Directory.CreateDirectory(invoicesPath);

            var filePath = Path.Combine(invoicesPath, fileName);
           // pdf.SaveAs(filePath);

            // Redirect to summary page with orderId
            return RedirectToAction("InvoiceSummary", new { orderId = orderId });
        }

        public IActionResult InvoiceSummary(int orderId)
        {
            var order = _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Book)
                .FirstOrDefault(o => o.OrderId == orderId);

            if (order == null)
                return NotFound("Order not found.");

            ViewBag.PdfFileName = $"Invoice_Order_{orderId}.pdf";

            return View(order);
        }
        public IActionResult DownloadInvoice(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return BadRequest();

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "invoices", fileName);

            if (!System.IO.File.Exists(filePath))
                return NotFound();

            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, "application/pdf", fileName);
        }

        [HttpGet]
        [Route("Customer/TestGenerate")]
        public IActionResult TestGenerate()
        {
            return GenerateInvoice(5);
        }

    }
}

