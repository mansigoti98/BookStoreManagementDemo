using System.ComponentModel.DataAnnotations;
using BookStoreManagement_1.Models;
using Microsoft.EntityFrameworkCore;

namespace BookStoreManagement_1.Models
{
    public class Country
    {
        [Key]
        public int CountryId { get; set; }
        public string Name { get; set; }
        public ICollection<State> States { get; set; }

    }
}
