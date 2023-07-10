using AutoMapper;
using CarCategoriesApi.Data;
using CarCategoriesApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CarCategoriesApi.Repositories
{
    public interface ICarRepository
    {
        public Task<List<CarInfo>> getAllCarAsync();
        public Task AddCarAsync(CarInfo CarInfo);
        public Task UpdateCarAsync(CarInfo carInfo);
        public Task DeleteCarAsync(CarInfo CarInfo);
    }

    public class CarRepository : ICarRepository
    {
        private readonly CarStoreContext _context;

        public CarRepository(CarStoreContext context)
        {
            _context = context;
        }
        public async Task AddCarAsync(CarInfo CarInfo)
        {
            _context.CarInfos!.Add(CarInfo);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCarAsync(CarInfo CarInfo)
        {
            _context.CarInfos!.Remove(CarInfo);
            await _context.SaveChangesAsync();
        }

        public async Task<List<CarInfo>> getAllCarAsync()
        {
            var cars = await _context.CarInfos!.ToListAsync();
            return cars;
        }

        public async Task UpdateCarAsync(CarInfo carInfo)
        {
            _context.CarInfos.Update(carInfo);
            await _context.SaveChangesAsync();
        }
    }
}
