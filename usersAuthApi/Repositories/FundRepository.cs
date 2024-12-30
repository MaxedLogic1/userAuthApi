using System.Numerics;
using Microsoft.EntityFrameworkCore;
using usersAuthApi.ApplicationDbContext;
using usersAuthApi.Models.Domain;
using usersAuthApi.Models.DTO;

namespace usersAuthApi.Repositories
{
    public class FundRepository : IFundRepository
    {
        private readonly userDbContext _userDbContext;
        public FundRepository(userDbContext userDbContext)
        {
            _userDbContext = userDbContext;
        }
        public async Task<fundTransectionResponseDto> AddFundTransactionAsync(fundTransactionRequestDto requestDto)
        {
            if (requestDto == null)
            {
                throw new ArgumentNullException(nameof(requestDto));
            }
            //Add new Images
            if (requestDto.Image == null || requestDto.Image.Length == 0)
            {
                throw new ArgumentException("No image file provided.");
            }

            // Validate image size (max 10MB)
            if (requestDto.Image.Length > 5 * 1024 * 1024)
            {
                throw new ArgumentException("Image size must be less than 10MB.");
            }

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".pdf" };
            var fileExtension = Path.GetExtension(requestDto.Image.FileName).ToLower();
            if (!allowedExtensions.Contains(fileExtension))
            {
                throw new ArgumentException("Only .jpg, .jpeg, and .png files are allowed.");
            }

            var uniqueFileName = $"Photo_{new Random().Next(1000, 9999)}{fileExtension}";

            var uploadDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Images");

            if (!Directory.Exists(uploadDirectory))
            {
                Directory.CreateDirectory(uploadDirectory);
            }

            var filePath = Path.Combine(uploadDirectory, uniqueFileName);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await requestDto.Image.CopyToAsync(fileStream);
            }

            var fundTransaction = new FundTransactionModel
            {
                GId = null,
                PId = requestDto.PId,
                CreditAmount = requestDto.CreditAmount,
                Remark = requestDto.Remark,
                Type = "Add Fund",
                TransactionDate = DateTime.Now,
                Images = uniqueFileName,
                TxNoId = $"TX_{new Random().Next(1000, 9999)}"
            };

            await _userDbContext.Tab_FundTransaction.AddAsync(fundTransaction);
            //add amon
            await _userDbContext.SaveChangesAsync();

            decimal totalAmount = await _userDbContext.Tab_FundTransaction
                      .Where(f => f.PId == requestDto.PId)
                      .SumAsync(f => (decimal?)f.CreditAmount) ?? 0;

            // Prepare response DTO
            var responseDto = new fundTransectionResponseDto
            {
                Id = fundTransaction.Id,
                PId = fundTransaction.PId,
                CreditAmount = fundTransaction.CreditAmount,
                TotalAmount = totalAmount,
                Remark = fundTransaction.Remark,
                TransactionDate = fundTransaction.TransactionDate,
                Type = fundTransaction.Type,
                TxNoId = fundTransaction.TxNoId,
                Image = $"/userAuthApi/Images/{uniqueFileName}"
            };

            return responseDto;
        }




    }
}


