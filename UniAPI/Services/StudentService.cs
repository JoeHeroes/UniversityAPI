using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using UniAPI.Entites;
using UniAPI.Exceptions;
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
    public class StudentService : IStudentService
    {
        private readonly UniversityDbContext _dbContext;
        private readonly IMapper _mapper;

        public StudentService(UniversityDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }


        public StudentDto GetById(int id)
        {
            var stu = _dbContext
                .Students
                .Include(r => r.Address)
                .FirstOrDefault(u => u.Id == id);

            if (stu is null)
            {
                throw new NotFoundException("Student not found");
            }

            var result = _mapper.Map<StudentDto>(stu);

            return result;
        }

        public IEnumerable<StudentDto> GetAll()
        {
            var stu = _dbContext
                .Students
                .Include(r => r.Address)
                .ToList();

            if (stu is null)
            {
                throw new NotFoundException("Student not found");
            }

            var result = _mapper.Map<List<StudentDto>>(stu);

            return result;
        }

        public int Create(CreateStudentDto dto)
        {
            var stu = _mapper.Map<Student>(dto);
            _dbContext.Students.Add(stu);
            _dbContext.SaveChanges();

            return stu.Id;
        }
        public void Delete(int id)
        {
            var stu = _dbContext
                .Students
                .FirstOrDefault(u => u.Id == id);

            if (stu is null)
            {
                throw new NotFoundException("Student not found");
            }

            _dbContext.Students.Remove(stu);
            _dbContext.SaveChanges();
        }

        public void Update(int id, UpdateStudentDto dto)
        {
            var stu = _dbContext
                .Students
                .FirstOrDefault(u => u.Id == id);

            if (stu is null)
            {
                throw new NotFoundException("Student not found");
            }

            stu.Title = dto.Title;
            stu.Specialization = dto.Specialization;
            stu.YearOfStudies = dto.YearOfStudies;


            _dbContext.SaveChanges();

        }

    }
}
