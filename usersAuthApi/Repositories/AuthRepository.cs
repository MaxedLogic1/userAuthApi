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
       

        public async Task<UserModel> RegisterUserAsync(registerRequestDto registerDto)
        {
            var checkEmail = await _userDbContext.Tab_Register.FirstOrDefaultAsync(u => u.UserName == registerDto.UserName);
            if (checkEmail !=null)
            {
                return null;
            }


            var newUser = new UserModel
            {
               
                RandomId = $"loco_{new Random().Next(1000, 9999)}",
                Name = registerDto.Name,
                UserName = registerDto.UserName,
                Password = registerDto.Password,
                IsActive = true,
                EntryDate = DateTime.UtcNow.AddMinutes(330)
            };

            _userDbContext.Tab_Register.Add(newUser);
            await _userDbContext.SaveChangesAsync();

            //
            return newUser;
        }

        public async Task<loginResponseDto> LoginUserAsync(loginRequestDto loginDto)
        {
            var user = await _userDbContext.Tab_Register.SingleOrDefaultAsync(u => u.UserName == loginDto.UserName && u.Password == loginDto.Password);

            if (user == null)
            {
                return null;
            }

            return new loginResponseDto
            {
                Id = user.Id,
                Name=user.Name,
              //  IsActive = user.IsActive,
                //RandomId = user.RandomId,
                UserName= user.UserName,
                EntryDate =user.EntryDate
            };
        }

        public async Task<UserModel> ForgetPasswordAsync(forgetPasswordDto forget_PasswordDto)
        {
            var user = await _userDbContext.Tab_Register.SingleOrDefaultAsync(u => u.UserName == forget_PasswordDto.UserName);
             
            if (user == null)
            {
                return null;
            }
            else
            {
                return user;
            }
        }
    }
}



