using System.Text;
using CityManagerApi.Data;
using CityManagerApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace CityManagerApi.Services;

public class CityService : ICityService
{
    private readonly AppDbContext _context;

    public CityService(AppDbContext context)
    {
        _context = context;
    }

    public async Task UploadCitiesFromTxtAsync(IFormFile file)
    {
        if (file == null || file.Length == 0)
            throw new ArgumentException("File is empty");

        var citiesToAdd = new List<City>();

        using (var stream = file.OpenReadStream())
        using (var reader = new StreamReader(stream, Encoding.UTF8))
        {
            string? line;
            while ((line = await reader.ReadLineAsync()) != null)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;

                var parts = line.Split(';');
                if (parts.Length >= 2 && int.TryParse(parts[1].Trim(), out int population))
                {
                    citiesToAdd.Add(new City
                    {
                        Name = parts[0].Trim(),
                        Population = population
                    });
                }
            }
        }

        if (citiesToAdd.Any())
        {
            await _context.Cities.AddRangeAsync(citiesToAdd);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<(byte[] FileBytes, string ContentType, string FileName)> DownloadCitiesToTxtAsync()
    {
        var cities = await _context.Cities.ToListAsync();

        var sb = new StringBuilder();
        foreach (var city in cities)
        {
            sb.AppendLine($"{city.Name};{city.Population}");
        }

        byte[] fileBytes = Encoding.UTF8.GetBytes(sb.ToString());
        string contentType = "text/plain";
        string fileName = $"cities_{DateTime.Now:yyyyMMdd_HHmmss}.txt";

        return (fileBytes, contentType, fileName);
    }
}