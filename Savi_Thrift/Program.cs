using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NLog;
using NLog.Web;
using Savi_Thrift.Application.Interfaces.Services;
using Savi_Thrift.Common.Utilities;
using Savi_Thrift.Domain.Entities;
using Savi_Thrift.Domain.Entities.Helper;
using Savi_Thrift.Infrastructure.Services;
using Savi_Thrift.Persistence.Context;
using CloudinaryDotNet;
using Microsoft.Extensions.Options;
using Savi_Thrift.Domain.Entities;
using Savi_Thrift.Common.Utilities;
using Savi_Thrift.Application;
using Savi_Thrift.Configurations;
using Savi_Thrift.Application.ServicesImplementation;

var builder = WebApplication.CreateBuilder(args);
ConfigurationHelper.InstantiateConfiguration(builder.Configuration);

var configuration = builder.Configuration;
var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
try
{
    // Add services to the container.
    //var configuration = builder.Configuration;
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddDbContext<SaviDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
    builder.Services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<SaviDbContext>().AddDefaultTokenProviders();
    builder.Services.AddScoped<RoleManager<IdentityRole>>();
    builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
    builder.Services.AddTransient<IEmailServices, EmailServices>();
    builder.Services.AddScoped<ICloudinaryServices, CloudinaryServices>();
    //builder.Services.AddScoped<IKycService, KycService>();


    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddSingleton(provider =>
    {

        var cloudinarySettings = provider.GetRequiredService<IOptions<CloudinarySettings>>().Value;

        Account cloudinaryAccount = new(

            cloudinarySettings.CloudName,

            cloudinarySettings.ApiKey,

            cloudinarySettings.ApiSecret);

        return new Cloudinary(cloudinaryAccount);

    });


    builder.Services.AddAuthentication();
    builder.Services.ConfigureAuthentication(configuration);

    builder.Logging.ClearProviders();
    builder.Host.UseNLog();
    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Savi_Thrift v1"));
    }
    using (var scope = app.Services.CreateScope())
    {
        var serviceProvider = scope.ServiceProvider;
        Seeder.SeedRolesAndSuperAdmin(serviceProvider);
    }
    app.UseHttpsRedirection();

    app.UseAuthentication();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    logger.Error(ex, "Something is not right here");
}
finally
{
    NLog.LogManager.Shutdown();
}


