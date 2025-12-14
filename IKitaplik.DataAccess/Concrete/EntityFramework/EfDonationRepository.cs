using Core.Contexts;
using Core.DataAccess.EntityFramework;
using Core.Utilities.Results;
using IKitaplik.DataAccess.Abstract;
using IKitaplik.Entities.Concrete;
using IKitaplik.Entities.DTOs.DonationDTOs;
using Microsoft.EntityFrameworkCore;
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
        public EfDonationRepository(Context context, IUserContext userContext) : base(context, userContext)
        {
            _context = context;
        }

        public PagedResult<DonationGetDTO> GetAllDTO(int page, int pageSize, Expression<Func<DonationGetDTO, bool>> filter = null)
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
            if (filter != null)
                list = list.Where(filter);
            int totalCount = list.Count();
            list = list.Skip((page - 1) * pageSize).Take(pageSize);
            return new PagedResult<DonationGetDTO>
            {
                Items = list.ToList(),
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };
        }

        public async Task<PagedResult<DonationGetDTO>> GetAllDTOAsync(int page, int pageSize, Expression<Func<DonationGetDTO, bool>> filter = null)
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
           if(filter != null)
                list = list.Where(filter);
            int totalCount = await list.CountAsync();
            list = list.Skip((page - 1) * pageSize).Take(pageSize);
            return new PagedResult<DonationGetDTO>
            {
                Items =  await list.ToListAsync(),
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };
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

        public async Task<DonationGetDTO> GetDTOAsync(Expression<Func<DonationGetDTO, bool>> filter)
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
            return await list.Where(filter).FirstOrDefaultAsync();
        }
    }
}
