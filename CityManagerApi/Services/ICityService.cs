using Microsoft.AspNetCore.Http;

namespace CityManagerApi.Services;

public interface ICityService
{
    Task UploadCitiesFromTxtAsync(IFormFile file);
    Task<(byte[] FileBytes, string ContentType, string FileName)> DownloadCitiesToTxtAsync();
}