using Core.DataAccess;
using Core.Utilities.Results;
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
    public interface IMovementRepository:IEntityRepository<Movement>
    {
        PagedResult<MovementGetDTO> GetAllDTO(int page,int pageSize,Expression<Func<MovementGetDTO, bool>> filter = null);
        MovementGetDTO GetDTO(Expression<Func<MovementGetDTO, bool>> filter);
        Task<PagedResult<MovementGetDTO>> GetAllDTOAsync(int page, int pageSize, Expression<Func<MovementGetDTO, bool>> filter = null);
        Task<MovementGetDTO> GetDTOAsync(Expression<Func<MovementGetDTO, bool>> filter);
    }
}
