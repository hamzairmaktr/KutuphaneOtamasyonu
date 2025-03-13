using Core.Utilities.Results;
using FluentValidation;
using IKitaplik.Business.Abstract;
using IKitaplik.Entities.Concrete;
using IKitaplik.DataAccess.UnitOfWork;

namespace IKitaplik.Business.Concrete
{
    public class CategoryManager : ICategoryService
    {
        private readonly IValidator<Category> _validator;
        private readonly IUnitOfWork _unitOfWork;

        public CategoryManager(IUnitOfWork unitOfWork, IValidator<Category> validator)
        {
            _unitOfWork = unitOfWork;
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
                _unitOfWork.BeginTransaction();
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

        public IResult Delete(Category category)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                _unitOfWork.Categorys.Delete(category);
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

        public IResult Update(Category category)
        {
            try
            {
                var validator = _validator.Validate(category);
                if (!validator.IsValid)
                {
                    return new ErrorResult(validator.Errors.First().ErrorMessage);
                }
                _unitOfWork.BeginTransaction();
                _unitOfWork.Categorys.Update(category);
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
