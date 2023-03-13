using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Konsten.Models;


namespace Konsten.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    public DbSet<Konsten.Models.Artwork> Artwork { get; set; } = default!;
    public DbSet<Konsten.Models.ArtistName> ArtistName { get; set; } = default!;

}
