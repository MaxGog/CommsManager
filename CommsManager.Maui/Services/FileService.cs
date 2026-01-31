using CommsManager.Maui.Data.Models;
using CommsManager.Maui.Enums;
using CommsManager.Maui.Interfaces;

namespace CommsManager.Maui.Services;

public class FileService : IFileService
{
    private readonly string _imagesDirectory;
    private readonly string _documentsDirectory;
    private readonly string _backupsDirectory;
    private readonly string _tempDirectory;

    public FileService()
    {
        _imagesDirectory = Path.Combine(FileSystem.AppDataDirectory, "images");
        _documentsDirectory = Path.Combine(FileSystem.AppDataDirectory, "documents");
        _backupsDirectory = Path.Combine(FileSystem.AppDataDirectory, "backups");
        _tempDirectory = Path.Combine(FileSystem.CacheDirectory, "temp");

        EnsureDirectories();
    }

    private void EnsureDirectories()
    {
        Directory.CreateDirectory(_imagesDirectory);
        Directory.CreateDirectory(_documentsDirectory);
        Directory.CreateDirectory(_backupsDirectory);
        Directory.CreateDirectory(_tempDirectory);
    }

    public async Task<string?> PickImageAsync()
    {
        try
        {
            var result = await FilePicker.Default.PickAsync(new PickOptions
            {
                PickerTitle = "Выберите изображение",
                FileTypes = FilePickerFileType.Images
            });

            return result?.FullPath;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error picking image: {ex.Message}");
            return null;
        }
    }

    public async Task<string?> TakePhotoAsync()
    {
        try
        {
            if (!MediaPicker.Default.IsCaptureSupported)
                return null;

            var photo = await MediaPicker.Default.CapturePhotoAsync();
            return photo?.FullPath;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error taking photo: {ex.Message}");
            return null;
        }
    }

    public async Task<byte[]> LoadImageBytesAsync(string filePath)
    {
        if (!File.Exists(filePath))
            return Array.Empty<byte>();

        return await File.ReadAllBytesAsync(filePath);
    }

    public async Task<string> SaveImageAsync(byte[] imageData, string fileName)
    {
        var filePath = Path.Combine(_imagesDirectory, fileName);
        await File.WriteAllBytesAsync(filePath, imageData);
        return filePath;
    }

    public async Task<string> SaveImageForEntityAsync(byte[] imageData, Guid entityId, EntityImageType type)
    {
        var extension = GetImageExtension(imageData);
        var fileName = $"{type}_{entityId}_{DateTime.Now.Ticks}{extension}";
        return await SaveImageAsync(imageData, fileName);
    }

    private string GetImageExtension(byte[] imageData)
    {
        if (imageData.Length < 4)
            return ".jpg";

        if (imageData[0] == 0xFF && imageData[1] == 0xD8)
            return ".jpg";

        if (imageData[0] == 0x89 && imageData[1] == 0x50 &&
            imageData[2] == 0x4E && imageData[3] == 0x47)
            return ".png";

        if (imageData[0] == 0x47 && imageData[1] == 0x49 &&
            imageData[2] == 0x46 && imageData[3] == 0x38)
            return ".gif";

        if (imageData[0] == 0x42 && imageData[1] == 0x4D)
            return ".bmp";

        return ".jpg";
    }

    public async Task<bool> DeleteImageAsync(string filePath)
    {
        try
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                return true;
            }
            return false;
        }
        catch
        {
            return false;
        }
    }

    public Task<List<string>> GetEntityImagesAsync(Guid entityId)
    {
        var images = Directory.GetFiles(_imagesDirectory, $"*{entityId}*")
            .OrderByDescending(f => f)
            .ToList();

        return Task.FromResult(images);
    }

    public async Task<string?> PickDocumentAsync()
    {
        try
        {
            var result = await FilePicker.Default.PickAsync(new PickOptions
            {
                PickerTitle = "Выберите документ",
                FileTypes = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                    { DevicePlatform.WinUI, new[] { ".pdf", ".doc", ".docx", ".xls", ".xlsx" } },
                    { DevicePlatform.Android, new[] { "application/pdf", "application/msword", "application/vnd.ms-excel" } },
                    { DevicePlatform.iOS, new[] { "public.data" } }
                })
            });

            return result?.FullPath;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error picking document: {ex.Message}");
            return null;
        }
    }

    public async Task<string> SaveDocumentAsync(byte[] documentData, string fileName)
    {
        var filePath = Path.Combine(_documentsDirectory, fileName);
        await File.WriteAllBytesAsync(filePath, documentData);
        return filePath;
    }

    public async Task<string> CreateBackupAsync()
    {
        var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
        var backupFileName = $"backup_{timestamp}.db3";
        var backupPath = Path.Combine(_backupsDirectory, backupFileName);

        var dbPath = Path.Combine(FileSystem.AppDataDirectory, "commsmanager.db3");

        if (File.Exists(dbPath))
        {
            File.Copy(dbPath, backupPath, true);
        }

        return backupPath;
    }

    public async Task<bool> RestoreBackupAsync(string backupPath)
    {
        try
        {
            if (!File.Exists(backupPath))
                return false;

            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "commsmanager.db3");
            File.Copy(backupPath, dbPath, true);

            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<string> ExportDataAsync(ExportFormat format)
    {
        var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
        var fileName = $"export_{timestamp}.{format.ToString().ToLower()}";
        var exportPath = Path.Combine(_tempDirectory, fileName);

        await File.WriteAllTextAsync(exportPath, string.Empty);

        return exportPath;
    }

    public async Task<bool> ImportDataAsync(string filePath)
    {
        try
        {
            if (!File.Exists(filePath))
                return false;

            var importFileName = $"import_{DateTime.Now.Ticks}.tmp";
            var importPath = Path.Combine(_tempDirectory, importFileName);
            File.Copy(filePath, importPath);

            return true;
        }
        catch
        {
            return false;
        }
    }

    public Task<string> GetTempFilePathAsync(string extension = ".tmp")
    {
        var fileName = $"temp_{Guid.NewGuid()}{extension}";
        var filePath = Path.Combine(_tempDirectory, fileName);
        return Task.FromResult(filePath);
    }

    public Task CleanupTempFilesAsync()
    {
        try
        {
            var tempFiles = Directory.GetFiles(_tempDirectory, "*.*");
            foreach (var file in tempFiles)
            {
                try
                {
                    File.Delete(file);
                }
                catch
                {
                }
            }
        }
        catch
        {
        }

        return Task.CompletedTask;
    }
}