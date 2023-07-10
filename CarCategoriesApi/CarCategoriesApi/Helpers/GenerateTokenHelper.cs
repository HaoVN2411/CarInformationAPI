using CarCategoriesApi.Data;
using CarCategoriesApi.Models;
using CarCategoriesApi.Repositories;
using CarCategoriesApi.Repository;
using CarCategoriesApi.Services;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace CarCategoriesApi.Helpers
{
    public class GenerateTokenHelper
    {
        private readonly IAccountRepository _accountService;
        private readonly ITokenRepository _TokenRepo;
        private readonly IConfiguration _configuration;

        public GenerateTokenHelper(IAccountRepository accountService, IConfiguration configuration, ITokenRepository tokenRepository)
        {
            _accountService = accountService;
            _TokenRepo = tokenRepository;
            _configuration = configuration;
        }

        public async Task<TokenModel> GenerateToken(UserModel userLogin)
        {
            var jwtTokenHandle = new JwtSecurityTokenHandler();

            var secretKeyBytes = Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]);

            var tokenDescription = new SecurityTokenDescriptor
            {
                Audience = _configuration["JWT:ValidAudience"],
                Issuer = _configuration["JWT:ValidIssuer"],
                Subject = new System.Security.Claims.ClaimsIdentity(new[]
                {
                    new Claim (JwtRegisteredClaimNames.Name, userLogin.FullName),
                    new Claim (JwtRegisteredClaimNames.Email, userLogin.Email),
                    new Claim (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim ("username", userLogin.Username),
                    new Claim (ClaimTypes.Role, userLogin.Role),
                }),
                IssuedAt = DateTime.Now,
                Expires = DateTime.Now.AddSeconds(20),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKeyBytes),
                SecurityAlgorithms.HmacSha256Signature),
            };
            var Token = jwtTokenHandle.CreateToken(tokenDescription);
            var accessToken = jwtTokenHandle.WriteToken(Token);
            var refreshToken = RefreshToken();
            var tokenEntity = new RefreshToken
            {
                token = refreshToken,
                userID = userLogin.id,
                JwtId = Token.Id,
                isUsed = false,
                CreateTime = DateTime.Now,
                ExpireTime = DateTime.Now.AddMonths(6),
            };

            await _TokenRepo.CreateRefreshToken(tokenEntity);

            return new TokenModel
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                Message = "Success"
            };
        }

        public string RefreshToken()
        {
            var random = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(random);
                return Convert.ToBase64String(random);
            };
        }

        public async Task<TokenModel> RenewTokenAsync(TokenModel tokenModel)
        {
            var jwtTokenHandle = new JwtSecurityTokenHandler();
            var secretKeyBytes = Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]);
            var TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidAudience = _configuration["JWT:ValidAudience"],
                ValidIssuer = _configuration["JWT:ValidIssuer"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes
                        (_configuration["JWT:Secret"]!)),
                ClockSkew = TimeSpan.Zero,
                ValidateLifetime = false
            };

            try
            {
                var tokenVerification = jwtTokenHandle.ValidateToken(tokenModel.AccessToken,
                    TokenValidationParameters, out var validatedToken);
                if (validatedToken is JwtSecurityToken jwtSecurityToken)
                {
                    var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                        StringComparison.InvariantCultureIgnoreCase);
                    if (result == false)
                    {
                        return new TokenModel
                        {
                            Message = "Invalid Token"
                        };
                    }
                }
                var expiryDate = long.Parse(tokenVerification.Claims.
                    Single(x => x.Type == JwtRegisteredClaimNames.Exp).Value);
                var expireDateTimeUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(expiryDate).ToUniversalTime();
                if (expireDateTimeUtc > DateTime.UtcNow)
                {
                    return new TokenModel
                    {
                        Message = "This Token hasn't expired yet"
                    };
                }
                var storeToken = _TokenRepo.GetRefreshToken().Result.Where(x => x.token == tokenModel.RefreshToken).SingleOrDefault();
                if (storeToken == null)
                {
                    return new TokenModel
                    {
                        Message = "Refresh Token does not exist"
                    };
                }
                if (storeToken.isUsed == true)
                {
                    return new TokenModel
                    {
                        Message = "Refresh Token has been used"
                    };
                }
                var jti = tokenVerification.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Jti).Value;
                if (storeToken.JwtId != jti)
                {
                    return new TokenModel
                    {
                        Message = "AccessToken is not matched"
                    };
                }

                storeToken.isUsed = true;
                await _TokenRepo.UpdateToken(storeToken);
                var user = _accountService.getAllUser().Result.Where(x => x.id == storeToken.userID).SingleOrDefault();
                var token = await GenerateToken(new UserModel { id = user.id, Email = user.Email, FullName = user.FullName,
                Passwpord = user.Passwpord, Role = user.Role, Username = user.Username});
                return new TokenModel
                {
                    AccessToken = token.AccessToken,
                    RefreshToken = token.RefreshToken,
                    Message = "Success",
                };
            }
            catch
            {
                return new TokenModel
                {
                    Message = "Error at Generate Token"
                };
            }
        }

    }
}
