using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Savi_Thrift.Application.Interfaces.Services;
using Savi_Thrift.Domain.Entities.Helper;
using Savi_Thrift.Infrastructure.Services;
using Savi_Thrift.Persistence.Extensions;
using NLog;
using NLog.Web;
using Microsoft.EntityFrameworkCore;
using Savi_Thrift.Persistence.Context;
using CloudinaryDotNet;
using Microsoft.Extensions.Options;
using Savi_Thrift.Domain.Entities;
using Savi_Thrift.Common.Utilities;
using Savi_Thrift.Application;

var builder = WebApplication.CreateBuilder(args);
ConfigurationHelper.InstantiateConfiguration(builder.Configuration);

var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
try
{

    // Add services to the container.
    var configuration = builder.Configuration;
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddDbContext<SaviDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

    builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
    builder.Services.AddTransient<IEmailServices, EmailServices>();
    builder.Services.AddScoped<ICloudinaryServices, CloudinaryServices>();


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



    builder.Logging.ClearProviders();
    builder.Host.UseNLog();
  var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Savi_Thrift v1"));
}
    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch(Exception ex)
{
    logger.Error(ex, "Something is not right here");    
}
finally
{
    NLog.LogManager.Shutdown();
}


