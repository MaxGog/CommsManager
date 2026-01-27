using SQLite;
using System.Text.Json;
using CommsManager.Core.Models;

namespace CommsManager.Maui.Data.Models;

[Table("Customers")]
public class LocalCustomer
{
    [PrimaryKey]
    public Guid Id { get; set; }

    [NotNull, MaxLength(200), Indexed]
    public string Name { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string? Description { get; set; }

    [MaxLength(500)]
    public string? Communication { get; set; }

    public string? CustomerPictureBase64 { get; set; }

    public string? CustomerPicturePath { get; set; }

    [NotNull]
    public DateTime CreatedDate { get; set; }

    [NotNull, Indexed]
    public DateTime UpdatedDate { get; set; }

    [NotNull, Indexed]
    public bool IsActive { get; set; } = true;

    public string? PhonesJson { get; set; }
    public string? EmailsJson { get; set; }
    public string? SocialLinksJson { get; set; }

    [Ignore]
    public List<Phones> Phones
    {
        get => !string.IsNullOrEmpty(PhonesJson)
            ? JsonSerializer.Deserialize<List<Phones>>(PhonesJson) ?? []
            : [];
        set => PhonesJson = JsonSerializer.Serialize(value ?? []);
    }

    [Ignore]
    public List<Core.Models.Email> Emails
    {
        get => !string.IsNullOrEmpty(EmailsJson)
            ? JsonSerializer.Deserialize<List<Core.Models.Email>>(EmailsJson) ?? []
            : [];
        set => EmailsJson = JsonSerializer.Serialize(value ?? []);
    }

    [Ignore]
    public List<SocialLink> SocialLinks
    {
        get => !string.IsNullOrEmpty(SocialLinksJson)
            ? JsonSerializer.Deserialize<List<SocialLink>>(SocialLinksJson) ?? new List<SocialLink>()
            : new List<SocialLink>();
        set => SocialLinksJson = JsonSerializer.Serialize(value ?? new List<SocialLink>());
    }

    [Ignore]
    public bool HasPicture => !string.IsNullOrEmpty(CustomerPictureBase64) ||
                              !string.IsNullOrEmpty(CustomerPicturePath);

    [Ignore]
    public string DisplayName => !string.IsNullOrEmpty(Name) ? Name : "Без имени";

    [Ignore]
    public int OrderCount { get; set; }

    public void AddPhone(Phones phone)
    {
        var phones = Phones;
        phones.Add(phone);
        Phones = phones;
        UpdatedDate = DateTime.UtcNow;
    }

    public void RemovePhone(Phones phone)
    {
        var phones = Phones;
        phones.Remove(phone);
        Phones = phones;
        UpdatedDate = DateTime.UtcNow;
    }

    public void AddEmail(Core.Models.Email email)
    {
        var emails = Emails;
        emails.Add(email);
        Emails = emails;
        UpdatedDate = DateTime.UtcNow;
    }

    public void SetPictureFromBytes(byte[] imageBytes)
    {
        CustomerPictureBase64 = Convert.ToBase64String(imageBytes);
        UpdatedDate = DateTime.UtcNow;
    }

    public byte[]? GetPictureBytes()
    {
        if (string.IsNullOrEmpty(CustomerPictureBase64))
            return null;

        return Convert.FromBase64String(CustomerPictureBase64);
    }
}