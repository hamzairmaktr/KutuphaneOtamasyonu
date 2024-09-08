using Core.Utilities.Results;
using IKitaplık.Entities.Concrete;
using IKitaplık.Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKitaplik.Business.Abstract
{
    public interface IDonationService
    {
        IResult Add(Book book, Donation donation);

        IDataResult<List<DonationGetDTO>> GetAllDTO();
        IDataResult<DonationGetDTO> GetByIdDTO(int id);
    }
}
