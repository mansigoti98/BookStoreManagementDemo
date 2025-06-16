using System.ComponentModel.DataAnnotations;

namespace BookStoreManagement_1.ViewModels.Admin
{
    public class AdminLoginViewModel
    {
        [Required(ErrorMessage = "Phone number or Email is required")]
        [Display(Name = "Phone or Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
