using Core.Utilities.Results;
using FluentValidation;
using IKitaplik.Business.Abstract;
using IKitaplik.Entities.Concrete;
using IKitaplik.DataAccess.UnitOfWork;
using IKitaplik.Entities.DTOs.CategoryDTOs;
using AutoMapper;
using System.Threading.Tasks;
using IKitaplik.Entities.DTOs;

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

        public async Task<IResult> AddAsync(CategoryAddDto categoryAddDto)
        {
            return await HandleWithTransactionHelper.Handling(async () => {
                var category = _mapper.Map<Category>(categoryAddDto);
                var validator = _validator.Validate(category);
                if (!validator.IsValid)
                {
                    return new ErrorResult(validator.Errors.First().ErrorMessage);
                }
                category.CreatedDate = DateTime.Now;
                await _unitOfWork.Categorys.AddAsync(category);
                return new SuccessResult("Kategori başarı ile eklendi");
            }, _unitOfWork);
           

        }

        public async Task<IResult> DeleteAsync(int id)
        {
            return await HandleWithTransactionHelper.Handling(async () =>
            {
                var category = await GetByIdAsync(id);
                if (!category.Success)
                {
                    return new ErrorResult(category.Message);
                }
                await _unitOfWork.Categorys.DeleteAsync(category.Data);
                return new SuccessResult("Kategori silindi");
            }, _unitOfWork);
        }

        public async Task<IDataResult<PagedResult<CategoryGetDto>>> GetAllAsync(PageRequestDto requestDto)
        {
            try
            {
                var list = await _unitOfWork.Categorys.GetAllAsyncPageResult(requestDto.Page,requestDto.PageSize);
                var dtoList = _mapper.Map<PagedResult<CategoryGetDto>>(list);
                return new SuccessDataResult<PagedResult<CategoryGetDto>>(dtoList, "Kategoriler çekildi");
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<PagedResult<CategoryGetDto>>("Kategoriler çekilirken hata oluştu" + ex.Message);
            }
        }

        public async Task<IDataResult<Category>> GetByIdAsync(int id)
        {
            try
            {
                var result = await _unitOfWork.Categorys.GetAsync(p => p.Id == id);
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

        public async Task<IResult> UpdateAsync(CategoryUpdateDto categoryUpdateDto)
        {
            return await HandleWithTransactionHelper.Handling(async () =>
            { 
                var existingCategory = await GetByIdAsync(categoryUpdateDto.Id);
                if (!existingCategory.Success)
                {
                    return new ErrorResult(existingCategory.Message);
                }
                var category = _mapper.Map(categoryUpdateDto,existingCategory.Data);
                var validator = _validator.Validate(category);
                if (!validator.IsValid)
                {
                    return new ErrorResult(validator.Errors.Select(e => e.ErrorMessage)
                        .First());
                }
                await _unitOfWork.Categorys.UpdateAsync(category);
                return new SuccessResult("Kategori güncellendi");
            }, _unitOfWork); 
        }
    }
}
