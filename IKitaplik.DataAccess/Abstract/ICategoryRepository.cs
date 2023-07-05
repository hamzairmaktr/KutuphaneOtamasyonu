using Core.DataAccess;
using IKitaplık.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKitaplik.DataAccess.Abstract
{
    public interface ICategoryRepository:IEntityRepository<Category>
    {
    }
}
