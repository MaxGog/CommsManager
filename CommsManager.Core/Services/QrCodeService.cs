public class QrCodeService
{
    public byte[] GenerateQrCode(string url)
    {
        using var qrGenerator = new QRCodeGenerator();
        var qrCodeData = qrGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);
        var qrCode = new PngByteQRCode(qrCodeData);
        return qrCode.GetGraphic(20);
    }

    public string GetProfileUrl(string userName)
    {
        return $"https://commsmanager.app/profile/{userName}";
    }
}