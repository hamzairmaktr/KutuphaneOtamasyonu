using Core.Utilities.Results;
using FluentValidation;
using IKitaplik.Business.Abstract;
using IKitaplik.DataAccess.Abstract;
using IKitaplık.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKitaplik.Business.Concrete
{
    public class CategoryManager : ICategoryService
    {
        ICategoryRepository _repostiory;
        IValidator<Category> _validator;

        public CategoryManager(ICategoryRepository repostiory, IValidator<Category> validator)
        {
            _repostiory = repostiory;
            _validator = validator;
        }

        public IResult Add(Category category)
        {
            try
            {
                var validator = _validator.Validate(category);
                if (!validator.IsValid)
                {
                    return new ErrorResult(validator.Errors.First().ErrorMessage);
                }
                _repostiory.Add(category);
                return new SuccessResult("Kategori başarı ile eklendi");
            }
            catch (Exception ex)
            {
                return new ErrorResult("Kategori eklenirken hata oluştu : " + ex.Message);
            }
        }

        public IResult Delete(Category category)
        {
            try
            {
                _repostiory.Delete(category);
                return new SuccessResult("Kategori silindi");
            }
            catch (Exception ex)
            {
                return new ErrorResult("Kategori silinirken hata oluştu : " + ex.Message);
            }
        }

        public IDataResult<List<Category>> GetAll()
        {
            try
            {
                var list = _repostiory.GetAll();
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
                var result = _repostiory.Get(p => p.Id == id);
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

        public IResult Update(Category category)
        {
            try
            {
                var validator = _validator.Validate(category);
                if (!validator.IsValid)
                {
                    return new ErrorResult(validator.Errors.First().ErrorMessage);
                }

                _repostiory.Update(category);
                return new SuccessResult("Kategori güncellendi");
            }
            catch (Exception ex)
            {
                return new ErrorResult("Kategori güncellenirken hata oluştu : " + ex.Message);
            }
        }
    }
}
