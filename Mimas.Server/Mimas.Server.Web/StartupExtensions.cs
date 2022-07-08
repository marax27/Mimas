using MediatR;
using Microsoft.Extensions.Options;
using Mimas.Server.Application.Features;
using Mimas.Server.Application.Ports;
using Mimas.Server.Infrastructure;
using Mimas.Server.Infrastructure.Repositories;

namespace Mimas.Server.Web;

public static class StartupExtensions
{
    public static WebApplicationBuilder SetupApplicationBuilder(this WebApplicationBuilder builder)
    {
        builder.Configuration.AddUserSecrets<Program>();

        // Add services to the container.

        RegisterDatabaseConnectionString(builder.Configuration, builder.Services);

        _ = new GetAllItemsQuery(); // hack
        builder.Services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());

        builder.Services.AddTransient<IItemRepository, DbItemRepository>();
        builder.Services.AddTransient<IOwnerRepository, DbOwnerRepository>();

        builder.Services.AddRazorPages();

        return builder;
    }

    public static WebApplication SetupApplication(this WebApplication app)
    {
        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.MapRazorPages();

        return app;
    }

    private static void RegisterDatabaseConnectionString(IConfiguration configuration, IServiceCollection services)
    {
        var dbUser = configuration["MimasDatabase:UserId"];
        var dbPassword = configuration["MimasDatabase:Password"];
        var dbConnectionString = configuration.GetConnectionString("MimasDatabase") +
                                 $"User ID={dbUser};Password={dbPassword};";

        services.AddSingleton(Options.Create(new DatabaseConnectionSettings(dbConnectionString)));
    }
}
