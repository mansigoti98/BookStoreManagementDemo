using System.ComponentModel.DataAnnotations;

namespace BookStoreManagement_1.ViewModels.Customer
{
    public class CustomerCreateViewModel
    {
        public int CustomerId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public string BillingAddress { get; set; }

        /*   [Required(ErrorMessage = "City is required")]
           public int? CityId { get; set; }

           [Required(ErrorMessage = "State is required")]
           public int? StateId { get; set; }

           [Required(ErrorMessage = "Country is required")]

           public int? CountryId { get; set; }



           public IEnumerable<SelectListItem>? Countries { get; set; }*/
    }
}
