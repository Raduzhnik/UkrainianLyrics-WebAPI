using Microsoft.EntityFrameworkCore;
using UkrainianLyrics.WebAPI.Data;

namespace UkrainianLyrics.WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            Directory.CreateDirectory(AppDbContext.DbDirectoryPath);
            Directory.CreateDirectory(LuceneSearchEngine.IndexDirectoryPath);

            builder.Services
                .AddDbContext<AppDbContext>(options
                => options.UseSqlite($"Data Source={AppDbContext.DbPath}"));

            builder.Services
                .AddScoped<IAppRepository, AppRepository>()
                .AddSingleton<LuceneSearchEngine>();

            builder.Services.AddControllers();

            var app = builder.Build();

            app.UseAuthorization();

            app.MapControllers();

            app.Services
                .GetRequiredService<ILogger<AppDbContext>>()
                .LogInformation("Database is based at: {Database Path}", AppDbContext.DbPath);

            app.Run();
        }
    }
}