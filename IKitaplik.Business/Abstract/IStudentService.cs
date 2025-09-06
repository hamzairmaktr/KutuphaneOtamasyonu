﻿using Core.Utilities.Results;
using IKitaplik.Entities.Concrete;
using IKitaplik.Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKitaplik.Business.Abstract
{
    public interface IStudentService
    {
        Task<IResult> AddAsync(StudentAddDto studentAddDto);
        Task<IResult> UpdateAsync(StudentUpdateDto studentUpdateDto, bool isDonationOrDeposit = false);
        Task<IResult> DeleteAsync(int id);

        Task<IDataResult<List<StudentGetDto>>> GetAllAsync();
        Task<IDataResult<List<StudentGetDto>>> GetAllActiveAsync();
        Task<IDataResult<List<StudentGetDto>>> GetAllByNameAsync(string name);
        Task<IDataResult<Student>> GetByIdAsync(int id);
    }
}
