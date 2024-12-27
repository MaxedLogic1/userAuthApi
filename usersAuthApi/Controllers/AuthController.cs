using Microsoft.AspNetCore.Mvc;
using usersAuthApi.Models.DTO;
using usersAuthApi.Repositories;

namespace usersAuthApi.Controllers
{
    [Route("Api/UserAuthentication")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _iUserRepository;
        public AuthController(IAuthRepository iUserRepository)
        {
            _iUserRepository = iUserRepository;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromForm] registerRequestDto registerDto)
        {
            try
            {
                var user = await _iUserRepository.RegisterUserAsync(registerDto);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromForm] loginRequestDto loginDto)
        {
            try
            {
                var user = await _iUserRepository.LoginUserAsync(loginDto);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return Unauthorized(ex.Message);
            }
        }
        [HttpPost]
        [Route("Password/Forget")]
        public async Task<IActionResult> ForgetPassword([FromForm] forgetPasswordDto forget_PasswordDto)
        {
                if (forget_PasswordDto == null)
                {
                    return BadRequest("Please enter the required values."); 
                }
                try
                {
                    var result = await _iUserRepository.ForgetPasswordAsync(forget_PasswordDto);

                    return Ok(result);  
                }
                catch (UnauthorizedAccessException ex)
                {
                    return Unauthorized(ex.Message);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Internal server error: {ex.Message}");  
                }
            }
    }
}
