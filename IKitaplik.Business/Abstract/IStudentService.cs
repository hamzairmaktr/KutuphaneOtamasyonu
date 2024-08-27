using Core.Utilities.Results;
using IKitaplık.Entities.Concrete;
using IKitaplık.Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKitaplik.Business.Abstract
{
    public interface IStudentService
    {
        IResult Add(Student student);
        IResult Update(Student student);
        IResult Delete(Student student);

        IDataResult<List<Student>> GetAll();
        IDataResult<List<Student>> GetAllByName(string name);
        IDataResult<Student> GetById(int id);
    }
}
