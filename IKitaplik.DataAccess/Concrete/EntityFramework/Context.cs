using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using IKitaplık.Entities.Concrete;

namespace IKitaplik.DataAccess.Concrete.EntityFramework
{
    public class Context:DbContext
    {
        
        private readonly string str = ConfigurationManager.ConnectionStrings["conStringLocal"].ConnectionString;
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(str);
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>()
                .HasOne(c => c.Category)
                .WithMany(b => b.Books)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Deposit>()
                .HasOne(d => d.Book)
                .WithOne(b => b.Deposit)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Deposit>()
                .HasOne(d => d.Student)
                .WithOne(b => b.Deposit)
                .OnDelete(DeleteBehavior.NoAction);

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Book> Books {  get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Deposit> Deposits { get; set; }
        public DbSet<Student> Students { get; set; }
    }
}
