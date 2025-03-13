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
    public interface IDepositService
    {
        IResult DepositGiven(Deposit deposit, int bookId, int studentId);
        IResult DepositReceived(Deposit deposit);
        IResult Delete(Deposit deposit);
        IResult ExtendDueDate(int depositId, int additionalDays, DateTime date, bool asDate);

        IDataResult<List<DepositGetDTO>> GetAllDTO();
        IDataResult<DepositGetDTO> GetByIdDTO(int id);
        IDataResult<Deposit> GetById(int id);
    }
}
