using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniAPI.Entites;
using UniAPI.Exceptions;
using UniAPI.Models;

namespace UniAPI.Services
{
    public interface IDepartmentService
    {
        int Create(int id, DepartmentDto dto);
        void Delete(int id, int depId);
        IEnumerable<DepartmentDto> GetAll(int id);
        DepartmentDto GetById(int id, int depId);
    }
    public class DepartmentService : IDepartmentService
    {

        private readonly UniversityDbContext _dbContext;
        private readonly IMapper _mapper;

        public DepartmentService(UniversityDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public University GetUniversity(int id)
        {
            var uni = _dbContext.Universities.FirstOrDefault(u => u.Id == id);
            if (uni is null)
            {
                throw new NotFoundException("Not found University");
            }

            return uni;
        }

        public int Create(int id, DepartmentDto dto)
        {
            var uni = GetUniversity(id);

            var dep = _mapper.Map<Department>(dto);

            dep.UniversityId = id;

            _dbContext.Departments.Add(dep);
            _dbContext.SaveChanges();

            return dep.Id;
        }

        public void Delete(int id, int depId)
        {
            var uni = GetUniversity(id);

            _dbContext.RemoveRange(uni.Departments);
            _dbContext.SaveChanges();
        }

        public IEnumerable<DepartmentDto> GetAll(int id)
        {

            var uni = _dbContext
                .Universities
                .Include(r => r.Departments)
                .FirstOrDefault(u => u.Id == id);
            if (uni is null)
            {
                throw new NotFoundException("Not found University");
            }

            var depDto = _mapper.Map<List<DepartmentDto>>(uni.Departments);

            return depDto;


        }

        public DepartmentDto GetById(int id, int depId)
        {
            var uni = _dbContext
               .Universities
               .Include(r => r.Departments)
               .FirstOrDefault(u => u.Id == id);
            if (uni is null)
            {
                throw new NotFoundException("Not found University");
            }

            var dep = _dbContext.Departments.FirstOrDefault(d => d.Id == depId);
            if (dep is null || dep.UniversityId != id)
            {
                throw new NotFoundException("Not found Department");
            }

            var depDto = _mapper.Map<DepartmentDto>(dep);

            return depDto;
        }

       
    }
}
