using Core.Contexts;
using Core.Entities;
using IKitaplik.Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Linq.Expressions;

namespace IKitaplik.DataAccess.Concrete.EntityFramework
{
    public class Context : DbContext
    {

        private readonly IConfiguration _configuration;
        private readonly int? _currentUserId;

        public Context(IConfiguration configuration, IUserContext userContext)
        {
            _configuration = configuration;
            if (int.TryParse(userContext.UserId, out int userId))
            {
                _currentUserId = userId;
            }
        }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer(_configuration.GetConnectionString("conStringGlobal"));
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>()
                .HasOne(c => c.Category)
                .WithMany(b => b.Books)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Movement>()
                .HasOne(m => m.Book)
                .WithMany(b => b.Movements)
                .HasForeignKey(m => m.BookId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Deposit>()
                .HasOne(d => d.Book)
                .WithMany(b => b.Deposits)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Deposit>()
                .HasOne(d => d.Student)
                .WithMany(b => b.Deposits)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Writer>()
                .HasMany(b => b.Books)
                .WithOne(d => d.Writer)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Books)
                .WithOne(b => b.User)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Categories)
                .WithOne(c => c.User)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Writers)
                .WithOne(w => w.User)
                .HasForeignKey(w => w.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Deposits)
                .WithOne(d => d.User)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Donations)
                .WithOne(d => d.User)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Students)
                .WithOne(s => s.User)
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Movements)
                .WithOne(m => m.User)
                .HasForeignKey(m => m.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Category>().HasQueryFilter(c => c.UserId == _currentUserId && c.IsDeleted == false);
            modelBuilder.Entity<Book>().HasQueryFilter(c => c.UserId == _currentUserId && c.IsDeleted == false);
            modelBuilder.Entity<Deposit>().HasQueryFilter(c => c.UserId == _currentUserId && c.IsDeleted == false);
            modelBuilder.Entity<Writer>().HasQueryFilter(c => c.UserId == _currentUserId && c.IsDeleted == false);
            modelBuilder.Entity<Student>().HasQueryFilter(c => c.UserId == _currentUserId && c.IsDeleted == false);
            modelBuilder.Entity<Image>().HasQueryFilter(c => c.UserId == _currentUserId && c.IsDeleted == false);
            modelBuilder.Entity<Movement>().HasQueryFilter(c => c.UserId == _currentUserId && c.IsDeleted == false);
            modelBuilder.Entity<Donation>().HasQueryFilter(c => c.UserId == _currentUserId && c.IsDeleted == false);
            base.OnModelCreating(modelBuilder);
        }


        public DbSet<Book> Books { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Deposit> Deposits { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Donation> Donations { get; set; }
        public DbSet<Movement> Movements { get; set; }
        public DbSet<Writer> Writers { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Image> Images { get; set; }
    }
}
