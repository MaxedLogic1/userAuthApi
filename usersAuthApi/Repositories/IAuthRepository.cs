using usersAuthApi.Models.Domain;
using usersAuthApi.Models.DTO;

namespace usersAuthApi.Repositories
{
    public interface IAuthRepository
    {
       
        Task<UserModel> RegisterUserAsync(registerRequestDto registerDto);
        Task<loginResponseDto> LoginUserAsync(loginRequestDto loginDto);
        Task<UserModel> ForgetPasswordAsync(forgetPasswordDto forget_PasswordDto);

    }
}
