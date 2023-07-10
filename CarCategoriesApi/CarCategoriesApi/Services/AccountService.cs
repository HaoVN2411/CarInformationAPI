using AutoMapper;
using CarCategoriesApi.Data;
using CarCategoriesApi.Helpers;
using CarCategoriesApi.Models;
using CarCategoriesApi.Repositories;

namespace CarCategoriesApi.Services
{
    public interface IAccountService
    {
        public Task<Boolean> SignUpAsync(SignUpModel signUpModel);
        public Task<TokenModel> SignInAsync(UserModel userModel);
        public Task<List<UserModel>> getAllUser();
        public Task<TokenModel> RefreshToken(TokenModel tokenModel);
    }
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepo;
        private readonly GenerateTokenHelper _generateTokenHelper;
        private readonly IMapper _mapper;

        public AccountService(IAccountRepository accountRepository, GenerateTokenHelper generateTokenHelper, IMapper mapper)
        {
            _accountRepo = accountRepository;
            _generateTokenHelper = generateTokenHelper;
            _mapper = mapper;
        }


        public async Task<List<UserModel>> getAllUser()
        {
            var listUserLogins = await _accountRepo.getAllUser();
            return _mapper.Map<List<UserModel>>(listUserLogins);
        }

        public async Task<TokenModel> SignInAsync(UserModel userModel)
        {
            var tokenModel = await _generateTokenHelper.GenerateToken(userModel);
            return tokenModel;
        }

        public async Task<Boolean> SignUpAsync(SignUpModel signUpModel)
        {
            bool checkInsert = false;
            var newUser = new UserLogin
            {
                Username = signUpModel.Username,
                Email = signUpModel.Email,
                FullName = signUpModel.FullName,
                Passwpord = signUpModel.Passwpord,
                Role = "Customer"
            };
            if (newUser != null)
            {
                await _accountRepo.CreateUser(newUser);
                checkInsert = true;
            }
            return checkInsert;
        }

        public async Task<TokenModel> RefreshToken(TokenModel tokenModel)
        {
            var token = await _generateTokenHelper.RenewTokenAsync(tokenModel);
            return token;
        }
    }
}
