public class ArtistProfile
{
    public Guid Id { get; set; }
    public string UserName { get; set; }
    public string Bio { get; set; }
    public string AvatarUrl { get; set; }

    public List<SocialLink> SocialLinks { get; set; }
    public List<PortfolioItem> Portfolio { get; set; }
    public PriceList PriceList { get; set; }

    public string QrCodeImageUrl { get; set; }
    public string PublicProfileUrl { get; set; }
}

public class SocialLink
{
    public string Platform { get; set; }
    public string Url { get; set; }
    public int DisplayOrder { get; set; }
}