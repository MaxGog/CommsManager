using SQLite;
using System.Text.Json;

namespace CommsManager.Maui.Converters;

public static class SqliteConverters
{
    public static string PhonesToJson(List<CommsManager.Core.Models.Phones> phones)
    {
        return JsonSerializer.Serialize(phones ?? []);
    }

    public static List<CommsManager.Core.Models.Phones> JsonToPhones(string json)
    {
        if (string.IsNullOrEmpty(json))
            return [];

        try
        {
            return JsonSerializer.Deserialize<List<CommsManager.Core.Models.Phones>>(json)
                ?? [];
        }
        catch
        {
            return [];
        }
    }

    public static string EmailsToJson(List<CommsManager.Core.Models.Email> emails)
    {
        return JsonSerializer.Serialize(emails ?? new List<CommsManager.Core.Models.Email>());
    }

    public static List<CommsManager.Core.Models.Email> JsonToEmails(string json)
    {
        if (string.IsNullOrEmpty(json))
            return [];

        try
        {
            return JsonSerializer.Deserialize<List<CommsManager.Core.Models.Email>>(json)
                ?? [];
        }
        catch
        {
            return [];
        }
    }

    public static string AttachmentsToJson(List<CommsManager.Core.Models.OrderAttachment> attachments)
    {
        return JsonSerializer.Serialize(attachments ?? new List<CommsManager.Core.Models.OrderAttachment>());
    }

    public static List<CommsManager.Core.Models.OrderAttachment> JsonToAttachments(string json)
    {
        if (string.IsNullOrEmpty(json))
            return [];

        try
        {
            return JsonSerializer.Deserialize<List<CommsManager.Core.Models.OrderAttachment>>(json)
                ?? [];
        }
        catch
        {
            return [];
        }
    }

    public static string BytesToBase64(byte[]? bytes)
    {
        if (bytes == null || bytes.Length == 0)
            return string.Empty;

        return Convert.ToBase64String(bytes);
    }

    public static byte[]? Base64ToBytes(string? base64)
    {
        if (string.IsNullOrEmpty(base64))
            return null;

        try
        {
            return Convert.FromBase64String(base64);
        }
        catch
        {
            return null;
        }
    }

    public static long DateTimeToUnix(DateTime dateTime)
    {
        return ((DateTimeOffset)dateTime).ToUnixTimeSeconds();
    }

    public static DateTime UnixToDateTime(long unixTime)
    {
        return DateTimeOffset.FromUnixTimeSeconds(unixTime).UtcDateTime;
    }

    public static string GuidToString(Guid guid)
    {
        return guid.ToString();
    }

    public static Guid StringToGuid(string guidString)
    {
        if (Guid.TryParse(guidString, out var guid))
            return guid;

        return Guid.Empty;
    }
}

public class JsonColumnAttribute : Attribute
{
    public Type TargetType { get; }

    public JsonColumnAttribute(Type targetType)
    {
        TargetType = targetType;
    }
}