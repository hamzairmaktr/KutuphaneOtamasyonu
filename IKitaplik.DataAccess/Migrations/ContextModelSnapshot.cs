﻿// <auto-generated />
using System;
using IKitaplik.DataAccess.Concrete.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace IKitaplik.DataAccess.Migrations
{
    [DbContext(typeof(Context))]
    partial class ContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.8");

            modelBuilder.Entity("IKitaplık.Entities.Concrete.Book", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Barcode")
                        .HasColumnType("TEXT");

                    b.Property<int>("CategoryId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<int>("Piece")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ShelfNo")
                        .HasColumnType("TEXT");

                    b.Property<bool>("Situation")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Writer")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("Books");
                });

            modelBuilder.Entity("IKitaplık.Entities.Concrete.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("IKitaplık.Entities.Concrete.Deposit", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("AmILate")
                        .HasColumnType("INTEGER");

                    b.Property<int>("BookId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("DeliveryDate")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsItDamaged")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("IssueDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Note")
                        .HasColumnType("TEXT");

                    b.Property<int>("StudentId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("BookId")
                        .IsUnique();

                    b.HasIndex("StudentId")
                        .IsUnique();

                    b.ToTable("Deposits");
                });

            modelBuilder.Entity("IKitaplık.Entities.Concrete.Student", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Class")
                        .HasColumnType("TEXT");

                    b.Property<string>("EMail")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<int>("NumberofBooksRead")
                        .HasColumnType("INTEGER");

                    b.Property<int>("SchoolName")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Situation")
                        .HasColumnType("INTEGER");

                    b.Property<string>("TelephoneNumber")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Students");
                });

            modelBuilder.Entity("IKitaplık.Entities.Concrete.Book", b =>
                {
                    b.HasOne("IKitaplık.Entities.Concrete.Category", "Category")
                        .WithMany("Books")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Category");
                });

            modelBuilder.Entity("IKitaplık.Entities.Concrete.Deposit", b =>
                {
                    b.HasOne("IKitaplık.Entities.Concrete.Book", "Book")
                        .WithOne("Deposit")
                        .HasForeignKey("IKitaplık.Entities.Concrete.Deposit", "BookId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("IKitaplık.Entities.Concrete.Student", "Student")
                        .WithOne("Deposit")
                        .HasForeignKey("IKitaplık.Entities.Concrete.Deposit", "StudentId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Book");

                    b.Navigation("Student");
                });

            modelBuilder.Entity("IKitaplık.Entities.Concrete.Book", b =>
                {
                    b.Navigation("Deposit");
                });

            modelBuilder.Entity("IKitaplık.Entities.Concrete.Category", b =>
                {
                    b.Navigation("Books");
                });

            modelBuilder.Entity("IKitaplık.Entities.Concrete.Student", b =>
                {
                    b.Navigation("Deposit");
                });
#pragma warning restore 612, 618
        }
    }
}
