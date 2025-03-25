using Core.DataAccess.EntityFramework;
using IKitaplik.DataAccess.Abstract;
using IKitaplik.Entities.Concrete;
using IKitaplik.Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace IKitaplik.DataAccess.Concrete.EntityFramework
{
    public class EfDonationRepository : EfEntityRepositoryBase<Donation, Context>, IDonationRepository
    {
        Context _context;
        public EfDonationRepository(Context context) : base(context)
        {
            _context = context;
        }

        public List<DonationGetDTO> GetAllDTO(Expression<Func<DonationGetDTO, bool>> filter = null)
        {
            var list = from d in _context.Donations
                       join s in _context.Students
                       on d.StudentId equals s.Id
                       join b in _context.Books
                       on d.BookId equals b.Id
                       where d.IsDeleted == false
                       select new DonationGetDTO
                       {
                           BookBarcode = b.Barcode,
                           BookName = b.Name,
                           Date = d.Date,
                           Id = d.Id,
                           IsItDamaged = d.IsItDamaged,
                           StudentName = s.Name,
                       };
            return filter == null ? list.ToList() : list.Where(filter).ToList();
        }

        public DonationGetDTO GetDTO(Expression<Func<DonationGetDTO, bool>> filter)
        {
            var list = from d in _context.Donations
                       join s in _context.Students
                       on d.StudentId equals s.Id
                       join b in _context.Books
                       on d.BookId equals b.Id
                       where d.IsDeleted == false
                       select new DonationGetDTO
                       {
                           BookBarcode = b.Barcode,
                           BookName = b.Name,
                           Date = d.Date,
                           Id = d.Id,
                           IsItDamaged = d.IsItDamaged,
                           StudentName = s.Name,
                       };
            return list.Where(filter).FirstOrDefault();
        }
    }
}
