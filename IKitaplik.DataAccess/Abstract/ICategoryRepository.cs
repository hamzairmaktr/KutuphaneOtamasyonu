using Core.DataAccess;
using IKitaplik.Entities.Concrete;
using IKitaplik.Entities.DTOs.CategoryDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace IKitaplik.DataAccess.Abstract
{
    public interface ICategoryRepository:IEntityRepository<Category>
    {
       
    }
}
