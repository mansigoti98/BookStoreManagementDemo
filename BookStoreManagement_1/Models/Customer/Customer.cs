using System;
using System.Collections.Generic;

namespace BookStoreManagement_1.Models;

public partial class Customer
{
    public int CustomerId { get; set; }
    public string FullName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Phone { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;
    public string PasswordSalt { get; set; } = null!;
    public string BillingAddress { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
