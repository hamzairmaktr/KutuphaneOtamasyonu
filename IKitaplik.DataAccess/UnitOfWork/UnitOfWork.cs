using IKitaplik.DataAccess.Abstract;
using IKitaplik.DataAccess.Concrete.EntityFramework;

namespace IKitaplik.DataAccess.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly Context _context;
        public IBookRepository Books { get; private set; }
        public ICategoryRepository Categorys { get; private set; }
        public IDepositRepository Deposits { get; private set; }
        public IDonationRepository Donations { get; private set; }
        public IMovementRepository Movements { get; private set; }
        public IStudentRepository Students { get; private set; }
        public UnitOfWork(Context context)
        {
            _context = context;
            Books = new EfBookRepository(_context);
            Categorys = new EfCategoryRepository(_context);
            Deposits = new EfDepositRepository(_context);
            Donations = new EfDonationRepository(_context);
            Movements = new EfMovomentRepository(_context);
            Students = new EfStudentRepository(_context);
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