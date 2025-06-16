using BookStoreManagement_1.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace BookStoreManagement_1.ViewModels
{
    public class BookFormViewModel
    {
        public Book Book { get; set; } = new Book();
        public IEnumerable<SelectListItem> Categories { get; set; } = new List<SelectListItem>();
         
    }
}