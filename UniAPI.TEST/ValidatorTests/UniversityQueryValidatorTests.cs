using FluentValidation.TestHelper;
using System.Collections.Generic;
using System.Linq;
using UniAPI.Entites;
using UniAPI.Models;
using UniAPI.Models.Validators;
using Xunit;

namespace UniAPI.TEST
{
    public class UniversityQueryValidatorTests
    {
        public static IEnumerable<object[]> GetSampleValidData()
        {
            var list = new List<UniversityQuery>()
            {
                new UniversityQuery()
                {
                    PageNumber = 1,
                    PageSize = 10,
                },
                new UniversityQuery()
                {
                    PageNumber = 2,
                    PageSize = 15,
                },
                new UniversityQuery()
                {
                    PageNumber = 22,
                    PageSize = 5,
                    SortBy = nameof(University.Name)
                },
                new UniversityQuery()
                {
                    PageNumber = 12,
                    PageSize = 5,
                    SortBy = nameof(University.Type)
                }
            };

            return list.Select(q => new object[] { q });
        }



        public static IEnumerable<object[]> GetSampleInvalidData()
        {
            var list = new List<UniversityQuery>()
            {
                new UniversityQuery()
                {
                    PageNumber = 0,
                    PageSize = 10,
                },
                new UniversityQuery()
                {
                    PageNumber = 2,
                    PageSize = 13,
                },
                new UniversityQuery()
                {
                    PageNumber = 22,
                    PageSize = 5,
                    SortBy = nameof(University.ContactEmail)
                },
                new UniversityQuery()
                {
                    PageNumber = 12,
                    PageSize = 5,
                    SortBy = nameof(University.ContactNumber)
                }
            };

            return list.Select(q => new object[] { q });
        }


        [Theory]
        [MemberData(nameof(GetSampleValidData))]
        public void Validate_ForCorrectModel_ReturnsSucces(UniversityQuery model) 
        {

            //arrange
            var validator = new UniversityQueryValidator();
         

            //act

            var result = validator.TestValidate(model);

            //assert

            result.ShouldNotHaveAnyValidationErrors();
            

        } 
        
        
        [Theory]
        [MemberData(nameof(GetSampleInvalidData))]
        public void Validate_ForCorrectModel_ReturnsFailure(UniversityQuery model) 
        {

            //arrange
            var validator = new UniversityQueryValidator();
         

            //act

            var result = validator.TestValidate(model);

            //assert

            result.ShouldHaveAnyValidationError();
            

        }
    }
}
