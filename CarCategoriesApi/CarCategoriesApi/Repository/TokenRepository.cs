using CarCategoriesApi.Data;
using Microsoft.EntityFrameworkCore;

namespace CarCategoriesApi.Repository
{
    public interface ITokenRepository
    {
        public Task CreateRefreshToken(RefreshToken token);
        public Task<List<RefreshToken>> GetRefreshToken();
        public Task UpdateToken(RefreshToken storeToken);
    }
    public class TokenRepository : ITokenRepository
    {
        private readonly CarStoreContext _context;

        public TokenRepository(CarStoreContext carStoreContext)
        {
            _context = carStoreContext;
        }

        public async Task CreateRefreshToken(RefreshToken token)
        {
            await _context.refreshTokens.AddAsync(token);
            _context.SaveChanges();
        }
        public async Task<List<RefreshToken>> GetRefreshToken()
        {
            var Token = await _context.refreshTokens.ToListAsync();
            return Token;
        }
        public async Task UpdateToken(RefreshToken storeToken)
        {
            _context.Update(storeToken);
            await _context.SaveChangesAsync();
        }
    }
}
