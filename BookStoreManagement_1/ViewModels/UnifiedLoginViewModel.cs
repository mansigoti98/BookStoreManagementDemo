using System.ComponentModel.DataAnnotations;

namespace BookStoreManagement_1.ViewModels
{
    public class UnifiedLoginViewModel
    {
        [Required]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool IsAdmin { get; set; }
    }
}
