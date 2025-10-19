﻿using Core.Utilities.Results;
using FluentValidation;
using IKitaplik.Business.Abstract;
using IKitaplik.Entities.Concrete;
using IKitaplik.DataAccess.UnitOfWork;
using AutoMapper;
using System.Threading.Tasks;

namespace IKitaplik.Business.Concrete
{
    public class StudentManager : IStudentService
    {
        private readonly IValidator<Student> _validator;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMovementService _movementService;
        private readonly IMapper _mapper;
        private readonly IImageService _imageService;
        public StudentManager(IValidator<Student> validator,
                              IUnitOfWork unitOfWork,
                              IMovementService movementService,
                              IMapper mapper,
                              IImageService imageService)
        {
            _movementService = movementService;
            _unitOfWork = unitOfWork;
            _validator = validator;
            _mapper = mapper;
            _imageService = imageService;
        }

        public async Task<IResult> AddAsync(StudentAddDto studentAddDto)
        {
            return await HandleWithTransactionHelper.Handling(async () =>
            {
                var student = _mapper.Map<Student>(studentAddDto);
                var isValid = _validator.Validate(student);
                if (!isValid.IsValid)
                {
                    return new ErrorResult(isValid.Errors.First().ErrorMessage);
                }
                student.CreatedDate = DateTime.Now;
                student.Point = 100;
                student.NumberofBooksRead = 0;
                student.Situation = true;
                await _unitOfWork.Students.AddAsync(student);

                var movementResponse = await _movementService.AddAsync(new Movement
                {
                    StudentId = student.Id,
                    Title = "Öğrenci Eklendi",
                    MovementDate = DateTime.Now,
                    Note = $"{DateTime.Now:g} tarihinde {student.Name} adlı öğrenci eklendi",
                });

                if (!movementResponse.Success)
                {
                    return new ErrorResult(movementResponse.Message);
                }
                return new SuccessResult("Öğrenci başarı ile eklendi");
            }, _unitOfWork);
        }

        public async Task<IResult> DeleteAsync(int id)
        {
            return await HandleWithTransactionHelper.Handling(async () =>
            {
                var student = await GetByIdAsync(id);
                if (!student.Success)
                    return new ErrorResult(student.Message);
                await _unitOfWork.Students.DeleteAsync(student.Data);
                await _imageService.DeleteAllAsync(Entities.Enums.ImageType.Student, id);
                var movementResponse = await _movementService.AddAsync(new Movement
                {
                    StudentId = student.Data.Id,
                    Title = "Öğrenci Silindi",
                    MovementDate = DateTime.Now,
                    Note = $"{DateTime.Now:g} tarihinde {student.Data.Name} adlı öğrenci silindi",
                });

                if (!movementResponse.Success)
                {
                    return new ErrorResult(movementResponse.Message);
                }

                return new SuccessResult("Öğrenci başarı ile silindi");

            }, _unitOfWork);
        }

        public async Task<IDataResult<List<StudentGetDto>>> GetAllAsync()
        {
            try
            {
                List<Student> students = await _unitOfWork.Students.GetAllAsync();
                var listDto = _mapper.Map<List<StudentGetDto>>(students);
                return new SuccessDataResult<List<StudentGetDto>>(listDto, "Öğrenciler başarı ile çekildi");
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<List<StudentGetDto>>("Öğrenciler çekilirken bir hata oluştu: " + ex.Message);
            }
        }

        public async Task<IDataResult<List<StudentGetDto>>> GetAllActiveAsync()
        {
            try
            {
                List<Student> students = await _unitOfWork.Students.GetAllAsync(p => p.Situation);
                var listDto = _mapper.Map<List<StudentGetDto>>(students);
                return new SuccessDataResult<List<StudentGetDto>>(listDto, "Öğrenciler başarı ile çekildi");
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<List<StudentGetDto>>("Öğrenciler çekilirken bir hata oluştu: " + ex.Message);
            }
        }

        public async Task<IDataResult<List<StudentGetDto>>> GetAllByNameAsync(string name)
        {
            try
            {
                List<Student> students = await _unitOfWork.Students.GetAllAsync(p => p.Name.ToUpper().Contains(name.ToUpper()));
                var listDto = _mapper.Map<List<StudentGetDto>>(students);
                return new SuccessDataResult<List<StudentGetDto>>(listDto, "Öğrenciler başarı ile çekildi");
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<List<StudentGetDto>>("İlgili öğrenciler çekilirken hata oluştu: " + ex.Message);
            }
        }

        public async Task<IDataResult<Student>> GetByIdAsync(int id)
        {
            try
            {
                Student student = await _unitOfWork.Students.GetAsync(p => p.Id == id);
                if (student != null)
                {
                    return new SuccessDataResult<Student>(student, "Öğrenci başarı ile çekildi");
                }
                return new ErrorDataResult<Student>("İlgili öğrenci bulunamadı");
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<Student>("İlgili öğrenci çekilirken hata oluştu: " + ex.Message);
            }
        }

        public async Task<IResult> UpdateAsync(StudentUpdateDto studentUpdateDto, bool isDonationOrDeposit = false)
        {
            if (isDonationOrDeposit)
            {
                return await UpdateLocalAsync(studentUpdateDto);
            }
            return await HandleWithTransactionHelper.Handling(async () =>
            {
                return await UpdateLocalAsync(studentUpdateDto);
            }, _unitOfWork);
        }

        private async Task<IResult> UpdateLocalAsync(StudentUpdateDto studentUpdateDto)
        {
            var studentExisting = await GetByIdAsync(studentUpdateDto.Id);
            if (!studentExisting.Success)
            {
                return new ErrorResult(studentExisting.Message);
            }
            DateTime createdDate = studentExisting.Data.CreatedDate;
            var student = _mapper.Map<Student>(studentUpdateDto);
            var isValid = _validator.Validate(student);
            if (!isValid.IsValid)
            {
                return new ErrorResult(isValid.Errors.First().ErrorMessage);
            }
            student.CreatedDate = createdDate;
            student.UpdatedDate = DateTime.Now;
            await _unitOfWork.Students.UpdateAsync(student);

            var movementResponse = await _movementService.AddAsync(new Movement
            {
                StudentId = student.Id,
                MovementDate = DateTime.Now,
                Title = "Öğrenci Güncellendi",
                Note = $"{DateTime.Now:g} tarihinde {student.Name} adlı öğrenci güncellendi"
            });

            if (!movementResponse.Success)
            {
                return new ErrorResult(movementResponse.Message);
            }
            return new SuccessResult("Öğrenci başarı ile güncellendi");
        }
    }
}
