using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using UniAPI.Authorization;
using UniAPI.Entites;
using UniAPI.Exceptions;
using UniAPI.Models;

namespace UniAPI.Services
{
    public class AccountServices : IAccountServices
    {
        private readonly UniversityDbContext _dbContext;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly AuthenticationSettings _authenticationSetting;
        public AccountServices(UniversityDbContext dbContext, IPasswordHasher<User> passwordHasher, AuthenticationSettings authenticationSetting)
        {
            _dbContext = dbContext;
            _passwordHasher = passwordHasher;
            _authenticationSetting = authenticationSetting;
        }
        public void RegisterUser(RegisterUserDto dto)
        {
            var newUser = new User()
            {
                Email = dto.Email,
                //BirthOfDate = dto.BirthOfDate,
                Nationality = dto.Nationality,
                //PasswordHash = dto.Password,
                RoleId = dto.RoleId

            };

            var hashedPass = _passwordHasher.HashPassword(newUser, dto.Password);

            newUser.PasswordHash = hashedPass;
            _dbContext.Users.Add(newUser);
            _dbContext.SaveChanges();
        }
        
        public string GeneratJwt(LoginDto dto)
        {
            var user = _dbContext
                 .Users
                 .Include(u => u.Role)
                 .FirstOrDefault(u => u.Email == dto.Email);


            if (user is null)
            {
                throw new BadRequestException("Invalid username or password");
            }


            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
            if (result == PasswordVerificationResult.Failed)
            {
                throw new BadRequestException("Invalid username or password");
            }

            var clasims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                new Claim(ClaimTypes.Role, $"{user.Role.Name} "),
                //new Claim("DateOfBirth", user.DateOfBirth.Value.ToString("yyyy-mm--dd")),
                new Claim("Nationality", user.Nationality),
            };


            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSetting.JwtKey));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(_authenticationSetting.JwtExpireDays);


            var token = new JwtSecurityToken(_authenticationSetting.JwtIssuer,
                _authenticationSetting.JwtIssuer,
                clasims,
                expires: expires,
                signingCredentials: cred
                );

            var tokenHander = new JwtSecurityTokenHandler();
            return tokenHander.WriteToken(token);
        }
       
    }
}
