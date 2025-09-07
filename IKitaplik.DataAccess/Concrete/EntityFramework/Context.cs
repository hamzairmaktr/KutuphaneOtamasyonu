﻿using Microsoft.EntityFrameworkCore;
using IKitaplik.Entities.Concrete;
using Microsoft.Extensions.Configuration;

namespace IKitaplik.DataAccess.Concrete.EntityFramework
{
    public class Context:DbContext
    {
        
        private readonly IConfiguration _configuration;
        public Context(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer(_configuration.GetConnectionString("conStringLocal"));
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
                .OnDelete(DeleteBehavior.Cascade); // Cascade silme işlemi

            modelBuilder.Entity<Deposit>()
                .HasOne(d => d.Book)
                .WithMany(b => b.Deposits)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Deposit>()
                .HasOne(d => d.Student)
                .WithMany(b => b.Deposits)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Writer>()
                .HasMany(b=>b.Books)
                .WithOne(d=>d.Writer)
                .OnDelete(DeleteBehavior.NoAction);

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Book> Books {  get; set; }
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
