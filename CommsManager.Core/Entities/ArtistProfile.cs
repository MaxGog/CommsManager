using CommsManager.Core.Enums;
using CommsManager.Core.Models;

namespace CommsManager.Core.Entities;

public class ArtistProfile : BaseEntity
{
    private string _name;
    private string? _description;
    private byte[]? _artistPicture;
    private byte[]? _artistBanner;
    private readonly List<Phones> _phones = [];
    private readonly List<Email> _emails = [];
    private readonly List<SocialLink> _socialLinks = [];
    private readonly List<Commission> _commissions = [];

    public ArtistProfile(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Имя художника не может быть пустым.", nameof(name));

        Id = Guid.NewGuid();
        _name = name;
        CreatedDate = DateTime.UtcNow;
    }

    public string Name
    {
        get => _name;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Имя художника не может быть пустым.", nameof(value));
            _name = value;
        }
    }

    public string? Description
    {
        get => _description;
        set => _description = value;
    }

    public DateTime CreatedDate { get; private set; }

    public byte[]? ArtistPicture
    {
        get => _artistPicture;
        private set => _artistPicture = value;
    }

    public byte[]? ArtistBanner
    {
        get => _artistBanner;
        private set => _artistBanner = value;
    }

    public IReadOnlyCollection<Phones> Phones => _phones.AsReadOnly();
    public IReadOnlyCollection<Email> Emails => _emails.AsReadOnly();
    public IReadOnlyCollection<SocialLink> SocialLinks => _socialLinks.AsReadOnly();
    public IReadOnlyCollection<Commission> Commissions => _commissions.AsReadOnly();

    public void SetArtistPicture(byte[] bytes)
    {
        ArgumentNullException.ThrowIfNull(bytes);
        if (bytes.Length == 0)
            throw new ArgumentException("Аватарка не может быть пустой.", nameof(bytes));

        ArtistPicture = bytes;
    }

    public void ClearArtistPicture() => ArtistPicture = null;

    public void SetArtistBanner(byte[] bytes)
    {
        ArgumentNullException.ThrowIfNull(bytes);
        if (bytes.Length == 0)
            throw new ArgumentException("Баннер не может быть пустым.", nameof(bytes));

        ArtistBanner = bytes;
    }

    public void ClearArtistBanner() => ArtistBanner = null;

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
            throw new ArgumentException("Почта не может быть пустым.", nameof(email));

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

    public void AddCommission(Commission commission)
    {
        ArgumentNullException.ThrowIfNull(commission);
        _commissions.Add(commission);
    }

    public bool RemoveCommission(Commission commission) => _commissions.Remove(commission);

    public void UpdateDescription(string description)
    {
        Description = description;
    }

    public void UpdateProfile(string name, string? description = null)
    {
        Name = name;

        if (description != null)
        {
            Description = description;
        }
    }
}