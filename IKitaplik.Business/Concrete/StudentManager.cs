using Core.Utilities.Results;
using FluentValidation;
using IKitaplik.Business.Abstract;
using IKitaplık.Entities.Concrete;
using IKitaplik.DataAccess.UnitOfWork;

namespace IKitaplik.Business.Concrete
{
    public class StudentManager : IStudentService
    {
        private readonly IValidator<Student> _validator;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMovementService _movementService;

        public StudentManager(IValidator<Student> validator,
                              IUnitOfWork unitOfWork,
                              IMovementService movementService)
        {
            _movementService = movementService;
            _unitOfWork = unitOfWork;
            _validator = validator;
        }

        public IResult Add(Student student)
        {
            try
            {
                var isValid = _validator.Validate(student);
                if (!isValid.IsValid)
                {
                    return new ErrorResult(isValid.Errors.First().ErrorMessage);
                }
                _unitOfWork.BeginTransaction();

                student.Point = 100;
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

        public IResult Delete(Student student)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                _unitOfWork.Students.Delete(student);

                var movementResponse = _movementService.Add(new Movement{
                    StudentId = student.Id,
                    Title = "Öğrenci Silindi",
                    MovementDate = DateTime.Now,
                    Note = $"{DateTime.Now:g} tarihinde {student.Name} adlı öğrenci silindi",
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

        public IDataResult<List<Student>> GetAll()
        {
            try
            {
                List<Student> students = _unitOfWork.Students.GetAll().ToList();
                return new SuccessDataResult<List<Student>>(students, "Öğrenciler başarı ile çekildi");
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<List<Student>>("Öğrenciler çekilirken bir hata oluştu: " + ex.Message);
            }
        }

        public IDataResult<List<Student>> GetAllByName(string name)
        {
            try
            {
                List<Student> students = _unitOfWork.Students.GetAll(p => p.Name == name).ToList();
                return new SuccessDataResult<List<Student>>(students, "Öğrenciler başarı ile çekildi");
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<List<Student>>("İlgili öğrenciler çekilirken hata oluştu: " + ex.Message);
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

        public IResult Update(Student student)
        {
            try
            {
                var isValid = _validator.Validate(student);
                if (!isValid.IsValid)
                {
                    return new ErrorResult(isValid.Errors.First().ErrorMessage);
                }
                _unitOfWork.BeginTransaction();
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
