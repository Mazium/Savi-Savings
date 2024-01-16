using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Savi_Thrift.Application.DTO;
using Savi_Thrift.Application.Interfaces.Repositories;
using Savi_Thrift.Application.Interfaces.Services;
using Savi_Thrift.Common.Utilities;
using Savi_Thrift.Domain.Entities;

namespace Savi_Thrift.Application
{
	public class CloudinaryServices<TEntity> : ICloudinaryServices<TEntity> where TEntity : class
	{
		private readonly IGenericRepository<TEntity> _repository;
		private readonly Cloudinary _cloudinary;

		public CloudinaryServices(
			IGenericRepository<TEntity> repository,
			IConfiguration configuration)
		{
			_repository = repository ?? throw new ArgumentNullException(nameof(repository));

			var cloudinarySettings = new CloudinarySettings();
			configuration.GetSection("CloudinarySettings").Bind(cloudinarySettings);

			_cloudinary = new Cloudinary(new Account(
				cloudinarySettings.CloudName,
				cloudinarySettings.ApiKey,
				cloudinarySettings.ApiSecret));
		}

		public async Task<CloudinaryUploadResponse> UploadImage(IFormFile file)
		{
			var uploadParams = new ImageUploadParams
			{
				File = new FileDescription(file.FileName, file.OpenReadStream())
			};

			var uploadResult = await _cloudinary.UploadAsync(uploadParams);
			var response = new CloudinaryUploadResponse
			{
				PublicId = uploadResult.PublicId,
				Url = uploadResult.SecureUrl.AbsoluteUri.ToString(),
			};
			return response;
		}

	}

}

