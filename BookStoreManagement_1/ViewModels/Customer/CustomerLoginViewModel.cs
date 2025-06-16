using System.ComponentModel.DataAnnotations;

namespace BookStoreManagement_1.ViewModels
{
    public class CustomerLoginViewModel
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

    }
}
