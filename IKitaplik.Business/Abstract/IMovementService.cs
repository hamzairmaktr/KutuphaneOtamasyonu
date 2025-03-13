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
    public interface IMovementService
    {
        IResult Add(Movement movement);

        IDataResult<List<MovementGetDTO>> GetAll();
        IDataResult<List<MovementGetDTO>> GetAllFilteredStudentName(string fullName);
        IDataResult<List<MovementGetDTO>> GetAllFilteredStudentId(int id);
        IDataResult<List<MovementGetDTO>> GetAllFilteredBookId(int id);
        IDataResult<List<MovementGetDTO>> GetAllFilteredBookName(string name);
        IDataResult<List<MovementGetDTO>> GetAllFilteredDepositId(int id);
        IDataResult<MovementGetDTO> GetByIdDto(int id);
        IDataResult<Movement> GetById(int id);
    }
}
