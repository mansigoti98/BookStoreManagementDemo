using System.Security.Claims;
using AutoMapper;
using BookStoreManagement_1.Data;
using BookStoreManagement_1.Models;
using BookStoreManagement_1.Models.Admin;
using BookStoreManagement_1.ViewModels;
using BookStoreManagement_1.ViewModels.Admin;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BookStoreManagement_1.Controllers
{
    [Authorize(AuthenticationSchemes = "AdminScheme")]
    public class AdminController : Controller
    {
        private readonly BookStoreDbContext _context;
        private readonly ILogger<AdminController> _logger;
        private readonly IMapper _mapper;

        public AdminController(BookStoreDbContext context, ILogger<AdminController> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        public IActionResult Dashboard()
        {
            var books = _context.Books.ToList();
            return View(books);
        }

        [HttpGet]
        public IActionResult AddBook()
        {
            var categories = _context.Categories
                .Select(c => new SelectListItem { Value = c.CategoryId.ToString(), Text = c.CategoryName })
                .ToList();

            var viewModel = new BookFormViewModel
            {
                Categories = categories,
                Book = new Book()
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddBook(BookFormViewModel viewModel)
        {
            if (viewModel.Book.CategoryId == 0)
            {
                ModelState.AddModelError("Book.CategoryId", "Please select a category.");
            }

            if (!ModelState.IsValid)
            {
                foreach (var modelStateKey in ModelState.Keys)
                {
                    var value = ModelState[modelStateKey];
                    foreach (var error in value.Errors)
                    {
                        _logger.LogWarning("Invalid field: {Key}, Error: {Error}", modelStateKey, error.ErrorMessage);
                    }
                }

                _logger.LogInformation("Invalid ModelState...");

                viewModel.Categories = _context.Categories
                    .Select(c => new SelectListItem { Value = c.CategoryId.ToString(), Text = c.CategoryName })
                    .ToList();
                return View(viewModel);

            }

            _context.Books.Add(viewModel.Book);
            _context.SaveChanges();
            return RedirectToAction("Dashboard", "Admin"); 
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var book = await _context.Books.FindAsync(id);
            if (book == null)
                return NotFound();

            return View(book);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Book book)
        {
            if (id != book.BookId) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(book);
                _context.SaveChanges();
                return RedirectToAction("Dashboard");
            }

            return View(book);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var book = _context.Books.Find(id);
            if (book != null)
            {
                _context.Books.Remove(book);
                _context.SaveChanges();
            }
            return RedirectToAction("Dashboard");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetCountries()
        {
            var countries = _context.Countries
                .Select(c => new { c.CountryId, c.Name })
                .ToList();

            return Json(countries);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetStates(int countryId)
        {
            var states = _context.States
                .Where(s => s.CountryId == countryId)
                .Select(s => new { s.StateId, s.Name })
                .ToList();

            return Json(states);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetCities(int stateId)
        {
            var cities = _context.Cities
                .Where(c => c.StateId == stateId)
                .Select(c => new { c.CityId, c.Name })
                .ToList();

            return Json(cities);
        }
    }

}
