using System.ComponentModel.DataAnnotations.Schema;

namespace BookStoreManagement_1.Models.Admin
{
    public class Admin
    {
        public int AdminId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string BillingAddress { get; set; }
        public int? CityId { get; set; }
        public int? StateId { get; set; }
        public int? CountryId { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        [ForeignKey("CountryId")]
        public Country Country { get; set; }
        [ForeignKey("StateId")]
        public State State { get; set; }
        [ForeignKey("CityId")]
        public City City { get; set; }      
    }
}
