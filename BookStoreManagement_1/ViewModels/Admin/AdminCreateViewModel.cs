using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace BookStoreManagement_1.ViewModels.Admin
{
    public class AdminCreateViewModel
    {
        public int AdminId { get; set; }

        [Required(ErrorMessage = "Full name is required")]
        [StringLength(100)]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email format")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [MinLength(6, ErrorMessage = "Minimum 6 characters required")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Phone is required")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone must be 10 digits")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Billing address is required")]
        public string BillingAddress { get; set; }

        [Required(ErrorMessage = "City is required")]
        public int? CityId { get; set; }

        [Required(ErrorMessage = "State is required")]
        public int? StateId { get; set; }

        [Required(ErrorMessage = "Country is required")]

        public int? CountryId { get; set; }

        public IEnumerable<SelectListItem>? Countries { get; set; }
    }
}
