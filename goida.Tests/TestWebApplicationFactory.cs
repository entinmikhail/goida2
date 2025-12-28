using System.Data.Common;
using goida.Data;
using goida.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace goida.Tests;

public class TestWebApplicationFactory : WebApplicationFactory<Program>
{
    private DbConnection? _connection;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((_, config) =>
        {
            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Database:ApplyMigrations"] = "false",
                ["Jwt:Issuer"] = "goida",
                ["Jwt:Audience"] = "goida",
                ["Jwt:Key"] = "test_key_1234567890",
                ["Jwt:ExpiresMinutes"] = "120"
            });
        });

        builder.ConfigureServices(services =>
        {
            services.RemoveAll(typeof(DbContextOptions<ApplicationDbContext>));
            services.RemoveAll(typeof(ApplicationDbContext));

            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlite(_connection);
            });

            var storagePath = Path.Combine(Path.GetTempPath(), "goida-tests", Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(storagePath);
            services.Configure<FileStorageOptions>(options =>
            {
                options.RootPath = storagePath;
                options.MaxFileSizeBytes = 10 * 1024 * 1024;
                options.AllowedContentTypes = ["application/pdf"];
            });
        });

        builder.UseEnvironment("Development");
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        _connection?.Dispose();
    }
}
