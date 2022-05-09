using FluentValidation.TestHelper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using UniAPI.Authorization;
using UniAPI.Entites;
using UniAPI.Models;
using UniAPI.Models.Validators;

namespace UniAPI.TEST
{
    public class RegisterUserDtoValidatorTests
    {
        private UniversityDbContext _dbContext;

        public RegisterUserDtoValidatorTests()
        {
            var builder = new DbContextOptionsBuilder<UniversityDbContext>();

            builder.UseInMemoryDatabase("TestDb");

            _dbContext = new UniversityDbContext(builder.Options);
        }

        public void Seed()
        {
            var testUsers = new List<User>()
            {
                new User()
                {
                    Email = "tes2t@test.com"
                },
                new User()
                {
                    Email = "test3@test.com"
                },
            };
            _dbContext.Users.AddRange(testUsers);   
        }

        public void Validate_ForValidModel_ReturnsSuccess()
        {
            //arrange

            var model = new RegisterUserDto()
            {
                Email = "test@wp.pl",
                Password = "pass123",
                ConfirmPassword = "pass123",
            };


          

            var validator = new RegisterUserDtoValidator(_dbContext);



            //act

            var result = validator.TestValidate(model);

            //assert

            result.ShouldNotHaveAnyValidationErrors();
        } 
        
        
        public void Validate_ForValidModel_ReturnsFailure()
        {
            //arrange

            var model = new RegisterUserDto()
            {
                Email = "test2@wp.pl",
                Password = "pass123",
                ConfirmPassword = "pass123",
            };


          

            var validator = new RegisterUserDtoValidator(_dbContext);



            //act

            var result = validator.TestValidate(model);

            //assert

            result.ShouldHaveAnyValidationError();
        }
    }
}
