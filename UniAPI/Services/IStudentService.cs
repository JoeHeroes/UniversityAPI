using System.Collections.Generic;
using UniAPI.Models;

namespace UniAPI.Services
{
    public interface IStudentService
    {
        int Create(CreateStudentDto dto);
        void Delete(int id);
        IEnumerable<StudentDto> GetAll();
        StudentDto GetById(int id);
        void Update(int id, UpdateStudentDto dto);
    }
}