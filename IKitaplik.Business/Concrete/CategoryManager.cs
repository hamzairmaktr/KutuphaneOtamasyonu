﻿using Core.Utilities.Results;
using FluentValidation;
using IKitaplik.Business.Abstract;
using IKitaplik.Entities.Concrete;
using IKitaplik.DataAccess.UnitOfWork;
using IKitaplik.Entities.DTOs.CategoryDTOs;
using AutoMapper;

namespace IKitaplik.Business.Concrete
{
    public class CategoryManager : ICategoryService
    {
        private readonly IValidator<Category> _validator;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public CategoryManager(IUnitOfWork unitOfWork, IValidator<Category> validator, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _validator = validator;
            _mapper = mapper;
        }

        public IResult Add(CategoryAddDto categoryAddDto)
        {
            try
            {
                var category = _mapper.Map<Category>(categoryAddDto);
                var validator = _validator.Validate(category);
                if (!validator.IsValid)
                {
                    return new ErrorResult(validator.Errors.First().ErrorMessage);
                }
                _unitOfWork.BeginTransaction();
                category.CreatedDate = DateTime.Now;
                _unitOfWork.Categorys.Add(category);
                _unitOfWork.Commit();
                return new SuccessResult("Kategori başarı ile eklendi");
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                return new ErrorResult("Kategori eklenirken hata oluştu : " + ex.Message);
            }
        }

        public IResult Delete(int id)
        {
            try
            {
                var category = GetById(id);
                if (!category.Success)
                {
                    return new ErrorResult(category.Message);
                }
                _unitOfWork.BeginTransaction();
                _unitOfWork.Categorys.Delete(category.Data);
                _unitOfWork.Commit();
                return new SuccessResult("Kategori silindi");
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                return new ErrorResult("Kategori silinirken hata oluştu : " + ex.Message);
            }
        }

        public IDataResult<List<Category>> GetAll()
        {
            try
            {
                var list = _unitOfWork.Categorys.GetAll();
                return new SuccessDataResult<List<Category>>(list, "Kategoriler çekildi");
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<List<Category>>("Kategoriler çekilirken hata oluştu" + ex.Message);
            }
        }

        public IDataResult<Category> GetById(int id)
        {
            try
            {
                var result = _unitOfWork.Categorys.Get(p => p.Id == id);
                if (result != null)
                {
                    return new SuccessDataResult<Category>(result, "Kategori başarı ile çekildi");
                }
                else
                {
                    return new ErrorDataResult<Category>("İlgili kategori bulunamadı");
                }
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<Category>("İlgili kategori çekilirken hata oluştu : " + ex.Message);
            }
        }

        public IResult Update(CategoryUpdateDto categoryUpdateDto)
        {
            try
            {
                // Önce mevcut kategoriyi veritabanından alalım
                var existingCategory = GetById(categoryUpdateDto.Id);
                if (!existingCategory.Success)
                {
                    return new ErrorResult(existingCategory.Message);
                }

                // Mevcut kategorinin CreatedDate bilgisini saklayalım
                var createdDate = existingCategory.Data.CreatedDate;

                // DTO'dan gelen bilgileri mevcut kategoriye mapleyelim
                _mapper.Map(categoryUpdateDto, existingCategory);

                // CreatedDate'i koruyalım ve UpdatedDate'i güncelleyelim
                existingCategory.Data.CreatedDate = createdDate;
                existingCategory.Data.UpdatedDate = DateTime.Now;

                var validator = _validator.Validate(existingCategory.Data);
                if (!validator.IsValid)
                {
                    return new ErrorResult(validator.Errors.First().ErrorMessage);
                }

                _unitOfWork.BeginTransaction();
                _unitOfWork.Categorys.Update(existingCategory.Data);
                _unitOfWork.Commit();
                return new SuccessResult("Kategori güncellendi");
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                return new ErrorResult("Kategori güncellenirken hata oluştu : " + ex.Message);
            }
        }
    }
}
