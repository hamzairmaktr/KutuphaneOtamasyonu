using Core.Utilities.Results;
using IKitaplik.Business.Abstract;
using IKitaplik.Entities.Concrete;
using IKitaplik.Entities.DTOs;
using IKitaplik.DataAccess.UnitOfWork;

namespace IKitaplik.Business.Concrete
{
    public class MovementManager : IMovementService
    {
        private readonly IUnitOfWork _unitOfWork;
        public MovementManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IResult Add(Movement movement)
        {
            try
            {
                _unitOfWork.Movements.Add(movement);
                return new SuccessResult();
            }
            catch (Exception ex)
            {
                return new ErrorResult(ex.Message);
            }
        }

        public IDataResult<List<MovementGetDTO>> GetAll()
        {
            var result = _unitOfWork.Movements.GetAllDTO();
            if (result.Count <= 0)
            {
                return new ErrorDataResult<List<MovementGetDTO>>("Veri bulunamadı");
            }
            return new SuccessDataResult<List<MovementGetDTO>>(result);
        }

        public IDataResult<List<MovementGetDTO>> GetAllFilteredBookId(int id)
        {
            var result = _unitOfWork.Movements.GetAllDTO(p => p.BookId == id);
            if (result.Count <= 0)
            {
                return new ErrorDataResult<List<MovementGetDTO>>("Veri bulunamadı");
            }
            return new SuccessDataResult<List<MovementGetDTO>>(result);
        }

        public IDataResult<List<MovementGetDTO>> GetAllFilteredBookName(string name)
        {
            var result = _unitOfWork.Movements.GetAllDTO(p => p.BookName.Contains(name) || p.BookName.Equals(name));
            if (result.Count <= 0)
            {
                return new ErrorDataResult<List<MovementGetDTO>>("Veri bulunamadı");
            }
            return new SuccessDataResult<List<MovementGetDTO>>(result);
        }

        public IDataResult<List<MovementGetDTO>> GetAllFilteredDepositId(int id)
        {
            var result = _unitOfWork.Movements.GetAllDTO(p => p.DepositId == id);
            if (result.Count <= 0)
            {
                return new ErrorDataResult<List<MovementGetDTO>>("Veri bulunamadı");
            }
            return new SuccessDataResult<List<MovementGetDTO>>(result);
        }

        public IDataResult<List<MovementGetDTO>> GetAllFilteredDonationId(int id)
        {
            var res = _unitOfWork.Movements.GetAllDTO(p => p.DonationId == id);
            if (res.Count <= 0)
                return new ErrorDataResult<List<MovementGetDTO>>("Kayıt bulunamadı");
            return new SuccessDataResult<List<MovementGetDTO>>(res,"Kayıt çekildi");
        }

        public IDataResult<List<MovementGetDTO>> GetAllFilteredStudentId(int id)
        {
            var result = _unitOfWork.Movements.GetAllDTO(p => p.StudentId == id);
            if (result.Count <= 0)
            {
                return new ErrorDataResult<List<MovementGetDTO>>("Veri bulunamadı");
            }
            return new SuccessDataResult<List<MovementGetDTO>>(result);
        }

        public IDataResult<List<MovementGetDTO>> GetAllFilteredStudentName(string fullName)
        {
            var result = _unitOfWork.Movements.GetAllDTO(p => p.StudentName.Contains(fullName) || p.StudentName.Equals(fullName));
            if (result.Count <= 0)
            {
                return new ErrorDataResult<List<MovementGetDTO>>("Veri bulunamadı");
            }
            return new SuccessDataResult<List<MovementGetDTO>>(result);
        }

        public IDataResult<Movement> GetById(int id)
        {
            var result = _unitOfWork.Movements.Get(p => p.Id == id);
            if (result == null)
            {
                return new ErrorDataResult<Movement>("Veri bulunamadı");
            }
            return new SuccessDataResult<Movement>(result);
        }

        public IDataResult<MovementGetDTO> GetByIdDto(int id)
        {
            var result = _unitOfWork.Movements.GetDTO(p => p.Id == id);
            if (result == null)
            {
                return new ErrorDataResult<MovementGetDTO>("Veri bulunamadı");
            }
            return new SuccessDataResult<MovementGetDTO>(result);
        }
    }
}
