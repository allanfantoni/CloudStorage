using CloudStorage.Domain.Entities;
using CloudStorage.Domain.Storage;
using Microsoft.AspNetCore.Http;

namespace CloudStorage.Infrastructure.Storage;

public class DropboxStorageService : IStorageService
{
    public string Upload(IFormFile file, User user)
    {
        throw new NotImplementedException();
    }
}