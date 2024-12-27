using usersAuthApi.Models.Domain;
using usersAuthApi.Models.DTO;

namespace usersAuthApi.Repositories
{
    public interface IAuthRepository
    {
        Task<string> RegisterUserAsync(registerRequestDto registerDto);
        Task<loginResponseDto> LoginUserAsync(loginRequestDto loginDto);
        Task<string> ForgetPasswordAsync(forgetPasswordDto forget_PasswordDto);

    }
}
