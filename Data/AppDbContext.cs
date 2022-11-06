using Microsoft.EntityFrameworkCore;
using UkrainianLyrics.Shared.Models;

namespace UkrainianLyrics.WebAPI.Data;

public class AppDbContext : DbContext
{
    public static string DbDirectoryPath { get => Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "UkrainianLyrics-WebAPI"); }
    public static string DbPath { get => Path.Join(DbDirectoryPath, "UkrainianLyrics-WebAPI.db"); }

    public DbSet<Author> Authors { get; init; } = null!;

    public DbSet<Composition> Compositons { get; init; } = null!;

    public AppDbContext(DbContextOptions options) : base(options)
    {
        Authors.Include(w => w.Writings).ToList();
    }
}