using Core.DataAccess;
using IKitaplik.Entities.Concrete;
using IKitaplik.Entities.DTOs;
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
        List<DepositGetDTO> GetAllDepositDTOs(Expression<Func<DepositGetDTO, bool>> filter = null);
        DepositGetDTO GetDepositFilteredDTOs(Expression<Func<DepositGetDTO, bool>> filter);
    }
}
