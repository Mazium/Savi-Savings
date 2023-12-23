﻿using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Http;
using Savi.Core.IServices;
using Savi.Model.Entities;
using Microsoft.Extensions.Configuration;
using Savi.Data.Repositories.Interface;
using Savi_Thrift.Application.Interfaces.Repositories;

namespace Savi.Core.Services
    {
        public class CloudinaryServices<T> : ICloudinaryServices<T> where T : class
        {
            private readonly IGenericRepository<T> _repository;
            private readonly Cloudinary _cloudinary;
            public CloudinaryServices(IGenericRepository<T> repository, IConfiguration configuration)
            {
                _repository = repository ?? throw new ArgumentNullException(nameof(repository));

                var cloudinarySettings = configuration.GetSection("CloudinarySettings").Get<CloudinarySettings>();

                _cloudinary = new Cloudinary(new Account(
                    cloudinarySettings.CloudName,
                    cloudinarySettings.ApiKey,
                    cloudinarySettings.ApiSecret
                ));
            }
            public async Task<string> UploadImage(string entityId, IFormFile file)
            {
                var entity = _repository.GetByIdAsync(entityId);

                if (entity == null)
                {
                    return $"{typeof(T).Name} not found";
                }
                var upload = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, file.OpenReadStream())
                };
                var uploadResult = await _cloudinary.UploadAsync(upload);
                _repository.UpdateAsync(entity);
                try
                {
                    _repository.SaveChangesAsync();
                    return uploadResult.SecureUrl.AbsoluteUri;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex}");
                    return "Database update error occurred";
                }
            }
        }

    }

