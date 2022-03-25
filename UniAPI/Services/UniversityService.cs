using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using UniAPI.Entites;
using UniAPI.Exceptions;
using UniAPI.Models;

namespace UniAPI.Services
{
    public class UniversityService : IUniversityService
    {
        private readonly UniversityDbContext _dbContext;
        private readonly IMapper _mapper;

        public UniversityService(UniversityDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }


        public UniversityDto GetById(int id)
        {
            var uni = _dbContext
                .Universities
                .Include(r => r.Address)
                .FirstOrDefault(u => u.Id == id);

            if (uni is null)
            {
                throw new NotFoundException("Restaurant not found");
            }

            var result = _mapper.Map<UniversityDto>(uni);

            return result;
        }

        public IEnumerable<UniversityDto> GetAll()
        {
            var uni = _dbContext
                .Universities
                .Include(r => r.Address)
                .ToList();

            if (uni is null)
            {
                throw new NotFoundException("Restaurant not found");
            }

            var result = _mapper.Map<List<UniversityDto>>(uni);

            return result;
        }

        public int Create(CreateUniversityDto dto)
        {
            var uni = _mapper.Map<University>(dto);
            _dbContext.Universities.Add(uni);
            _dbContext.SaveChanges();

            return uni.Id;
        }
        public void Delete(int id)
        {
            var uni = _dbContext
                .Universities
                .FirstOrDefault(u => u.Id == id);

            if (uni is null)
            {
                throw new NotFoundException("Restaurant not found");
            }

            _dbContext.Universities.Remove(uni);
            _dbContext.SaveChanges();
        }

        public void Update(int id, UpdateUniversityDto dto)
        {
            var uni = _dbContext
                .Universities
                .FirstOrDefault(u => u.Id == id);

            if (uni is null)
            {
                throw new NotFoundException("Restaurant not found");
            }

            uni.Name = dto.Name;
            uni.Description = dto.Description;
            uni.Type = dto.Type;
            uni.ContactEmail = dto.ContactEmail;

            _dbContext.SaveChanges();

        }

    }
}
