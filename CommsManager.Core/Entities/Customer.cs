namespace CommsManager.Core.Entities;

public class Customer : BaseEntity
{
    private Customer() { }

    public Customer(string name, string email)
    {
        Id = Guid.NewGuid();
        Name = name;
        Email = email;
        CreatedDate = DateTime.UtcNow;
        IsActive = true;
    }

    public string Name { get; private set; }
    public string Email { get; private set; }
    public string Phone { get; private set; }
    public string SocialMediaLink { get; private set; }
    public DateTime CreatedDate { get; private set; }
    public bool IsActive { get; private set; }

    private readonly List<Order> _orders = new();
    public IReadOnlyCollection<Order> Orders => _orders.AsReadOnly();

    private readonly List<CustomerContact> _contacts = new();
    public IReadOnlyCollection<CustomerContact> Contacts => _contacts.AsReadOnly();

    public void UpdateContactInfo(string phone, string socialMediaLink)
    {
        Phone = phone;
        SocialMediaLink = socialMediaLink;
    }

    public void AddContact(CustomerContact contact)
    {
        _contacts.Add(contact);
    }

    public void Deactivate()
    {
        IsActive = false;
    }
}