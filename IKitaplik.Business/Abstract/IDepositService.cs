using Core.Utilities.Results;
using IKitaplik.Entities.Concrete;
using IKitaplik.Entities.DTOs.DepositDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKitaplik.Business.Abstract
{
    public interface IDepositService
    {
        Task<IResult> DepositGivenAsync(DepositAddDto depositAddDto);
        Task<IResult> DepositReceivedAsync(DepositUpdateDto depositUpdateDto);
        Task<IResult> DeleteAsync(int id);
        Task<IResult> ExtendDueDateAsync(DepositExtentDueDateDto depositExtentDueDateDto);

        Task<IDataResult<PagedResult<DepositGetDTO>>> GetAllDTOAsync(int page, int pageSize);
        Task<IDataResult<DepositGetDTO>> GetByIdDTOAsync(int id);
        Task<IDataResult<Deposit>> GetByIdAsync(int id);
    }
}
