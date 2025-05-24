using AutoMapper;
using Core.Utilities.Results;
using FluentValidation;
using IKitaplik.Business.Abstract;
using IKitaplik.DataAccess.UnitOfWork;
using IKitaplik.Entities.Concrete;
using IKitaplik.Entities.DTOs.WriterDTOs;

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
        public IResult Add(WriterAddDto writerAddDto)
        {
            return HandleWithTransactionHelper.Handling(() =>
            {
                var writer = _mapper.Map<Writer>(writerAddDto);
                var validator = _validator.Validate(writer);
                if (!validator.IsValid)
                {
                    return new ErrorResult(validator.Errors.Select(p => p.ErrorMessage).ToList().ToString());
                }
                writer.CreatedDate = DateTime.Now;
                _unitOfWork.Writer.Add(writer);
                return new SuccessResult("Yazar eklendi");
            }, _unitOfWork);
        }

        public IResult Delete(int id)
        {
            return HandleWithTransactionHelper.Handling(() =>
            {
                var writer = GetById(id);
                if (!writer.Success)
                    return new ErrorResult(writer.Message);
                _unitOfWork.Writer.Delete(writer.Data);
                return new SuccessResult("Yazar silindi");
            }, _unitOfWork);
        }

        public IDataResult<Writer> GetById(int id)
        {
            try
            {
                var res = _unitOfWork.Writer.Get(p => p.Id == id);
                if (res == null)
                    return new ErrorDataResult<Writer>("İlgili yazar bulunamadı");
                return new SuccessDataResult<Writer>(res, "İlgili yazar çekildi");
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<Writer>("Yazar çekilirken hata oluştu: " + ex.Message);
            }
        }

        public IDataResult<List<WriterGetDto>> GetAll()
        {
            try
            {
                var res = _unitOfWork.Writer.GetAll();
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

        public IDataResult<List<WriterGetDto>> GetAllFilteredNameContains(string name)
        {
            try
            {
                var res = _unitOfWork.Writer.GetAll(p => p.WriterName.ToLower().Contains(name.ToLower()));
                var dto = _mapper.Map<List<WriterGetDto>>(res);
                return new SuccessDataResult<List<WriterGetDto>>(dto, "Veriler çekildi");
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<List<WriterGetDto>>("Veriler çekilirken hata oluştu: " + ex.Message);
            }
        }

        public IResult Update(WriterUpdateDto writerUpdateDto)
        {
            return HandleWithTransactionHelper.Handling(() =>
            {
                var existingWriter = GetById(writerUpdateDto.Id);
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
                _unitOfWork.Writer.Update(writer);
                return new SuccessResult("Yazar Güncellendi");
            }, _unitOfWork);
        }
    }
}
