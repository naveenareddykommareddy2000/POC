using Mango.Services.AuthAPI.Data;
using Mango.Services.AuthAPI.Models.DTO;
using Mango.Services.AuthAPI.Models.Service.IService;
using Microsoft.AspNetCore.Identity;

namespace Mango.Services.AuthAPI.Models.Service
{
    public class AuthService : IAuthService
    {
         private readonly ApplicationDbContext _context;
         private readonly UserManager<ApplicationUser> _userManager;
         private readonly RoleManager<IdentityRole> _roleManager;
         private readonly IJwtTokenGenerator _jwtTokenGenerator;

      public AuthService(ApplicationDbContext context, IJwtTokenGenerator ijwtTokenGenerator,UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _jwtTokenGenerator = ijwtTokenGenerator;
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<bool> AssignRole(string email, string roleName)
        {
            var user=_context.ApplicationUsers.FirstOrDefault(u => u.Email.ToLower() == email.ToLower());
            if(user!=null)
            {
                if(!_roleManager.RoleExistsAsync(roleName).GetAwaiter().GetResult())
                {
                    //create role if doesnot exist
                    _roleManager.CreateAsync(new IdentityRole(roleName)).GetAwaiter().GetResult();
                }
                await _userManager.AddToRoleAsync(user, roleName);
                return true;
            }
            return false;
        }

        public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
        {
            var user =  _context.ApplicationUsers.FirstOrDefault(u => u.UserName.ToLower() == loginRequestDto.Name.ToLower());
            bool isValid=await _userManager.CheckPasswordAsync(user, loginRequestDto.Password);
            if(user==null || isValid==false)
            {
                return new LoginResponseDto() { User = null, Token = "" };

            }
            //if user found generate Token
            var token = _jwtTokenGenerator.GenerateToken(user);
            UserDto userDto = new()
            {
                Email = user.Email,
                ID = user.Id,
                Name = user.Name,
                PhoneNumber = user.PhoneNumber,
            };
            LoginResponseDto loginResponseDto = new LoginResponseDto()
            {
                User = userDto,
                Token = token
            };
            return loginResponseDto;
        }

        public async Task<string> Register(RegistrationRequestDto registerRequestDto)
        {
            ApplicationUser user = new()
            {
                UserName = registerRequestDto.Email,
                Email = registerRequestDto.Email,
                NormalizedEmail = registerRequestDto.Email.ToUpper(),
                Name = registerRequestDto.Name,
                PhoneNumber = registerRequestDto.PhoneNumber

            };
            try
            {
                var result = await _userManager.CreateAsync(user,registerRequestDto.Password);
                if(result.Succeeded)
                {
                    var userToReturn = _context.ApplicationUsers.First(u => u.UserName == registerRequestDto.Email);
                    UserDto userDto = new()
                    {
                        Email = userToReturn.Email,
                        ID = userToReturn.Id,
                        Name = userToReturn.Name,
                        PhoneNumber = userToReturn.PhoneNumber
                    };
                    return "";
                }
                else
                {
                    return result.Errors.FirstOrDefault().Description;
                }
            }
            catch(Exception ex)
            {

            }
            return "Error Encountered";
        }
    }
}
