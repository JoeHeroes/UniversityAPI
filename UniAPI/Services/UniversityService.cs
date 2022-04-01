using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using UniAPI.Authorization.Policy;
using UniAPI.Entites;
using UniAPI.Exceptions;
using UniAPI.Models;

namespace UniAPI.Services
{
    public class UniversityService : IUniversityService
    {
        private readonly UniversityDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<UniversityService> _logger;
        private readonly IAuthorizationService _authorizationservice;
        private readonly IUserContextService _userContextService;

        public UniversityService(UniversityDbContext dbContext, IMapper mapper, ILogger<UniversityService> logger,
            IAuthorizationService authorizationservice, IUserContextService userContextService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
            _authorizationservice = authorizationservice;
            _userContextService = userContextService;
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
            uni.CreateById = _userContextService.GetUserId;
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
            var auto = _authorizationservice.AuthorizeAsync(_userContextService.User, uni, new ResourceOperationRequirement(ResourcOperation.Delete)).Result;


            if (!auto.Succeeded)
            {
                throw new ForbidExeption("");
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


            var auto = _authorizationservice.AuthorizeAsync(_userContextService.User, uni, new ResourceOperationRequirement(ResourcOperation.Update)).Result;


            if (!auto.Succeeded)
            {
                throw new ForbidExeption("");
            }

            uni.Name = dto.Name;
            uni.Description = dto.Description;
            uni.Type = dto.Type;
            uni.ContactEmail = dto.ContactEmail;

            _dbContext.SaveChanges();

        }

    }
}
