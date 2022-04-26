using Microsoft.AspNetCore.Mvc.Testing;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using System.Net.Http;

namespace UniAPI.TEST
{
    public class DepartamentControllerTests
    {
        private HttpClient _client;
        public DepartamentControllerTests()
        {

            var factory = new WebApplicationFactory<Startup>();
            _client = factory.CreateClient();

        }

        //[Fact]
        [Theory]
        [InlineData(1,1)]
        [InlineData(2,2)]
        //[InlineData(2,1)]
        public async Task GetAll_WithQueryParematers_ReturnsOkResult(int uniIndex,int depIndex)
        {

            //arrange


            //act


            var response = await _client.GetAsync("/api/university/"+uniIndex+"/department/"+depIndex);




            //assert

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }


        [Theory]
        [InlineData(11, 12)]
        [InlineData(32, 2)]
        [InlineData(2,2)]
        public async Task GetAll_WithQueryParematers_ReturnsUriTooLong(int uniIndex, int depIndex)
        {

            //arrange



            //act


            var response = await _client.GetAsync("/api/university/" + uniIndex + "/department/" + depIndex);




            //assert

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.RequestUriTooLong);
        }
    }


}
