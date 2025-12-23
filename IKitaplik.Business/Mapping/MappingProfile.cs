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
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<BookAddDto, Book>()
                .ForMember(dest => dest.UserId, opt => opt.Ignore())  // UserId maplenmesin
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore()); ;
            CreateMap<BookUpdateDto, Book>()
                .ForMember(dest => dest.UserId, opt => opt.Ignore())  // UserId maplenmesin
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore()); ;
            CreateMap<BookGetDTO, Book>();
            CreateMap<Book, BookAddDto>();
            CreateMap<Book, BookUpdateDto>();
            CreateMap<Book, BookGetDTO>();

            CreateMap<CategoryAddDto, Category>()
                .ForMember(dest => dest.UserId, opt => opt.Ignore())  // UserId maplenmesin
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore()); ;
            CreateMap<CategoryUpdateDto, Category>()
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore()); ;
            CreateMap<CategoryGetDto, Category>();
            CreateMap<Category, CategoryGetDto>();
            CreateMap<Category, CategoryUpdateDto>();
            CreateMap<Category, CategoryAddDto>();

            CreateMap<StudentAddDto, Student>()
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore());
            CreateMap<StudentUpdateDto, Student>()
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore());
            CreateMap<StudentGetDto, Student>();
            CreateMap<Student, StudentAddDto>();
            CreateMap<Student, StudentUpdateDto>();
            CreateMap<Student, StudentGetDto>();

            CreateMap<WriterAddDto, Writer>()
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore());
            CreateMap<WriterUpdateDto, Writer>()
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore());
            CreateMap<WriterGetDto, Writer>();
            CreateMap<Writer, WriterAddDto>();
            CreateMap<Writer, WriterUpdateDto>();
            CreateMap<Writer, WriterGetDto>();

            CreateMap<Donation, DonationAddDto>();
            CreateMap<Donation, DonationGetDTO>();
            CreateMap<DonationAddDto, Donation>()
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore());
            CreateMap<DonationGetDTO, Donation>();

            CreateMap<DepositAddDto, Deposit>()
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore());
            CreateMap<DepositUpdateDto, Deposit>()
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore());
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
