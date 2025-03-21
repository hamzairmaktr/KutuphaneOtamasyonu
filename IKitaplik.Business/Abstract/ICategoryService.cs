using Core.Utilities.Results;
using IKitaplik.Entities.Concrete;
using IKitaplik.Entities.DTOs.CategoryDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKitaplik.Business.Abstract
{
    public interface ICategoryService
    {
        IResult Add(CategoryAddDto categoryAddDto);
        IResult Update(CategoryUpdateDto categoryUpdateDto);
        IResult Delete(int id);

        IDataResult<List<Category>> GetAll();
        IDataResult<Category> GetById(int id);
    }
}
