using CarCategoriesApi.Data;
using CarCategoriesApi.Models;
using CarCategoriesApi.Repositories;
using CarCategoriesApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Serilog;
using Ilogger = Serilog.ILogger;

namespace CarCategoriesApi.Controllers
{
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly Ilogger _logger;

        public AccountsController(IAccountService accountService)
        {
            _accountService = accountService;
            _logger = Log.Logger;
        }

        [HttpPost]
        [Route("api/[controller]/SignUp")]
        public async Task<IActionResult> SignUp(SignUpModel signUpModel)
        {
            _logger.Information("Sign Up Infor: ");
            if (_accountService.getAllUser().Result.Where(x => x.Username == signUpModel.Username).Any())
            {
                return BadRequest("Username is existed");
            }
            if (!signUpModel.Passwpord.Equals(signUpModel.ConfirmPasswpord))
            {
                return BadRequest("Password and password confirm is not the same");
            }
            if (!signUpModel.Email.Contains("@"))
            {
                return BadRequest("Email is wrong format");
            }
            var result = await _accountService.SignUpAsync(signUpModel);
            if (result)
            {
                var jsonUser = JsonConvert.SerializeObject(signUpModel);
                _logger.Information(jsonUser);
                return Ok("Successfully");
            }
            return BadRequest();
        }

        [HttpPost]
        [Route("api/[controller]/SignIn")]
        public async Task<IActionResult> SignIn(SignInModel signInModel)
        {
            var userLogin = _accountService.getAllUser().Result.Where(x => x.Username == signInModel.Username
            && x.Passwpord == signInModel.Password).SingleOrDefault();
            if (userLogin == null)
            {
                return BadRequest("Username is invalid or password is wrong, please try again");
            }
            var result = await _accountService.SignInAsync(userLogin);
            if (result == null)
            {
                return Unauthorized();
            }
            var jsonUser = JsonConvert.SerializeObject(signInModel);
            _logger.Information(jsonUser);
            return Ok(result);
        }

        [HttpPost]
        [Route("api/[controller]/RefreshToken")]
        public async Task<IActionResult> RefreshToken(TokenModel tokenModel)
        {
            var result = await _accountService.RefreshToken(tokenModel);
            if (result == null)
            {
                return Unauthorized();
            }
            return Ok(result);
        }

    }
}
