using FluentAssertions;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using UniAPI.Entites;
using UniAPI.Models;
using UniAPI.Services;
using Xunit;


namespace UniAPI.TEST
{
    public class AccountControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {

        private HttpClient _client;
        private WebApplicationFactory<Startup> _factory;
        private Mock<IAccountServices> _accountMocService = new Mock<IAccountServices>();
        public AccountControllerTests(WebApplicationFactory<Startup> factory)
        {
            _factory = factory
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        var dbContextOptions = services.SingleOrDefault(services => services.ServiceType == typeof(DbContextOptions<UniversityDbContext>));

                        services.Remove(dbContextOptions);

                        services.AddSingleton<IAccountServices>(_accountMocService.Object);

                        services.AddMvc(option => option.Filters.Add(new FakeUserFilter()));

                        services.AddDbContext<UniversityDbContext>(options => options.UseInMemoryDatabase("UniversityDb"));
                    });
                });
            _client = _factory.CreateClient();
        }


        [Fact]
        public async Task RegisterUser_ForValidModel_ReturnOK()
        {


            _accountMocService
                .Setup(e => e.GeneratJwt(It.IsAny<LoginDto>()))
                .Returns("jwt");

            //arrange

            var registerUser = new RegisterUserDto()
            {
                Email = "Test@wp.pl",
                Password = "password123",
                ConfirmPassword = "password123"

            };


            var httpContent = registerUser.ToJsonHttpContent();


            //act

            var respone = await _client.PostAsync("/api/account/register", httpContent);

            //assert

            respone.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }


        [Fact]
        public async Task LoginUser_ForRegisterUser_ReturnNoContent()
        {
            //arrange

            var loginDto = new LoginDto()
            {
                Email = "Test@wp.pl",
                Password = "password123"
            };

            var httpContent = loginDto.ToJsonHttpContent();

            //act

            var respone = await _client.PostAsync("/api/account/login", httpContent);

            //assert

            respone.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
        }



        [Fact]
        public async Task RegisterUser_ForValidModel_ReturnBadRequest()
        {
            //arrange

            var registerUser = new RegisterUserDto()
            {
                Password = "password123",
                ConfirmPassword = "password123"

            };


            var httpContent = registerUser.ToJsonHttpContent();


            //act

            var respone = await _client.PostAsync("/api/account/register", httpContent);

            //assert

            respone.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

    }
}
