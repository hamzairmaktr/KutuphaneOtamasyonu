using IKitaplik.DataAccess.Abstract;
using IKitaplik.DataAccess.Concrete.EntityFramework;
using Microsoft.EntityFrameworkCore.Storage;

namespace IKitaplik.DataAccess.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private IDbContextTransaction _transaction;
        private readonly Context _context;
        public IBookRepository Books { get; private set; }
        public ICategoryRepository Categorys { get; private set; }
        public IDepositRepository Deposits { get; private set; }
        public IDonationRepository Donations { get; private set; }
        public IMovementRepository Movements { get; private set; }
        public IStudentRepository Students { get; private set; }
        public IWriterRepository Writer { get; private set; }
        public UnitOfWork(Context context)
        {
            _context = context;
            Books = new EfBookRepository(_context);
            Categorys = new EfCategoryRepository(_context);
            Deposits = new EfDepositRepository(_context);
            Donations = new EfDonationRepository(_context);
            Movements = new EfMovomentRepository(_context);
            Students = new EfStudentRepository(_context);
            Writer = new EfWriterRepository(_context);
        }

        public void BeginTransaction()
        {
            if (_transaction == null)
            {
                _transaction = _context.Database.BeginTransaction();
            }
        }

        public void Commit()
        {
            _transaction.Commit();
            _transaction = null;
        }

        public void Rollback()
        {
            _transaction.Rollback();
            _transaction = null;
        }

        public bool IsTransactionActive()
        {
            return _transaction != null;
        }
    }
}