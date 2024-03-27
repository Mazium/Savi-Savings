﻿using CloudinaryDotNet;
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
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.Cookies;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Builder;

namespace Savi_Thrift.Persistence.Extensions
{
    public static class DIServiceExtension
    {
        public static void AddDependencies(this IServiceCollection services, IConfiguration configuration)
		{
			// Register DbContext
			services.AddDbContext<SaviDbContext>(options =>
				options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
			


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
			services.AddScoped<IKycService, KycService>();
			services.AddScoped<IWalletService, WalletService>();
			services.AddScoped<IRecurringGroupJobs, RecurringGroupJobs>();
            services.AddTransient<WalletService>();
			services.AddScoped<IGroupTransactionService, GroupTransactionService>();
			// Register Email services
			var emailSettings = new EmailSettings();
			configuration.GetSection("EmailSettings").Bind(emailSettings);
			services.AddSingleton(emailSettings);
			services.AddTransient<IEmailServices, EmailServices>();
			

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
			services.AddScoped<IGroupSavingsService, GroupSavingsService>();
			services.AddScoped<IGroupMembersService, GroupMembersService>();
			
			services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IUserTransactionServices, UserTransactionServices>();

			services.AddScoped<IDefaultingUserService , DefaultingUserService>();

			

			
			services.AddAuthentication(options =>
			{
				options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
			})
                .AddCookie(options =>
                {
                    options.LoginPath = "/api/Authentication/Login";
                })
                .AddGoogle(GoogleDefaults.AuthenticationScheme, options =>
				{
					options.ClientId = "667855328126-eflhj0idirqrejsvfm1616prpbpfr03j.apps.googleusercontent.com";
					options.ClientSecret = "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";
					options.CallbackPath = "/api/Authentication/signin-google/token";
				});

            

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });

            //configure Hangfire
            services.AddHangfire(x =>
            {
                string connectionString = configuration.GetConnectionString("DefaultConnection");
                if (string.IsNullOrEmpty(connectionString))
                {
                     throw new InvalidOperationException("Connection string is null or empty.");
                }
                else
                {
                    x.UsePostgreSqlStorage(connectionString);
                }
            });
            services.AddHangfireServer();
        }
        public static void ApplyMigrations(this IApplicationBuilder app)
        {
            using IServiceScope scope = app.ApplicationServices.CreateScope();
            using SaviDbContext dbContext =
                scope.ServiceProvider.GetRequiredService<SaviDbContext>();

            dbContext.Database.Migrate();
        }
    }
}