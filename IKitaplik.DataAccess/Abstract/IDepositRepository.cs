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
        List<DepositGetDTO> GetAllDepositDTOs();
        List<DepositGetDTO> GetAllDepositFilteredDTOs(Expression<Func<DepositGetDTO, bool>> filter);
        DepositGetDTO GetDepositFilteredDTOs(Expression<Func<DepositGetDTO, bool>> filter);
    }
}
