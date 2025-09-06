using Core.Utilities.Results;
using IKitaplik.Entities.Concrete;
using IKitaplik.Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKitaplik.Business.Abstract
{
    public interface IMovementService
    {
        Task<IResult> AddAsync(Movement movement);

        Task<IDataResult<List<MovementGetDTO>>> GetAllAsync();
        Task<IDataResult<List<MovementGetDTO>>> GetAllFilteredStudentNameAsync(string fullName);
        Task<IDataResult<List<MovementGetDTO>>> GetAllFilteredStudentIdAsync(int id);
        Task<IDataResult<List<MovementGetDTO>>> GetAllFilteredBookIdAsync(int id);
        Task<IDataResult<List<MovementGetDTO>>> GetAllFilteredBookNameAsync(string name);
        Task<IDataResult<List<MovementGetDTO>>> GetAllFilteredDepositIdAsync(int id);
        Task<IDataResult<List<MovementGetDTO>>> GetAllFilteredDonationIdAsync(int id);
        Task<IDataResult<MovementGetDTO>> GetByIdDtoAsync(int id);
        Task<IDataResult<Movement>> GetByIdAsync(int id);
    }
}
