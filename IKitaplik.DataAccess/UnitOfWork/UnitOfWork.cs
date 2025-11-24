using Core.Contexts;
using IKitaplik.DataAccess.Abstract;
using IKitaplik.DataAccess.Concrete.EntityFramework;
using Microsoft.EntityFrameworkCore.Storage;

namespace IKitaplik.DataAccess.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private IDbContextTransaction _transaction;
        private readonly Context _context;
        private readonly IUserContext _userContext;
        public IBookRepository Books { get; private set; }
        public ICategoryRepository Categorys { get; private set; }
        public IDepositRepository Deposits { get; private set; }
        public IDonationRepository Donations { get; private set; }
        public IMovementRepository Movements { get; private set; }
        public IStudentRepository Students { get; private set; }
        public IWriterRepository Writer { get; private set; }
        public IUserRepository Users { get; private set; }
        public IImageRepository Images { get; private set; }
        public UnitOfWork(Context context,IUserContext userContext)
        {
            _context = context;
            _userContext = userContext;
            Books = new EfBookRepository(_context,userContext);
            Categorys = new EfCategoryRepository(_context,userContext);
            Deposits = new EfDepositRepository(_context, userContext);
            Donations = new EfDonationRepository(_context, userContext);
            Movements = new EfMovomentRepository(_context, userContext);
            Students = new EfStudentRepository(_context, userContext);
            Writer = new EfWriterRepository(_context, userContext);
            Users = new EfUserRepository(_context, userContext);
            Images = new EfImageRepository(_context, userContext);
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