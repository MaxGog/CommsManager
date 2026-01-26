using CommsManager.Core.Enums;

namespace CommsManager.Core.Models;

public class SocialLink
{
    public required string Link { get; set; }
    public SocialPlatform? TypeLink { get; set; }
    public bool IsActive { get; set; }
    public bool IsVisible { get; set; }
}