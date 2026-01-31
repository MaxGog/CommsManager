using CommsManager.Maui.Enums;
using CommsManager.Maui.Services;

namespace CommsManager.Maui.Interfaces;

public interface IFileService
{
    Task<string?> PickImageAsync();
    Task<string?> TakePhotoAsync();
    Task<byte[]> LoadImageBytesAsync(string filePath);
    Task<string> SaveImageAsync(byte[] imageData, string fileName);
    Task<string> SaveImageForEntityAsync(byte[] imageData, Guid entityId, EntityImageType type);
    Task<bool> DeleteImageAsync(string filePath);
    Task<List<string>> GetEntityImagesAsync(Guid entityId);

    Task<string?> PickDocumentAsync();
    Task<string> SaveDocumentAsync(byte[] documentData, string fileName);

    Task<string> CreateBackupAsync();
    Task<bool> RestoreBackupAsync(string backupPath);

    Task<string> ExportDataAsync(ExportFormat format);
    Task<bool> ImportDataAsync(string filePath);

    Task<string> GetTempFilePathAsync(string extension = ".tmp");
    Task CleanupTempFilesAsync();
}