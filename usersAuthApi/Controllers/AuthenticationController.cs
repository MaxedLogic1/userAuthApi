using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using usersAuthApi.Models.DTO;
using usersAuthApi.Repositories;

namespace usersAuthApi.Controllers
{
    [Route("UserAuthentication")]
    [ApiController]

    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthRepository _iUserRepository;
        public AuthenticationController(IAuthRepository iUserRepository)
        {
            _iUserRepository = iUserRepository;
        }
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] registerRequestDto registerDto)
        {
            try
            {
                if (registerDto == null)
                {
                    return BadRequest("Invalid Data");
                }
                var user = await _iUserRepository.RegisterUserAsync(registerDto);
                if (user == null)
                {
                    return Ok(new { Message = $"{registerDto.Name} Already Exist: " });
                }
                else
                {
                    return Ok(new { Message = $"{registerDto.Name} Regiter Successfully: " });
                }

            }
            catch (Exception)
            {
                throw new ArgumentException("Invalid Register Credentials ");
            }
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] loginRequestDto loginDto)
        {
            try
            {
                if (loginDto.UserName == null || loginDto.Password == null)
                {
                    return BadRequest("Invalid Data");
                }
                var user = await _iUserRepository.LoginUserAsync(loginDto);
                if (user == null)
                {
                    return Ok(new { Message = $"{loginDto.UserName} Does Not Exist: " });
                }
                else
                {
                    return Ok(new { Message = $"{loginDto.UserName} Successfully Login: ", User = user });
                }

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while processing your request.", Error = ex.Message });
            }
        }
        [HttpPost]
        [Route("Forget/Password")]
        public async Task<IActionResult> ForgetPassword([FromBody] forgetPasswordDto forget_PasswordDto)
        {
            try
            {
                if (forget_PasswordDto.UserName == null)
                {
                    return BadRequest("Invalid Data");
                }
                var result = await _iUserRepository.ForgetPasswordAsync(forget_PasswordDto);
                if (result == null)
                {
                    return Ok(new { Message = $"{forget_PasswordDto.UserName} Does Not Exist: " });
                }
                else
                {
                    return Ok(new { Message = $"Forget Password Lind Send to {forget_PasswordDto.UserName} Successfully: " });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
