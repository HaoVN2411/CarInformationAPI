using AutoMapper;
using CarCategoriesApi.Data;
using CarCategoriesApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;

namespace CarCategoriesApi.Repositories
{
    public interface IBrandRepository
    {
        public Task<List<BrandInfo>> getAllBrandAsync();
        public Task AddBrandAsync(BrandInfo brandInfo);
        public Task UpdateBrandAsync(BrandInfo brandInfo);
        public Task DeleteBrandAsync(BrandInfo brandInfo);
    }

    public class BrandRepository : IBrandRepository
    {
        private readonly CarStoreContext _context;

        public BrandRepository(CarStoreContext carStoreContext)
        {
            _context = carStoreContext;
        }

        public async Task AddBrandAsync(BrandInfo brandInfo)
        {
            _context.BrandInfos!.Add(brandInfo);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteBrandAsync(BrandInfo brandInfo)
        {
            _context.BrandInfos!.Remove(brandInfo);
            await _context.SaveChangesAsync();
        }

        public async Task<List<BrandInfo>> getAllBrandAsync()
        {
            var brands = await _context.BrandInfos!.ToListAsync();
            return brands;
        }

        public async Task UpdateBrandAsync(BrandInfo brandInfo)
        {
            _context.BrandInfos!.Update(brandInfo);
            await _context.SaveChangesAsync();
        }
    }
}
