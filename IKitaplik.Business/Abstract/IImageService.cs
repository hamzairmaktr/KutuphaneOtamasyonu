using Core.Utilities.Results;
using IKitaplik.Entities.Concrete;
using IKitaplik.Entities.DTOs.ImagesDTOs;
using IKitaplik.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKitaplik.Business.Abstract
{
    public interface IImageService
    {
        Task<IDataResult<List<Image>>> UploadAsync(ImageUploadDto imageUploadDto);
        Task<IDataResult<Image>> GetByIdAsync(int id);
        Task<IDataResult<List<Image>>> GetAllAsync(ImageType? type = null, int? relationshipId = 0);
        Task<IResult> DeleteAsync(int id);
        Task<IResult> DeleteRangeAsync(int[] ids);
        Task<IResult> DeleteAllAsync(ImageType type, int relationshipId);
        Task<IResult> SetPrimaryAsync(int id);
    }
}