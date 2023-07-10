using AutoMapper;
using CarCategoriesApi.Data;
using CarCategoriesApi.Helpers;
using CarCategoriesApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CarCategoriesApi.Repositories
{
    public interface IAccountRepository
    {
        public Task<List<UserLogin>> getAllUser();
        public Task CreateUser(UserLogin userLogin);
    }

    public class AccountRepository : IAccountRepository
    {
        private readonly CarStoreContext _context;

        public AccountRepository(CarStoreContext context)
        {
            _context = context;
        }

        public async Task<List<UserLogin>> getAllUser()
        {
            var listUserLogins = await _context.UserLogins.ToListAsync();
            return listUserLogins;
        }

        public async Task CreateUser (UserLogin userLogin)
        {
            await _context.UserLogins.AddAsync(userLogin);
            _context.SaveChanges();
        }

    }
}
