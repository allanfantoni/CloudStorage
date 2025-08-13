using CloudStorage.Domain.Entities;
using CloudStorage.Domain.Storage;
using FileTypeChecker;
using Microsoft.AspNetCore.Http;

namespace CloudStorage.Application.UseCases.Users.UploadProfilePhoto;

public class UploadProfilePhotoUseCase : IUploadProfilePhotoUseCase
{
    private readonly IStorageService _storageService;

    public UploadProfilePhotoUseCase(IStorageService storageService)
    {
        _storageService = storageService;
    }

    public void Execute(IFormFile file)
    {
        var fileStream = file.OpenReadStream();
        var isImage = FileTypeValidator.IsImage(fileStream);

        if (!isImage)
            throw new Exception("Invalid file type. Only image files are allowed.");

        var user = GetFromDatabase();

        _storageService.Upload(file, user);
    }

    private User GetFromDatabase()
    {
        return new User
        {
            Id = 1,
            Name = "[EnterYourName]",
            Email = "[EnterYourEmail]",
            AccessToken = "[EnterYourAccessToken]",
            RefreshToken = "[EnterYourRefreshToken]"
        };
    }
}