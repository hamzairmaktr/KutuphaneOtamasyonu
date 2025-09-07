using IKitaplik.DataAccess.Abstract;

namespace IKitaplik.DataAccess.UnitOfWork
{
    public interface IUnitOfWork
    {
        public IBookRepository Books { get; }
        public ICategoryRepository Categorys { get; }
        public IDepositRepository Deposits { get; }
        public IDonationRepository Donations { get; }
        public IMovementRepository Movements { get; }
        public IStudentRepository Students { get; }
        public IWriterRepository Writer { get; }
        public IUserRepository Users { get; }
        public IImageRepository Images { get; }
        public void BeginTransaction();
        public void Commit();
        public void Rollback();
        public bool IsTransactionActive();
    }
}
