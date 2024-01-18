using NLog;
using NLog.Web;
using Savi_Thrift.Common.Utilities;
using Savi_Thrift.Configurations;
using Savi_Thrift.Mapper;
using Savi_Thrift.Persistence.Extensions;

var builder = WebApplication.CreateBuilder(args);
ConfigurationHelper.InstantiateConfiguration(builder.Configuration);

var configuration = builder.Configuration;
var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
try
{
    // Add services to the container.
   
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

	// Register SaviThrift services using the extension class
	builder.Services.AddDependencies(configuration);
    builder.Services.AddMailService(configuration);
    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddAuthentication();
    builder.Services.ConfigureAuthentication(configuration);
	builder.Services.AddAutoMapper(typeof(MapperProfile));
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
        await Seeder.SeedRolesAndSuperAdmin(serviceProvider);
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


