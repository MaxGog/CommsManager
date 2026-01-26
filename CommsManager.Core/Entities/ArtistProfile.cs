using CommsManager.Core.Enums;
using CommsManager.Core.Models;

namespace CommsManager.Core.Entities;

public class ArtistProfile : BaseEntity
{
    public string Name { get; private set; }
    public string? Description { get; private set; }
    public DateTime CreatedDate { get; private set; }

    public byte[]? ArtistPicture { get; private set; }
    public void SetArtistPicture(byte[] bytes) => ArtistPicture = bytes;
    public byte[]? ArtistBanner { get; private set; }
    public void SetArtistBanner(byte[] bytes) => ArtistBanner = bytes;

    private readonly List<Phones> _phones = [];
    public List<Phones> Phones => _phones;
    public void AddPhone(string phone, string? description, string? type)
        => _phones.Add(new Phones
        {
            NumberPhone = phone,
            Description = description,
            TypePhone = type
        });

    private readonly List<Email> _emails = [];
    public List<Email> Emails => _emails;
    public void AddEmail(string email, string? description, string? type)
        => _emails.Add(new Email
        {
            EmailAdress = email,
            Description = description,
            TypeEmail = type
        });

    private readonly List<SocialLink> _socialLinks = [];
    public List<SocialLink> SocialLinks => _socialLinks;
    public void AddSocialLink(string link, SocialPlatform? type, bool visible = true)
        => _socialLinks.Add(new SocialLink
        {
            Link = link,
            TypeLink = type,
            IsVisible = visible,
            IsActive = visible
        });

    private readonly List<Commission> _commissions = [];
    public List<Commission> Commissions => _commissions;

    public ArtistProfile(string name)
    {
        Id = Guid.NewGuid();
        Name = name;
        CreatedDate = DateTime.UtcNow;
    }
}