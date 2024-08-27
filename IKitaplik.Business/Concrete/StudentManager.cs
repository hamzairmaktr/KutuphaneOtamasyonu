using Core.Utilities.Results;
using FluentValidation;
using IKitaplik.Business.Abstract;
using IKitaplik.DataAccess.Abstract;
using IKitaplık.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IKitaplik.Business.Concrete
{
    public class StudentManager : IStudentService
    {
        IStudentRepository _studentRepository;
        IValidator<Student> _validator;

        public StudentManager(IStudentRepository studentRepository, IValidator<Student> validator)
        {
            _validator = validator;
            _studentRepository = studentRepository;
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
                _studentRepository.Add(student);
                return new SuccessResult("Öğrenci başarı ile eklendi");
            }
            catch (Exception ex)
            {
                return new ErrorResult("Öğrenci eklenirken hata oluştu: " + ex.Message);
            }
        }

        public IResult Delete(Student student)
        {
            try
            {
                _studentRepository.Delete(student);
                return new SuccessResult("Öğrenci başarı ile silindi");
            }
            catch (Exception ex)
            {
                return new ErrorResult("Öğrenci silinirken hata oluştu: " + ex.Message);
            }
        }

        public IDataResult<List<Student>> GetAll()
        {
            try
            {
                List<Student> students = _studentRepository.GetAll().ToList();
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
                List<Student> students = _studentRepository.GetAll(p => p.Name == name).ToList();
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
                Student student = _studentRepository.Get(p => p.Id == id);
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
                _studentRepository.Update(student);
                return new SuccessResult("Öğrenci başarı ile güncellendi");
            }
            catch (Exception ex)
            {
                return new ErrorResult("Öğrenci güncellenirken hata oluştu: " + ex.Message);
            }
        }
    }
}
