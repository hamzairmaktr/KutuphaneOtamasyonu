using Core.DataAccess.EntityFramework;
using IKitaplik.DataAccess.Abstract;
using IKitaplık.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKitaplik.DataAccess.Concrete.EntityFramework
{
    public class EfStudentRepository:EfEntityRepositoryBase<Student,Context>,IStudentRepository
    {
        private readonly Context _context;

        public EfStudentRepository(Context context) : base(context)
        {
            this._context = context;
        }
    }
}
