using Azure;
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

        Task<IDataResult<PagedResult<MovementGetDTO>>> GetAllAsync(int page, int pageSize);
        Task<IDataResult<PagedResult<MovementGetDTO>>> GetAllFilteredStudentNameAsync(string fullName, int page, int pageSize);
        Task<IDataResult<PagedResult<MovementGetDTO>>> GetAllFilteredStudentIdAsync(int id, int page, int pageSize);
        Task<IDataResult<PagedResult<MovementGetDTO>>> GetAllFilteredBookIdAsync(int id, int page, int pageSize);
        Task<IDataResult<PagedResult<MovementGetDTO>>> GetAllFilteredBookNameAsync(string name, int page, int pageSize);
        Task<IDataResult<PagedResult<MovementGetDTO>>> GetAllFilteredDepositIdAsync(int id, int page, int pageSize);
        Task<IDataResult<PagedResult<MovementGetDTO>>> GetAllFilteredDonationIdAsync(int id, int page, int pageSize);
        Task<IDataResult<MovementGetDTO>> GetByIdDtoAsync(int id);
        Task<IDataResult<Movement>> GetByIdAsync(int id);
    }
}
