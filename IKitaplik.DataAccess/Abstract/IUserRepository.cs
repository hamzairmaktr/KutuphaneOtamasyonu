using Core.DataAccess;
using IKitaplik.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKitaplik.DataAccess.Abstract
{
    public interface IUserRepository:IEntityRepository<User>
    {
    }
}
