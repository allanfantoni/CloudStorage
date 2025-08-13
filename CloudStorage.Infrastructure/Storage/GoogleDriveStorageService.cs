using CloudStorage.Domain.Entities;
using CloudStorage.Domain.Storage;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Upload;
using Microsoft.AspNetCore.Http;

namespace CloudStorage.Infrastructure.Storage;

public class GoogleDriveStorageService : IStorageService
{
    private readonly GoogleAuthorizationCodeFlow _authorization;
    private readonly string _applicationName;

    public GoogleDriveStorageService(GoogleAuthorizationCodeFlow authorization, string applicationName)
    {
        _authorization = authorization;
        _applicationName = applicationName;
    }

    public string Upload(IFormFile file, User user)
    {
        var credential = new UserCredential(_authorization, user.Email, new TokenResponse
        {
            AccessToken = user.AccessToken,
            RefreshToken = user.RefreshToken
        });

        var service = new DriveService(new BaseClientService.Initializer
        {
            ApplicationName = _applicationName,
            HttpClientInitializer = credential
        });
        
        var driveFile = new Google.Apis.Drive.v3.Data.File
        {
            Name = file.Name,
            MimeType = file.ContentType
            // Parents = new List<string> { "appDataFolder" } // Use appDataFolder for private files
        };
        
        var command = service.Files.Create(driveFile, file.OpenReadStream(), file.ContentType);
        command.Fields = "id";

        var response = command.Upload();

        if (response.Status is not UploadStatus.Completed
            or UploadStatus.NotStarted)
            throw new Exception($"Failed to upload file: {response.Exception?.Message}");

        return command.ResponseBody.Id;
    }
}