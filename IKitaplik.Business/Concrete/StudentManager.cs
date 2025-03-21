using Core.Utilities.Results;
using FluentValidation;
using IKitaplik.Business.Abstract;
using IKitaplik.Entities.Concrete;
using IKitaplik.DataAccess.UnitOfWork;
using AutoMapper;

namespace IKitaplik.Business.Concrete
{
    public class StudentManager : IStudentService
    {
        private readonly IValidator<Student> _validator;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMovementService _movementService;
        private readonly IMapper _mapper;

        public StudentManager(IValidator<Student> validator,
                              IUnitOfWork unitOfWork,
                              IMovementService movementService,
                              IMapper mapper)
        {
            _movementService = movementService;
            _unitOfWork = unitOfWork;
            _validator = validator;
            _mapper = mapper;
        }

        public IResult Add(StudentAddDto studentAddDto)
        {
            try
            {
                var student = _mapper.Map<Student>(studentAddDto);
                var isValid = _validator.Validate(student);
                if (!isValid.IsValid)
                {
                    return new ErrorResult(isValid.Errors.First().ErrorMessage);
                }
                _unitOfWork.BeginTransaction();
                student.CreatedDate = DateTime.Now;
                student.Point = 100;
                student.NumberofBooksRead = 0;
                student.Situation = true;
                _unitOfWork.Students.Add(student);

                var movementResponse = _movementService.Add(new Movement{
                    StudentId = student.Id,
                    Title = "Öğrenci Eklendi",
                    MovementDate = DateTime.Now,
                    Note = $"{DateTime.Now:g} tarihinde {student.Name} adlı öğrenci eklendi",
                });

                if (!movementResponse.Success)
                {
                    _unitOfWork.Rollback();
                    return new ErrorResult(movementResponse.Message);
                }

                _unitOfWork.Commit();
                return new SuccessResult("Öğrenci başarı ile eklendi");
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                return new ErrorResult("Öğrenci eklenirken hata oluştu: " + ex.Message);
            }
        }

        public IResult Delete(int id)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                var student = GetById(id);
                if(!student.Success)
                    return new ErrorResult(student.Message);
                _unitOfWork.Students.Delete(student.Data);

                var movementResponse = _movementService.Add(new Movement{
                    StudentId = student.Data.Id,
                    Title = "Öğrenci Silindi",
                    MovementDate = DateTime.Now,
                    Note = $"{DateTime.Now:g} tarihinde {student.Data.Name} adlı öğrenci silindi",
                });

                if (!movementResponse.Success)
                {
                    _unitOfWork.Rollback();
                    return new ErrorResult(movementResponse.Message);
                }

                _unitOfWork.Commit();
                return new SuccessResult("Öğrenci başarı ile silindi");
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                return new ErrorResult("Öğrenci silinirken hata oluştu: " + ex.Message);
            }
        }

        public IDataResult<List<StudentGetDto>> GetAll()
        {
            try
            {
                List<Student> students = _unitOfWork.Students.GetAll().ToList();
                var listDto = _mapper.Map<List<StudentGetDto>>(students);
                return new SuccessDataResult<List<StudentGetDto>>(listDto, "Öğrenciler başarı ile çekildi");
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<List<StudentGetDto>>("Öğrenciler çekilirken bir hata oluştu: " + ex.Message);
            }
        }

        public IDataResult<List<StudentGetDto>> GetAllByName(string name)
        {
            try
            {
                List<Student> students = _unitOfWork.Students.GetAll(p => p.Name.ToUpper().Contains(name.ToUpper())).ToList();
                var listDto = _mapper.Map<List<StudentGetDto>>(students);
                return new SuccessDataResult<List<StudentGetDto>>(listDto, "Öğrenciler başarı ile çekildi");
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<List<StudentGetDto>>("İlgili öğrenciler çekilirken hata oluştu: " + ex.Message);
            }
        }

        public IDataResult<Student> GetById(int id)
        {
            try
            {
                Student student = _unitOfWork.Students.Get(p => p.Id == id);
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

        public IResult Update(StudentUpdateDto studentUpdateDto)
        {
            try
            {
                var studentExisting = GetById(studentUpdateDto.Id);
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
                _unitOfWork.BeginTransaction();
                student.CreatedDate = createdDate;
                student.UpdatedDate = DateTime.Now;
                _unitOfWork.Students.Update(student);

                var movementResponse = _movementService.Add(new Movement{
                    StudentId = student.Id,
                    MovementDate = DateTime.Now,
                    Title = "Öğrenci Güncellendi",
                    Note = $"{DateTime.Now:g} tarihinde {student.Name} adlı öğrenci güncellendi"
                });

                if(!movementResponse.Success){
                    _unitOfWork.Rollback();
                    return new ErrorResult(movementResponse.Message);
                }
                _unitOfWork.Commit();
                return new SuccessResult("Öğrenci başarı ile güncellendi");
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                return new ErrorResult("Öğrenci güncellenirken hata oluştu: " + ex.Message);
            }
        }
    }
}
