﻿using Core.DataAccess.EntityFramework;
using IKitaplik.DataAccess.Abstract;
using IKitaplik.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKitaplik.DataAccess.Concrete.EntityFramework
{
    public class EfUserRepository : EfEntityRepositoryBase<User, Context>, IUserRepository
    {
        public EfUserRepository(Context context) : base(context)
        {
        }
    }
}
