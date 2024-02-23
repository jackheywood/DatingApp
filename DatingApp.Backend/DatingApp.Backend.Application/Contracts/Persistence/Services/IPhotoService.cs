using DatingApp.Backend.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace DatingApp.Backend.Application.Contracts.Persistence.Services;

public interface IPhotoService
{
    Task<Photo> AddPhotoAsync(IFormFile file);
    Task DeletePhotoAsync(string publicId);
}
