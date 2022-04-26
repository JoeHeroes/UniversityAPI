using Microsoft.AspNetCore.Mvc.Testing;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;

namespace UniAPI.TEST
{
    public class UniversityControllerTests
    {

        [Fact]
        public async Task GetOne_WithQueryParametr_ReturnOkStatus()
        {
            //arrange

            var factory = new WebApplicationFactory<Startup>();
            var client = factory.CreateClient();

            //act

            var response = await client.GetAsync("/api/university/1");


            //assert

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);


            //assert

        }


    }
}
