using Core.Utilities.Results;
using IKitaplik.Business.Abstract;
using IKitaplik.Entities.Concrete;
using IKitaplik.Entities.DTOs;
using IKitaplik.DataAccess.UnitOfWork;
using System.Threading.Tasks;

namespace IKitaplik.Business.Concrete
{
    public class MovementManager : IMovementService
    {
        private readonly IUnitOfWork _unitOfWork;
        public MovementManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IResult> AddAsync(Movement movement)
        {
            try
            {
                await _unitOfWork.Movements.AddAsync(movement);
                return new SuccessResult();
            }
            catch (Exception ex)
            {
                return new ErrorResult(ex.Message);
            }
        }

        public async Task<IDataResult<List<MovementGetDTO>>> GetAllAsync()
        {
            var result = await _unitOfWork.Movements.GetAllDTOAsync();
            if (result.Count <= 0)
            {
                return new ErrorDataResult<List<MovementGetDTO>>("Veri bulunamadı");
            }
            return new SuccessDataResult<List<MovementGetDTO>>(result);
        }

        public async Task<IDataResult<List<MovementGetDTO>>> GetAllFilteredBookIdAsync(int id)
        {
            var result = await _unitOfWork.Movements.GetAllDTOAsync(p => p.BookId == id);
            if (result.Count <= 0)
            {
                return new ErrorDataResult<List<MovementGetDTO>>("Veri bulunamadı");
            }
            return new SuccessDataResult<List<MovementGetDTO>>(result);
        }

        public async Task<IDataResult<List<MovementGetDTO>>> GetAllFilteredBookNameAsync(string name)
        {
            var result = await _unitOfWork.Movements.GetAllDTOAsync(p => p.BookName.Contains(name) || p.BookName.Equals(name));
            if (result.Count <= 0)
            {
                return new ErrorDataResult<List<MovementGetDTO>>("Veri bulunamadı");
            }
            return new SuccessDataResult<List<MovementGetDTO>>(result);
        }

        public async Task<IDataResult<List<MovementGetDTO>>> GetAllFilteredDepositIdAsync(int id)
        {
            var result = await _unitOfWork.Movements.GetAllDTOAsync(p => p.DepositId == id);
            if (result.Count <= 0)
            {
                return new ErrorDataResult<List<MovementGetDTO>>("Veri bulunamadı");
            }
            return new SuccessDataResult<List<MovementGetDTO>>(result);
        }

        public async Task<IDataResult<List<MovementGetDTO>>> GetAllFilteredDonationIdAsync(int id)
        {
            var res = await _unitOfWork.Movements.GetAllDTOAsync(p => p.DonationId == id);
            if (res.Count <= 0)
                return new ErrorDataResult<List<MovementGetDTO>>("Kayıt bulunamadı");
            return new SuccessDataResult<List<MovementGetDTO>>(res,"Kayıt çekildi");
        }

        public async Task<IDataResult<List<MovementGetDTO>>> GetAllFilteredStudentIdAsync(int id)
        {
            var result = await _unitOfWork.Movements.GetAllDTOAsync(p => p.StudentId == id);
            if (result.Count <= 0)
            {
                return new ErrorDataResult<List<MovementGetDTO>>("Veri bulunamadı");
            }
            return new SuccessDataResult<List<MovementGetDTO>>(result);
        }

        public async Task<IDataResult<List<MovementGetDTO>>> GetAllFilteredStudentNameAsync(string fullName)
        {
            var result = await _unitOfWork.Movements.GetAllDTOAsync(p => p.StudentName.Contains(fullName) || p.StudentName.Equals(fullName));
            if (result.Count <= 0)
            {
                return new ErrorDataResult<List<MovementGetDTO>>("Veri bulunamadı");
            }
            return new SuccessDataResult<List<MovementGetDTO>>(result);
        }

        public async Task<IDataResult<Movement>> GetByIdAsync(int id)
        {
            var result = await _unitOfWork.Movements.GetAsync(p => p.Id == id);
            if (result == null)
            {
                return new ErrorDataResult<Movement>("Veri bulunamadı");
            }
            return new SuccessDataResult<Movement>(result);
        }

        public async Task<IDataResult<MovementGetDTO>> GetByIdDtoAsync(int id)
        {
            var result = await _unitOfWork.Movements.GetDTOAsync(p => p.Id == id);
            if (result == null)
            {
                return new ErrorDataResult<MovementGetDTO>("Veri bulunamadı");
            }
            return new SuccessDataResult<MovementGetDTO>(result);
        }
    }
}
