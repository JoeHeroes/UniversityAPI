using FluentAssertions;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using UniAPI.Entites;
using UniAPI.Models;
using Xunit;

namespace UniAPI.TEST
{
    public class DepartamentControllerTests: IClassFixture<WebApplicationFactory<Startup>>
    {
        private HttpClient _client;
        public DepartamentControllerTests(WebApplicationFactory<Startup> factory)
        {
            _client = factory
                .WithWebHostBuilder(builder => {
                    builder.ConfigureServices(services =>
                    {
                        var dbContextOptions = services.SingleOrDefault(services => services.ServiceType == typeof(DbContextOptions<UniversityDbContext>));

                        services.Remove(dbContextOptions);

                        services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();

                        services.AddMvc(option => option.Filters.Add(new FakeUserFilter()));

                        services.AddDbContext<UniversityDbContext>(options => options.UseInMemoryDatabase("UniversityDb"));
                    });
                })
                .CreateClient();
        }



        [Fact]
        public async Task CreatedUniversity_WithValidModel_ReturnsCreatedStatus()
        {
            //arrange 
            var model = new CreateUniversityDto()
            {
                Name = "Siedlce University",
                City = "Siedlce",
                Street = "LongIland 7"

            };

            var httpContent = model.ToJsonHttpContent();
            //act
            var response = await _client.PostAsync("/api/university", httpContent);
            
            //arrange

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
            response.Headers.Location.Should().NotBeNull();

        }



       


        //[Fact]
        [Theory]
 
        [InlineData(2,2)]
        
        public async Task GetAll_WithQueryParematers_ReturnsOkResult(int uniIndex,int depIndex)
        {

            //arrange

            //act

            var response = await _client.GetAsync("/api/university/"+uniIndex+"/department/"+depIndex);

            //assert

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }
        /*
        {
            //arrange

            var factory = new WebApplicationFactory<Startup>();
            var client = factory.CreateClient();

            //act

            var response = await client.GetAsync("/api/university/" + uniIndex + "/department/" + depIndex);

            //assert

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }
        */




        [Theory]
        [InlineData(11, 12)]
        
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
