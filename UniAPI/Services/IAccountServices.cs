using UniAPI.Models;

namespace UniAPI.Services
{
    public interface IAccountServices
    {
        string GeneratJwt(LoginDto dto);
        void RegisterUser(RegisterUserDto dto);
    }
}