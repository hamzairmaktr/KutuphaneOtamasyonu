using Core.Entities;
using Core.Utilities.Results;
using IKitaplik.Business.Abstract;
using IKitaplik.DataAccess.Abstract;
using IKitaplik.DataAccess.Concrete.EntityFramework;
using IKitaplik.DataAccess.UnitOfWork;
using IKitaplik.Entities.Concrete;
using IKitaplik.Entities.DTOs.ImagesDTOs;
using IKitaplik.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKitaplik.Business.Concrete
{
    public class ImageManager : IImageService
    {
        private readonly string _uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
        private readonly IUnitOfWork _unitOfWork;
        public ImageManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IResult> DeleteAllAsync(ImageType type, int relationshipId)
        {
            try
            {
                var listR = await GetAllAsync(type, relationshipId);
                if (listR.Success)
                {
                    foreach (var item in listR.Data)
                    {
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", item.FilePath.TrimStart('/'));
                        if (File.Exists(filePath))
                            File.Delete(filePath);
                    }
                    await _unitOfWork.Images.DeleteRangeAsync(listR.Data);
                    return new SuccessResult("Resimler başarı ile silindi");
                }
                return new ErrorResult(listR.Message);
            }
            catch (Exception ex)
            {
                return new ErrorResult("Resimler silinirken hata oluştu : " + ex.Message);
            }
        }

        public async Task<IResult> DeleteAsync(int id)
        {
            try
            {
                var entityRes = await GetByIdAsync(id);
                if (!entityRes.Success)
                {
                    return new ErrorResult(entityRes.Message);
                }
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", entityRes.Data.FilePath.TrimStart('/'));
                if (File.Exists(filePath))
                    File.Delete(filePath);
                await _unitOfWork.Images.DeleteAsync(entityRes.Data);
                return new SuccessResult("Resim silindi");
            }
            catch (Exception ex)
            {
                return new ErrorResult("Resim silinirken hata oluştu : " + ex.Message);
            }
        }

        public async Task<IResult> DeleteRangeAsync(int[] ids)
        {
            try
            {
                List<Image> images = new List<Image>();
                foreach (int id in ids)
                {
                    var entityRes = await GetByIdAsync(id);
                    if (!entityRes.Success)
                    {
                        return new ErrorResult(entityRes.Message);
                    }
                    images.Add(entityRes.Data);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", entityRes.Data.FilePath.TrimStart('/'));
                    if (File.Exists(filePath))
                        File.Delete(filePath);
                }
                await _unitOfWork.Images.DeleteRangeAsync(images);
                return new SuccessResult($"{ids.Length} adet resim silindi");
            }
            catch (Exception ex)
            {
                return new ErrorResult("Resimler silinirken hata oluştu : " + ex.Message);
            }
        }

        public async Task<IDataResult<List<Image>>> GetAllAsync(ImageType? type = null, int? relationshipId = 0)
        {
            try
            {
                if (type is not null && relationshipId > 0)
                {
                    var res = await _unitOfWork.Images.GetAllAsync(p => p.ImageType == type && p.RelationshipId == relationshipId);
                    if (res.Any())
                    {
                        return new SuccessDataResult<List<Image>>(res);
                    }
                    return new ErrorDataResult<List<Image>>("Resim bulunamadı");
                }
                else
                {
                    var res = await _unitOfWork.Images.GetAllAsync();
                    if (res.Any())
                    {
                        return new SuccessDataResult<List<Image>>(res);
                    }
                    return new ErrorDataResult<List<Image>>("Resim bulunamadı");
                }
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<List<Image>>("Resimler getirilirken hata oluştu : " + ex.Message);
            }
        }

        public async Task<IDataResult<Image>> GetByIdAsync(int id)
        {
            try
            {
                var res = await _unitOfWork.Images.GetAsync(p => p.Id == id);
                if (res is not null)
                {
                    return new SuccessDataResult<Image>(res);
                }
                return new ErrorDataResult<Image>("Resim bulunamadı");
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<Image>("Resim getirilirken hata oluştu : " + ex.Message);
            }
        }

        public async Task<IDataResult<List<Image>>> UploadAsync(ImageUploadDto imageUploadDto)
        {
            try
            {
                List<Image> addedImages = new List<Image>();
                foreach (var file in imageUploadDto.Files)
                {
                    if (file is null || file.Length == 0)
                        return new ErrorDataResult<List<Image>>("Dosya boş");

                    var allowedTypes = new[] { "image/jpeg", "image/png", "image/jpg" };
                    if (!allowedTypes.Contains(file.ContentType))
                        return new ErrorDataResult<List<Image>>("Desteklenmeyen dosya formatı.");

                    if (!Directory.Exists(_uploadPath))
                        Directory.CreateDirectory(_uploadPath);

                    var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                    var filePath = Path.Combine(_uploadPath, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                    var image = new Image
                    {
                        CreatedDate = DateTime.Now,
                        ImageType = imageUploadDto.ImageType,
                        RelationshipId = imageUploadDto.RelationshipId,
                        FileName = file.FileName,
                        FilePath = $"/images/{fileName}",
                        ContentType = file.ContentType,
                    };
                    addedImages.Add(image);
                }

                await _unitOfWork.Images.AddRangeAsync(addedImages);
                return new SuccessDataResult<List<Image>>(addedImages, $"{addedImages.Count} adet resim eklendi");
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<List<Image>>("Resim eklenirken hata oluştu");
            }
        }
    }
}
