using Core.Utilities.Results;
using IKitaplik.Entities.Concrete;
using IKitaplik.Entities.DTOs;
using IKitaplik.Entities.DTOs.BookDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKitaplik.Business.Abstract
{
    public interface IDonationService
    {
        IResult Add(BookAddDto bookAddDto, Donation donation);

        IDataResult<List<DonationGetDTO>> GetAllDTO();
        IDataResult<DonationGetDTO> GetByIdDTO(int id);
    }
}
