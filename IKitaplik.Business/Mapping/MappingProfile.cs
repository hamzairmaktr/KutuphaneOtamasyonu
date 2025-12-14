using AutoMapper;
using Core.Utilities.Results;
using IKitaplik.Entities.Concrete;
using IKitaplik.Entities.DTOs.BookDTOs;
using IKitaplik.Entities.DTOs.CategoryDTOs;
using IKitaplik.Entities.DTOs.DepositDTOs;
using IKitaplik.Entities.DTOs.DonationDTOs;
using IKitaplik.Entities.DTOs.UserDTOs;
using IKitaplik.Entities.DTOs.WriterDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKitaplik.Business.Mapping
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<BookAddDto, Book>();
            CreateMap<BookUpdateDto, Book>();
            CreateMap<BookGetDTO, Book>();
            CreateMap<Book, BookAddDto>();
            CreateMap<Book, BookUpdateDto>();
            CreateMap<Book, BookGetDTO>();

            CreateMap<CategoryAddDto, Category>();
            CreateMap<CategoryUpdateDto, Category>();
            CreateMap<CategoryGetDto, Category>();
            CreateMap<Category, CategoryGetDto>();
            CreateMap<Category, CategoryUpdateDto>();
            CreateMap<Category, CategoryAddDto>();

            CreateMap<StudentAddDto, Student>();
            CreateMap<StudentUpdateDto, Student>();
            CreateMap<StudentGetDto, Student>();
            CreateMap<Student, StudentAddDto>();
            CreateMap<Student, StudentUpdateDto>();
            CreateMap<Student, StudentGetDto>();

            CreateMap<WriterAddDto, Writer>();
            CreateMap<WriterUpdateDto, Writer>();
            CreateMap<WriterGetDto, Writer>();
            CreateMap<Writer,WriterAddDto>();
            CreateMap<Writer,WriterUpdateDto>();
            CreateMap<Writer,WriterGetDto>();

            CreateMap<Donation,DonationAddDto>();
            CreateMap<Donation, DonationGetDTO>();
            CreateMap<DonationAddDto, Donation>();
            CreateMap<DonationGetDTO, Donation>();

            CreateMap<DepositAddDto, Deposit>();
            CreateMap<DepositUpdateDto, Deposit>();
            CreateMap<DepositGetDTO, Deposit>();
            CreateMap<Deposit, DepositAddDto>();
            CreateMap<Deposit, DepositUpdateDto>();
            CreateMap<Deposit, DepositGetDTO>();

            CreateMap<UserRegisterDto, User>();
            CreateMap<UserLoginDto, User>();
            CreateMap<User, UserRegisterDto>();
            CreateMap<User, UserLoginDto>();

            CreateMap(typeof(PagedResult<>), typeof(PagedResult<>));
        }
    }
}
