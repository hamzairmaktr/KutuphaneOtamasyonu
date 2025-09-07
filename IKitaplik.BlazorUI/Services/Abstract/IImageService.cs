﻿using IKitaplik.BlazorUI.Responses;
using IKitaplik.Entities.Concrete;
using IKitaplik.Entities.DTOs.ImagesDTOs;
using IKitaplik.Entities.Enums;

namespace IKitaplik.BlazorUI.Services.Abstract
{
    public interface IImageService
    {
        Task<Response<Image>> Upload(ImageUploadDto imageUploadDto);
        Task<Response<List<Image>>> GetAll(ImageType? type = null, int relationshipId = 0);
        Task<Response> Delete(int id);
    }
}
