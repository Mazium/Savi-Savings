using CloudinaryDotNet;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Savi_Thrift.Application;
using Savi_Thrift.Application.Interfaces.Repositories;
using Savi_Thrift.Application.Interfaces.Services;
using Savi_Thrift.Application.ServicesImplementation;
using Savi_Thrift.Domain.Entities;
using Savi_Thrift.Domain.Entities.Helper;
using Savi_Thrift.Infrastructure.Services;
using Savi_Thrift.Persistence.Context;
using Savi_Thrift.Persistence.Repositories;

namespace Savi_Thrift.Persistence.Extensions
{
    public static class DIServiceExtension
    {
        public static void AddDependencies(this IServiceCollection services, IConfiguration configuration)
        {
			// Register DbContext
			services.AddDbContext<SaviDbContext>(options =>
				options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

			// Register Identity
			services.AddIdentity<AppUser, IdentityRole>()
				.AddEntityFrameworkStores<SaviDbContext>()
				.AddDefaultTokenProviders();

			// Register RoleManager
			services.AddScoped<RoleManager<IdentityRole>>();
			services.AddScoped<IWalletService, WalletService>();
			services.AddScoped<ISavingService, SavingService>();
			services.AddScoped<IUserService, UserService>();
			services.AddScoped<IAuthenticationServices, AuthenticationServices>();

			// Register Email services
			var emailSettings = new EmailSettings();
			configuration.GetSection("EmailSettings").Bind(emailSettings);
			services.AddSingleton(emailSettings);
			services.AddTransient<IEmailServices, EmailServices>();
			services.AddSingleton<IHttpContextAccessor , HttpContextAccessor>();

			// Register Cloudinary services
			var cloudinarySettings = new CloudinarySettings();
			configuration.GetSection("CloudinarySettings").Bind(cloudinarySettings);
			services.AddSingleton(cloudinarySettings);
			services.AddSingleton(provider =>
			{
				var account = new Account(
					cloudinarySettings.CloudName,
					cloudinarySettings.ApiKey,
					cloudinarySettings.ApiSecret);
				return new Cloudinary(account);
			});

			// Example in Startup.cs
			services.AddScoped(typeof(ICloudinaryServices<>), typeof(CloudinaryServices<>));



			// Register GenericRepository
			services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));

			// Register GroupService
			services.AddScoped<IGroupService, GroupService>();
			services.AddScoped<IUnitOfWork, UnitOfWork>();

		}
    }
}