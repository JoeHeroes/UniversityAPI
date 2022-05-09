using FluentAssertions;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UniAPI.Entites;
using UniAPI.Models;
using Xunit;


namespace UniAPI.TEST
{
    public class UniversityControllerTests:IClassFixture<WebApplicationFactory<Startup>>
    {

        private HttpClient _client;
        private WebApplicationFactory<Startup> _factory;
        public UniversityControllerTests(WebApplicationFactory<Startup> factory)
        {
            _factory = factory
                .WithWebHostBuilder(builder => {
                    builder.ConfigureServices(services =>
                    {
                        var dbContextOptions = services.SingleOrDefault(services => services.ServiceType == typeof(DbContextOptions<UniversityDbContext>));

                        services.Remove(dbContextOptions);

                        services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();

                        services.AddMvc(option => option.Filters.Add(new FakeUserFilter()));

                        services.AddDbContext<UniversityDbContext>(options => options.UseInMemoryDatabase("UniversityDb"));
                    });
                });
               _client = _factory.CreateClient();
        }






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


        }


        [Fact]
        public async Task CreateUniversity_WithValidModel_ReturnCreatedStatus()
        {

            //arrange
            var model = new CreateUniversityDto()
            {
                Name = "TestRestaurant",
                City = "Kraków",
                Street = "Długa 5"
            };


            var httpContent = model.ToJsonHttpContent();

            //act

            var respone = await _client.PostAsync("/api/university", httpContent);

            //assert

            respone.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
            respone.Headers.Location.Should().NotBeNull();

        }

        /*
         {

            //arrange
            var model = new CreateUniversityDto()
            {
                Name = "TestRestaurant",
                City = "Kraków",
                Street = "Długa 5"
            };


            var json = JsonConvert.SerializeObject(model);

            var httpContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            //act

            var respone = await _client.PostAsync("/api/university", httpContent);

            //assert

            respone.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
            respone.Headers.Location.Should().NotBeNull();

        }
         */

        [Fact]

        public async Task CreateUniversity_WithValidModel_ReturnBadStatus()
        {

            //arrange
            var model = new CreateUniversityDto()
            {
                ContactEmail = "FuckingVarFuck@wp.pl",
                City = "Kraków",
                Street = "Długa 5"
            };



            var httpContent = model.ToJsonHttpContent();


            //act

            var respone = await _client.PostAsync("/api/university", httpContent);

            //assert

            respone.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }



        [Fact]
        public async Task DeleteUniversity_ForNotExistingUniversity_ReturnForbiden()
        {

            //arrange
           
            //act

            var respone = await _client.DeleteAsync("/api/university/7");

            //assert

            respone.StatusCode.Should().Be(System.Net.HttpStatusCode.Forbidden);
        }



        [Fact]
        public async Task DeleteUniversity_ForUniversityOwner_ReturnNotContent()
        {

            //arrange

          

            var universities = new University()
            {
                CreateById = 1,
                Name = "Test"
            };

            SeedUniveristy(universities);

            //act

            var respone = await _client.DeleteAsync("/api/university/"+ universities.Id);

            //assert

            respone.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }



        private void SeedUniveristy(University universities)
        {
            var scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();

            using var scope = scopeFactory.CreateScope();

            var _dbContext = scope.ServiceProvider.GetService<UniversityDbContext>();

            _dbContext.Universities.Add(universities);
            _dbContext.SaveChanges();
        }

        [Fact]
        public async Task DeleteUniversity_ForNonUniveristyOwner_ReturnForbidden()
        {

            //arrange

            var universities = new University()
            {
                CreateById = 900,
            };

            SeedUniveristy(universities);


            //act

            var respone = await _client.DeleteAsync("/api/university/" + universities.Id);

            //assert

            respone.StatusCode.Should().Be(System.Net.HttpStatusCode.Forbidden);
        }



    }
}
