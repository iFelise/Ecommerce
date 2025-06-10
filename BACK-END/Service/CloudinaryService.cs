using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

public class CloudinaryService
{
    private readonly Cloudinary _cloudinary;

    public CloudinaryService(IConfiguration configuration)
    {
        // Primero intenta obtener desde appsettings.json
        var cloudName = configuration["Cloudinary:CloudName"];
        var apiKey = configuration["Cloudinary:ApiKey"];
        var apiSecret = configuration["Cloudinary:ApiSecret"];

        // Si alguno está vacío, intenta desde variables de entorno
        cloudName ??= Environment.GetEnvironmentVariable("CLOUDINARY_CLOUD_NAME");
        apiKey ??= Environment.GetEnvironmentVariable("CLOUDINARY_API_KEY");
        apiSecret ??= Environment.GetEnvironmentVariable("CLOUDINARY_API_SECRET");

        if (string.IsNullOrWhiteSpace(cloudName) ||
            string.IsNullOrWhiteSpace(apiKey) ||
            string.IsNullOrWhiteSpace(apiSecret))
        {
            throw new ArgumentException("❌ Configuración de Cloudinary no encontrada ni en appsettings.json ni en variables de entorno.");
        }

        var account = new Account(cloudName, apiKey, apiSecret);
        _cloudinary = new Cloudinary(account);
    }

    // Métodos Upload y Delete como ya los tienes...
    public async Task<ImageUploadResult> UploadImageAsync(IFormFile file)
    {
        using var stream = file.OpenReadStream();
        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(file.FileName, stream),
            Folder = "users"
        };
        return await _cloudinary.UploadAsync(uploadParams);
    }

    public async Task<ImageUploadResult> UploadImageFromUrlAsync(string imageUrl)
    {
        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(imageUrl),
            Folder = "users"
        };
        return await _cloudinary.UploadAsync(uploadParams);
    }

    public async Task<DeletionResult> DeleteImageAsync(string publicId)
    {
        return await _cloudinary.DestroyAsync(new DeletionParams(publicId));
    }
}
