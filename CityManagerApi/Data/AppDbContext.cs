using CityManagerApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace CityManagerApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<City> Cities { get; set; } = null!;

    internal async Task SaveChangesAsync()
    {
        throw new NotImplementedException();
    }
}