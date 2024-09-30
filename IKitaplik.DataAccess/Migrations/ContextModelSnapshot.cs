﻿// <auto-generated />
using System;
using IKitaplik.DataAccess.Concrete.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
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
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.32")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("IKitaplık.Entities.Concrete.Book", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Barcode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Piece")
                        .HasColumnType("int");

                    b.Property<string>("ShelfNo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Situation")
                        .HasColumnType("bit");

                    b.Property<string>("Writer")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("Books");
                });

            modelBuilder.Entity("IKitaplık.Entities.Concrete.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("IKitaplık.Entities.Concrete.Deposit", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<bool>("AmILate")
                        .HasColumnType("bit");

                    b.Property<int>("BookId")
                        .HasColumnType("int");

                    b.Property<DateTime>("DeliveryDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsItDamaged")
                        .HasColumnType("bit");

                    b.Property<DateTime>("IssueDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Note")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("StudentId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("BookId");

                    b.HasIndex("StudentId");

                    b.ToTable("Deposits");
                });

            modelBuilder.Entity("IKitaplık.Entities.Concrete.Donation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("BookId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<bool?>("IsItDamaged")
                        .HasColumnType("bit");

                    b.Property<int>("StudentId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("BookId")
                        .IsUnique();

                    b.HasIndex("StudentId");

                    b.ToTable("Donations");
                });

            modelBuilder.Entity("IKitaplık.Entities.Concrete.Movement", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int?>("BookId")
                        .HasColumnType("int");

                    b.Property<int?>("DepositId")
                        .HasColumnType("int");

                    b.Property<int?>("DonationId")
                        .HasColumnType("int");

                    b.Property<DateTime>("MovementDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Note")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("StudentId")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("BookId");

                    b.HasIndex("DepositId");

                    b.HasIndex("DonationId");

                    b.HasIndex("StudentId");

                    b.ToTable("Movements");
                });

            modelBuilder.Entity("IKitaplık.Entities.Concrete.Student", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Class")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EMail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("NumberofBooksRead")
                        .HasColumnType("int");

                    b.Property<int>("Point")
                        .HasColumnType("int");

                    b.Property<int>("SchoolName")
                        .HasColumnType("int");

                    b.Property<bool>("Situation")
                        .HasColumnType("bit");

                    b.Property<string>("TelephoneNumber")
                        .HasColumnType("nvarchar(max)");

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
                        .WithMany("Deposits")
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("IKitaplık.Entities.Concrete.Student", "Student")
                        .WithMany("Deposits")
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Book");

                    b.Navigation("Student");
                });

            modelBuilder.Entity("IKitaplık.Entities.Concrete.Donation", b =>
                {
                    b.HasOne("IKitaplık.Entities.Concrete.Book", "Book")
                        .WithOne("Donation")
                        .HasForeignKey("IKitaplık.Entities.Concrete.Donation", "BookId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("IKitaplık.Entities.Concrete.Student", "Student")
                        .WithMany("Donations")
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Book");

                    b.Navigation("Student");
                });

            modelBuilder.Entity("IKitaplık.Entities.Concrete.Movement", b =>
                {
                    b.HasOne("IKitaplık.Entities.Concrete.Book", "Book")
                        .WithMany()
                        .HasForeignKey("BookId");

                    b.HasOne("IKitaplık.Entities.Concrete.Deposit", "Deposit")
                        .WithMany()
                        .HasForeignKey("DepositId");

                    b.HasOne("IKitaplık.Entities.Concrete.Donation", "Donation")
                        .WithMany()
                        .HasForeignKey("DonationId");

                    b.HasOne("IKitaplık.Entities.Concrete.Student", "Student")
                        .WithMany()
                        .HasForeignKey("StudentId");

                    b.Navigation("Book");

                    b.Navigation("Deposit");

                    b.Navigation("Donation");

                    b.Navigation("Student");
                });

            modelBuilder.Entity("IKitaplık.Entities.Concrete.Book", b =>
                {
                    b.Navigation("Deposits");

                    b.Navigation("Donation");
                });

            modelBuilder.Entity("IKitaplık.Entities.Concrete.Category", b =>
                {
                    b.Navigation("Books");
                });

            modelBuilder.Entity("IKitaplık.Entities.Concrete.Student", b =>
                {
                    b.Navigation("Deposits");

                    b.Navigation("Donations");
                });
#pragma warning restore 612, 618
        }
    }
}
