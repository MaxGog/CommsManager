using CommsManager.Core.Enums;

namespace CommsManager.Core.Models;

public class OrderAttachment
{
    public string? Name { get; set; }
    public required byte[] Attachment { get; set; }
    public AttachmentType? TypeAttachment { get; set; }
    public string? Description { get; set; }
}