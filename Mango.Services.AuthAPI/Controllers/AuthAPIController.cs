using Mango.Services.AuthAPI.Models.DTO;
using Mango.Services.AuthAPI.Models.Service.IService;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.AuthAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthAPIController : ControllerBase
    {
        private readonly IAuthService _authService;
        protected ResponseDto _response;
        public AuthAPIController(IAuthService authService, ResponseDto response)
        {
            _authService = authService;
            _response = new();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequestDto model)
        {
            var errorMessage=await _authService.Register(model);
            if (!string.IsNullOrEmpty(errorMessage))
            {
                _response.IsSuccess = false;
                _response.Message=errorMessage;
                return BadRequest(_response);
            }
            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]LoginRequestDto model)
        {
            var loginResponse = await _authService.Login(model);
            if(loginResponse.User==null)
            {
                _response.IsSuccess = false;
                _response.Message = "username and Password is incorrect";
                return BadRequest(_response);
            }
            _response.Result=loginResponse;
            return Ok(_response);
        }

        [HttpPost("AssignRole")]
        public async Task<IActionResult> Assignrole([FromBody] RegistrationRequestDto model)
        {
            var assignRoleSuccessfull = await _authService.AssignRole(model.Email,model.Role.ToUpper());
            if(!assignRoleSuccessfull)
            {
                _response.IsSuccess = false;
                _response.Message = "OOppss Error Occurred";
                return BadRequest(_response);
            }
            return Ok(_response);
        }

    }
}
