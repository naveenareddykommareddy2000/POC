namespace Mango.Services.AuthAPI.Models.Service.IService
{
    public interface IJwtTokenGenerator
    {
        public string GenerateToken(ApplicationUser applicationUser);
    }
}
