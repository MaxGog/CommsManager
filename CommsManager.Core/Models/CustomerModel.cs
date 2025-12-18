public class Customer
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string SocialMediaLink { get; set; }

    public ICollection<Order> Orders { get; set; }
}