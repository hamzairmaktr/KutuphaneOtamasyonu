using AutoMapper;
using IKitaplik.Entities.Concrete;
using IKitaplik.Entities.DTOs.BookDTOs;
using IKitaplik.Entities.DTOs.CategoryDTOs;
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

            CreateMap<CategoryAddDto, Category>();
            CreateMap<CategoryUpdateDto, Category>();
            CreateMap<CategoryGetDto, Category>();
        }
    }
}
