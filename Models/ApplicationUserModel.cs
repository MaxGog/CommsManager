using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace CommsManager.Models;

public class ApplicationUser : IdentityUser
{
    public string? DisplayName { get; set; }
    public string? Bio { get; set; }
    public string? ProfileImageUrl { get; set; }
    public string? SocialMediaLinks { get; set; }

    public virtual ICollection<Service> Services { get; set; } = new List<Service>();
    public virtual ICollection<Client> Clients { get; set; } = new List<Client>();
    public virtual ICollection<Income> Incomes { get; set; } = new List<Income>();
    public virtual ICollection<Expense> Expenses { get; set; } = new List<Expense>();
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
