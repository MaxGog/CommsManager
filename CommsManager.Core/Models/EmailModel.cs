namespace CommsManager.Core.Models;

public class Email
{
    public required string EmailAdress { get; set; }
    public string? TypeEmail { get; set; }
    public string? Description { get; set; }
}