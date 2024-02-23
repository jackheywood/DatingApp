using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DatingApp.Backend.Application.Contracts.Persistence.Services;
using DatingApp.Backend.Application.Exceptions;
using DatingApp.Backend.Domain.Entities;
using DatingApp.Backend.Infrastructure.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace DatingApp.Backend.Infrastructure.Persistence.Services;

public class PhotoService : IPhotoService
{
    private readonly Cloudinary _cloudinary;

    public PhotoService(IOptions<CloudinarySettings> config)
    {
        var account = new Account(config.Value.CloudName, config.Value.ApiKey, config.Value.ApiSecret);
        _cloudinary = new Cloudinary(account);
    }

    public async Task<Photo> AddPhotoAsync(IFormFile file)
    {
        if (file.Length <= 0) throw new PhotoUploadException("File is empty");

        await using var stream = file.OpenReadStream();

        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(file.FileName, stream),
            Transformation = new Transformation().Height(500).Width(500).Crop("fill").Gravity("face"),
            Folder = "DatingApp",
        };

        var result = await _cloudinary.UploadAsync(uploadParams);
        if (result.Error is not null) throw new PhotoUploadException(result.Error.Message);

        return new Photo
        {
            Url = result.SecureUrl.AbsoluteUri,
            PublicId = result.PublicId,
        };
    }

    public async Task DeletePhotoAsync(string publicId)
    {
        var deleteParams = new DeletionParams(publicId);
        var result = await _cloudinary.DestroyAsync(deleteParams);
        if (result.Error is not null) throw new PhotoUploadException(result.Error.Message);
    }
}
