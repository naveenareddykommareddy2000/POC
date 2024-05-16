using Mango.Services.AuthAPI.Models.DTO;

namespace Mango.Services.AuthAPI.Models.Service.IService
{
    public interface IAuthService
    {
        public Task<string> Register(RegistrationRequestDto registerRequestDto);
        public Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto);

        public Task<bool> AssignRole(string email, string roleName);
    }
}
