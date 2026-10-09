using CityManagerApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CityManagerApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CitiesController : ControllerBase
{
    private readonly ICityService _cityService;

    public CitiesController(ICityService cityService)
    {
        _cityService = cityService;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        try
        {
            await _cityService.UploadCitiesFromTxtAsync(file);
            return Ok(new { message = "Data saved successfully" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpGet("download")]
    public async Task<IActionResult> Download()
    {
        var (fileBytes, contentType, fileName) = await _cityService.DownloadCitiesToTxtAsync();
        return File(fileBytes, contentType, fileName);
    }
}