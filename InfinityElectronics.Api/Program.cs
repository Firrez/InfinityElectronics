using InfinityElectronics.Data;
using InfinityElectronics.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;

namespace InfinityElectronics;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(opt =>
        {
            opt.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
            {
                Name = "X-API-KEY",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Description = "API Key"
            });
            
            opt.AddSecurityRequirement(doc => new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecuritySchemeReference("ApiKey", doc),
                    []
                }
            });
        });

        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite("Data Source=infinity.db"));

        builder.Services.AddHttpClient<ErpSyncService>();
        
        builder.Services.AddScoped<IProductService, ProductService>();
        
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        using (var scope = app.Services.CreateScope())
        {
            var syncService = scope.ServiceProvider.GetRequiredService<ErpSyncService>();
            await syncService.SyncAsync();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}