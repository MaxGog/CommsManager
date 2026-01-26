using CommsManager.Core.Enums;
using CommsManager.Core.Models;

namespace CommsManager.Core.Entities;

public class Customer : BaseEntity
{
    private string _name;
    private string? _description;
    private string? _communication;
    private byte[]? _customerPicture;
    private readonly List<Phones> _phones = [];
    private readonly List<Email> _emails = [];
    private readonly List<SocialLink> _socialLinks = [];
    private readonly List<Order> _orders = [];

    public Customer(string name)
    {
        if (string.IsNullOrWhiteSpace(name)) 
            throw new ArgumentException("Имя не может быть пустым.", nameof(name));

        Id = Guid.NewGuid();
        _name = name;
        CreatedDate = DateTime.UtcNow;
        IsActive = true;
    }

    public string Name
    {
        get => _name;
        set
        {
            if (string.IsNullOrWhiteSpace(value)) 
                throw new ArgumentException("Имя не может быть пустым.", nameof(value));
            _name = value;
        }
    }

    public DateTime CreatedDate { get; private set; }

    public bool IsActive { get; private set; }

    public string? Description
    {
        get => _description;
        set => _description = value;
    }

    public string? Communication
    {
        get => _communication;
        set => _communication = value;
    }

    public byte[]? CustomerPicture
    {
        get => _customerPicture;
        private set => _customerPicture = value;
    }

    public IReadOnlyCollection<Phones> Phones => _phones.AsReadOnly();
    public IReadOnlyCollection<Email> Emails => _emails.AsReadOnly();
    public IReadOnlyCollection<SocialLink> SocialLinks => _socialLinks.AsReadOnly();
    public IReadOnlyCollection<Order> Orders => _orders.AsReadOnly();

    public void AddPhone(string phone, string? description, string? type)
    {
        if (string.IsNullOrWhiteSpace(phone))
            throw new ArgumentException("Телефон не может быть пустым.", nameof(phone));

        _phones.Add(new Phones
        {
            NumberPhone = phone,
            Description = description,
            TypePhone = type
        });
    }

    public bool RemovePhone(Phones phone) => _phones.Remove(phone);

    public void AddEmail(string email, string? description, string? type)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Электронная почта не может быть пустой.", nameof(email));

        _emails.Add(new Email
        {
            EmailAdress = email,
            Description = description,
            TypeEmail = type
        });
    }

    public bool RemoveEmail(Email email) => _emails.Remove(email);

    public void AddSocialLink(string link, SocialPlatform? type, bool visible = true)
    {
        if (string.IsNullOrWhiteSpace(link))
            throw new ArgumentException("Ссылка на социальную сеть не может быть пустой.", nameof(link));

        _socialLinks.Add(new SocialLink
        {
            Link = link,
            TypeLink = type,
            IsVisible = visible,
            IsActive = visible
        });
    }

    public bool RemoveSocialLink(SocialLink socialLink) => _socialLinks.Remove(socialLink);

    public void AddOrder(Order order)
    {
        ArgumentNullException.ThrowIfNull(order);
        _orders.Add(order);
    }

    public bool RemoveOrder(Order order) => _orders.Remove(order);

    public void SetCustomerPicture(byte[] bytes)
    {
        ArgumentNullException.ThrowIfNull(bytes);
        if (bytes.Length == 0)
            throw new ArgumentException("Изображение не может быть пустым.", nameof(bytes));

        CustomerPicture = bytes;
    }

    public void ClearCustomerPicture() => CustomerPicture = null;

    public void Activate() => IsActive = true;

    public void Deactivate() => IsActive = false;
}