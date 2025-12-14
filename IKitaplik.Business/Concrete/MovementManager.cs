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

        public async Task<IDataResult<PagedResult<MovementGetDTO>>> GetAllAsync(int page,int pageSize)
        {
            var result = await _unitOfWork.Movements.GetAllDTOAsync(page,pageSize);
            if (result.TotalCount <= 0)
            {
                return new ErrorDataResult<PagedResult<MovementGetDTO>>("Veri bulunamadı");
            }
            return new SuccessDataResult<PagedResult<MovementGetDTO>>(result);
        }

        public async Task<IDataResult<PagedResult<MovementGetDTO>>> GetAllFilteredBookIdAsync(int id, int page, int pageSize)
        {
            var result = await _unitOfWork.Movements.GetAllDTOAsync(page, pageSize,p => p.BookId == id);
            if (result.TotalCount <= 0)
            {
                return new ErrorDataResult<PagedResult<MovementGetDTO>>("Veri bulunamadı");
            }
            return new SuccessDataResult<PagedResult<MovementGetDTO>>(result);
        }

        public async Task<IDataResult<PagedResult<MovementGetDTO>>> GetAllFilteredBookNameAsync(string name, int page, int pageSize)
        {
            var result = await _unitOfWork.Movements.GetAllDTOAsync(page, pageSize,p => p.BookName.Contains(name) || p.BookName.Equals(name));
            if (result.TotalCount <= 0)
            {
                return new ErrorDataResult<PagedResult<MovementGetDTO>>("Veri bulunamadı");
            }
            return new SuccessDataResult<PagedResult<MovementGetDTO>>(result);
        }

        public async Task<IDataResult<PagedResult<MovementGetDTO>>> GetAllFilteredDepositIdAsync(int id, int page, int pageSize)
        {
            var result = await _unitOfWork.Movements.GetAllDTOAsync(page, pageSize,p => p.DepositId == id);
            if (result.TotalCount <= 0)
            {
                return new ErrorDataResult<PagedResult<MovementGetDTO>>("Veri bulunamadı");
            }
            return new SuccessDataResult<PagedResult<MovementGetDTO>>(result);
        }

        public async Task<IDataResult<PagedResult<MovementGetDTO>>> GetAllFilteredDonationIdAsync(int id, int page, int pageSize)
        {
            var res = await _unitOfWork.Movements.GetAllDTOAsync(page, pageSize,p => p.DonationId == id);
            if (res.TotalCount <= 0)
                return new ErrorDataResult<PagedResult<MovementGetDTO>>("Kayıt bulunamadı");
            return new SuccessDataResult<PagedResult<MovementGetDTO>>(res,"Kayıt çekildi");
        }

        public async Task<IDataResult<PagedResult<MovementGetDTO>>> GetAllFilteredStudentIdAsync(int id, int page, int pageSize)
        {
            var result = await _unitOfWork.Movements.GetAllDTOAsync(page, pageSize,p => p.StudentId == id);
            if (result.TotalCount <= 0)
            {
                return new ErrorDataResult<PagedResult<MovementGetDTO>>("Veri bulunamadı");
            }
            return new SuccessDataResult<PagedResult<MovementGetDTO>>(result);
        }

        public async Task<IDataResult<PagedResult<MovementGetDTO>>> GetAllFilteredStudentNameAsync(string fullName, int page, int pageSize)
        {
            var result = await _unitOfWork.Movements.GetAllDTOAsync(page, pageSize,p => p.StudentName.Contains(fullName) || p.StudentName.Equals(fullName));
            if (result.TotalCount <= 0)
            {
                return new ErrorDataResult<PagedResult<MovementGetDTO>>("Veri bulunamadı");
            }
            return new SuccessDataResult<PagedResult<MovementGetDTO>>(result);
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
