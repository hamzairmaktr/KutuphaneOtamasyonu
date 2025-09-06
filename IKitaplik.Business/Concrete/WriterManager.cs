using AutoMapper;
using Core.Utilities.Results;
using FluentValidation;
using IKitaplik.Business.Abstract;
using IKitaplik.DataAccess.UnitOfWork;
using IKitaplik.Entities.Concrete;
using IKitaplik.Entities.DTOs.WriterDTOs;
using System.Threading.Tasks;

namespace IKitaplik.Business.Concrete
{
    public class WriterManager : IWriterService
    {
        private IUnitOfWork _unitOfWork;
        private readonly IValidator<Writer> _validator;
        IMapper _mapper;
        public WriterManager(IUnitOfWork unitOfWork, IValidator<Writer> validator, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _validator = validator;
        }
        public async Task<IResult> AddAsync(WriterAddDto writerAddDto)
        {
            return await HandleWithTransactionHelper.Handling(async () =>
            {
                var writer = _mapper.Map<Writer>(writerAddDto);
                var validator = _validator.Validate(writer);
                if (!validator.IsValid)
                {
                    return new ErrorResult(validator.Errors.Select(p => p.ErrorMessage).ToList().ToString());
                }
                writer.CreatedDate = DateTime.Now;
                await _unitOfWork.Writer.AddAsync(writer);
                return new SuccessResult("Yazar eklendi");
            }, _unitOfWork);
        }

        public async Task<IResult> DeleteAsync(int id)
        {
            return await HandleWithTransactionHelper.Handling(async () =>
            {
                var writer = await GetByIdAsync(id);
                if (!writer.Success)
                    return new ErrorResult(writer.Message);
                await _unitOfWork.Writer.DeleteAsync(writer.Data);
                return new SuccessResult("Yazar silindi");
            }, _unitOfWork);
        }

        public async Task<IDataResult<Writer>> GetByIdAsync(int id)
        {
            try
            {
                var res = await _unitOfWork.Writer.GetAsync(p => p.Id == id);
                if (res == null)
                    return new ErrorDataResult<Writer>("İlgili yazar bulunamadı");
                return new SuccessDataResult<Writer>(res, "İlgili yazar çekildi");
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<Writer>("Yazar çekilirken hata oluştu: " + ex.Message);
            }
        }

        public async Task<IDataResult<List<WriterGetDto>>> GetAllAsync()
        {
            try
            {
                var res = await _unitOfWork.Writer.GetAllAsync();
                if (res.Count <= 0)
                    return new ErrorDataResult<List<WriterGetDto>>("Veri bulunamadı");
                var dto = _mapper.Map<List<WriterGetDto>>(res);
                return new SuccessDataResult<List<WriterGetDto>>(dto, "Veriler çekildi");
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<List<WriterGetDto>>("Yazar çekilirken hata oluştu: " + ex.Message);
            }
        }

        public async Task<IDataResult<List<WriterGetDto>>> GetAllFilteredNameContainsAsync(string name)
        {
            try
            {
                var res = await _unitOfWork.Writer.GetAllAsync(p => p.WriterName.ToLower().Contains(name.ToLower()));
                var dto = _mapper.Map<List<WriterGetDto>>(res);
                return new SuccessDataResult<List<WriterGetDto>>(dto, "Veriler çekildi");
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<List<WriterGetDto>>("Veriler çekilirken hata oluştu: " + ex.Message);
            }
        }

        public async Task<IResult> UpdateAsync(WriterUpdateDto writerUpdateDto)
        {
            return await HandleWithTransactionHelper.Handling(async () =>
            {
                var existingWriter = await GetByIdAsync(writerUpdateDto.Id);
                if (!existingWriter.Success)
                {
                    return new ErrorResult(existingWriter.Message);
                }

                var writer = _mapper.Map<Writer>(writerUpdateDto);
                var validator = _validator.Validate(writer);
                if (!validator.IsValid)
                {
                    return new ErrorResult(validator.Errors.Select(p => p.ErrorMessage).ToList().ToString());
                }
                writer.CreatedDate = existingWriter.Data.CreatedDate;
                writer.UpdatedDate = DateTime.Now;
                await _unitOfWork.Writer.UpdateAsync(writer);
                return new SuccessResult("Yazar Güncellendi");
            }, _unitOfWork);
        }
    }
}
