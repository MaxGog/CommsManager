namespace CommsManager.Core.Models;

public class SocialLink
{
    public required string Link { get; set; }
    public string? TypeLink { get; set; }
    public bool IsActive { get; set; }
    public bool IsVisible { get; set; }
}

public enum TypeLink
{
    Linktree, X, Bluesky, Instagram, VK, Telegram, YouTube, Facebook
}