using Core.DataAccess;
using Core.Utilities.Results;
using IKitaplik.Entities.Concrete;
using IKitaplik.Entities.DTOs.DonationDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace IKitaplik.DataAccess.Abstract
{
    public interface IDonationRepository : IEntityRepository<Donation>
    {
        PagedResult<DonationGetDTO> GetAllDTO(int page, int pageSize, Expression<Func<DonationGetDTO, bool>> filter = null);
        DonationGetDTO GetDTO(Expression<Func<DonationGetDTO, bool>> filter);

        Task<PagedResult<DonationGetDTO>> GetAllDTOAsync(int page, int pageSize, Expression<Func<DonationGetDTO, bool>> filter = null);
        Task<DonationGetDTO> GetDTOAsync(Expression<Func<DonationGetDTO, bool>> filter);
    }
}
