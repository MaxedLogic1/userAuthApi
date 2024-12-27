using usersAuthApi.ApplicationDbContext;
using usersAuthApi.Models.Domain;
using usersAuthApi.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace usersAuthApi.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly userDbContext _userDbContext;

        public AuthRepository(userDbContext userDbContext)
        {
            _userDbContext = userDbContext;
        }

        public async Task<string> RegisterUserAsync(registerRequestDto registerDto)
        {
            var checkEmail = await _userDbContext.Tab_Register.AnyAsync(u => u.UserName == registerDto.UserName);
            if (checkEmail)
            {
                throw new Exception("Email already Exist.");
            }


            var newUser = new UserModel
            {
               
                RandomId = $"loco_{new Random().Next(1000, 9999)}",
                Name = registerDto.Name,
                UserName = registerDto.UserName,
                Password = registerDto.Password,
                IsActive = true,
                EntryDate = DateTime.Now
            };

            _userDbContext.Tab_Register.Add(newUser);
            await _userDbContext.SaveChangesAsync();

            return $"{registerDto.Name} Register Successfully:";
        }

        public async Task<loginResponseDto> LoginUserAsync(loginRequestDto loginDto)
        {
            if (loginDto == null)
            {
                throw new UnauthorizedAccessException("Please Enter the Values");
            }

            var user = await _userDbContext.Tab_Register.SingleOrDefaultAsync(u => u.UserName == loginDto.UserName && u.Password == loginDto.Password);

            if (user == null)
            {
                throw new UnauthorizedAccessException("Invalid credentials.");
            }

            return new loginResponseDto
            {
                Id = user.Id,
                Name=user.Name,
                IsActive = user.IsActive,
                RandomId = user.RandomId,
                UserName= user.UserName,
                EntryDate =user.EntryDate

            };

        }

        public async Task<string> ForgetPasswordAsync(forgetPasswordDto forget_PasswordDto)
        {
            if (forget_PasswordDto == null)
            {
                throw new UnauthorizedAccessException("Please Enter the Values");
            }

            var user = await _userDbContext.Tab_Register.SingleOrDefaultAsync(u => u.UserName == forget_PasswordDto.UserName);
             
            if (user == null)
            {
                throw new UnauthorizedAccessException("Invalid UserName");
            }
            return "link send to your mail Successfully";

        }
    }
}



