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
        IResult DepositGiven(DepositAddDto depositAddDto);
        IResult DepositReceived(DepositUpdateDto depositUpdateDto);
        IResult Delete(int id);
        IResult ExtendDueDate(DepositExtentDueDateDto depositExtentDueDateDto);

        IDataResult<List<DepositGetDTO>> GetAllDTO();
        IDataResult<DepositGetDTO> GetByIdDTO(int id);
        IDataResult<Deposit> GetById(int id);
    }
}
