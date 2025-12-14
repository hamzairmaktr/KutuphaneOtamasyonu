using Core.Utilities.Results;
using IKitaplik.Entities.Concrete;
using IKitaplik.Entities.DTOs.BookDTOs;
using IKitaplik.Entities.DTOs.DonationDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKitaplik.Business.Abstract
{
    public interface IDonationService
    {
        Task<IResult> AddAsync(DonationAddDto donation);

        Task<IDataResult<PagedResult<DonationGetDTO>>> GetAllDTOAsync(int page,int pageSize);
        Task<IDataResult<DonationGetDTO>> GetByIdDTOAsync(int id);
    }
}
