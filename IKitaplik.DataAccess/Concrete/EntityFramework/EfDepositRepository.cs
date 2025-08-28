﻿using Core.DataAccess.EntityFramework;
using IKitaplik.DataAccess.Abstract;
using IKitaplik.Entities.Concrete;
using IKitaplik.Entities.DTOs.DepositDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace IKitaplik.DataAccess.Concrete.EntityFramework
{
    public class EfDepositRepository : EfEntityRepositoryBase<Deposit, Context>, IDepositRepository
    {
        private readonly Context _context;
        public EfDepositRepository(Context context) : base(context)
        {
            this._context = context;
        }

        public List<DepositGetDTO> GetAllDepositDTOs(Expression<Func<DepositGetDTO, bool>> filter = null)
        {
            var result = from d in _context.Deposits
                         join b in _context.Books
                         on d.BookId equals b.Id
                         join s in _context.Students
                         on d.StudentId equals s.Id
                         where d.IsDeleted == false
                         select new DepositGetDTO
                         {
                             AmILate = d.AmILate,
                             BookName = b.Name,
                             DeliveryDate = d.DeliveryDate,
                             Id = d.Id,
                             IsItDamaged = d.IsItDamaged,
                             IssueDate = d.IssueDate,
                             Note = d.Note,
                             StudentName = s.Name,
                             IsDelivered = d.IsDelivered
                         };
            return filter == null 
                ? result.ToList()
                : result.Where(filter).ToList();
        }

        public DepositGetDTO GetDepositFilteredDTOs(Expression<Func<DepositGetDTO, bool>> filter)
        {
            var result = from d in _context.Deposits
                         join b in _context.Books
                         on d.BookId equals b.Id
                         join s in _context.Students
                         on d.StudentId equals s.Id
                         where d.IsDeleted == false
                         select new DepositGetDTO
                         {
                             AmILate = d.AmILate,
                             BookName = b.Name,
                             DeliveryDate = d.DeliveryDate,
                             Id = d.Id,
                             IsItDamaged = d.IsItDamaged,
                             IssueDate = d.IssueDate,
                             Note = d.Note,
                             StudentName = s.Name,
                             IsDelivered = d.IsDelivered
                         };
            return result.Where(filter).FirstOrDefault();
        }
    }
}
