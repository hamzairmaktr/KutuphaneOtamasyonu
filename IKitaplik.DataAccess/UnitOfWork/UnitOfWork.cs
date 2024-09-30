using IKitaplik.DataAccess.Concrete.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly Context _context;

        public UnitOfWork(Context context)
        {
            _context = context;
        }

        public void BeginTransaction()
        {
            _context.Database.BeginTransaction();
        }

        public void Commit()
        {
            _context.Database.CurrentTransaction?.Commit();
        }

        public void Rollback()
        {
            _context.Database.CurrentTransaction?.Rollback();
        }
    }
}