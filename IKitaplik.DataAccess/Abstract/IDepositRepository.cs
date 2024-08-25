using Core.DataAccess;
using IKitaplık.Entities.Concrete;
using IKitaplık.Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace IKitaplik.DataAccess.Abstract
{
    public interface IDepositRepository:IEntityRepository<Deposit>
    {
        List<DepositGetDTO> GetAllDepositDTOs();
        List<DepositGetDTO> GetAllDepositFilteredDTOs(Expression<Func<DepositGetDTO, bool>> filter);
    }
}
