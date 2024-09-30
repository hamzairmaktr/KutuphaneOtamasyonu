using Core.Utilities.Results;
using IKitaplik.Business.Abstract;
using IKitaplik.DataAccess.Abstract;
using IKitaplık.Entities.Concrete;
using IKitaplık.Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace IKitaplik.Business.Concrete
{
    public class MovementManager : IMovementService
    {
        IMovementRepository _repo;
        public MovementManager(IMovementRepository repo)
        {
            _repo = repo;
        }

        public IResult Add(Movement movement)
        {
            try
            {
                _repo.Add(movement);
                return new SuccessResult();
            }
            catch (Exception ex)
            {
                return new ErrorResult(ex.Message);
            }
        }

        public IDataResult<List<MovementGetDTO>> GetAll()
        {
            var result = _repo.GetAllDTO();
            if (result.Count <= 0)
            {
                return new ErrorDataResult<List<MovementGetDTO>>("Veri bulunamadı");
            }
            return new SuccessDataResult<List<MovementGetDTO>>(result);
        }

        public IDataResult<List<MovementGetDTO>> GetAllFilteredBookId(int id)
        {
            var result = _repo.GetAllDTO(p => p.BookId == id);
            if (result.Count <= 0)
            {
                return new ErrorDataResult<List<MovementGetDTO>>("Veri bulunamadı");
            }
            return new SuccessDataResult<List<MovementGetDTO>>(result);
        }

        public IDataResult<List<MovementGetDTO>> GetAllFilteredBookName(string name)
        {
            var result = _repo.GetAllDTO(p => p.BookName.Contains(name) || p.BookName.Equals(name));
            if (result.Count <= 0)
            {
                return new ErrorDataResult<List<MovementGetDTO>>("Veri bulunamadı");
            }
            return new SuccessDataResult<List<MovementGetDTO>>(result);
        }

        public IDataResult<List<MovementGetDTO>> GetAllFilteredDepositId(int id)
        {
            var result = _repo.GetAllDTO(p => p.DepositId == id);
            if (result.Count <= 0)
            {
                return new ErrorDataResult<List<MovementGetDTO>>("Veri bulunamadı");
            }
            return new SuccessDataResult<List<MovementGetDTO>>(result);
        }

        public IDataResult<List<MovementGetDTO>> GetAllFilteredStudentId(int id)
        {
            var result = _repo.GetAllDTO(p => p.StudentId == id);
            if (result.Count <= 0)
            {
                return new ErrorDataResult<List<MovementGetDTO>>("Veri bulunamadı");
            }
            return new SuccessDataResult<List<MovementGetDTO>>(result);
        }

        public IDataResult<List<MovementGetDTO>> GetAllFilteredStudentName(string fullName)
        {
            var result = _repo.GetAllDTO(p => p.StudentName.Contains(fullName) || p.StudentName.Equals(fullName));
            if (result.Count <= 0)
            {
                return new ErrorDataResult<List<MovementGetDTO>>("Veri bulunamadı");
            }
            return new SuccessDataResult<List<MovementGetDTO>>(result);
        }

        public IDataResult<Movement> GetById(int id)
        {
            var result = _repo.Get(p => p.Id == id);
            if (result == null)
            {
                return new ErrorDataResult<Movement>("Veri bulunamadı");
            }
            return new SuccessDataResult<Movement>(result);
        }

        public IDataResult<MovementGetDTO> GetByIdDto(int id)
        {
            var result = _repo.GetDTO(p => p.Id == id);
            if (result == null)
            {
                return new ErrorDataResult<MovementGetDTO>("Veri bulunamadı");
            }
            return new SuccessDataResult<MovementGetDTO>(result);
        }
    }
}
