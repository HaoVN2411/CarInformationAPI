using AutoMapper;
using CarCategoriesApi.Data;
using CarCategoriesApi.Models;
using CarCategoriesApi.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CarCategoriesApi.Services
{
    public interface ICarService
    {
        public Task<List<CarResponseModel>> getAllCarAsync();
        public Task<CarResponseModel> getCarAsync(string carName);
        public Task<List<CarResponseModel>> getCarFromBrandAsync(List<BrandResponseModel> brandModel);
        public Task<bool> AddCarAsync(CreateCarRequestModel CarModel);
        public Task<bool> DeleteCarAsync(int id);
        public Task<bool> UpdateCarAsync(int id, CreateCarRequestModel CarModel);
    }
    public class CarService : ICarService
    {
        private readonly ICarRepository _carRepo;
        private readonly IMapper _mapper;
        private readonly IBrandRepository _brandRepo;

        public CarService(ICarRepository carRepository, IMapper mapper, IBrandRepository brandRepository)
        {
            _carRepo = carRepository;
            _mapper = mapper;
            _brandRepo = brandRepository;
        }

        public async Task<bool> AddCarAsync(CreateCarRequestModel CarModel)
        {
            var newCar = _mapper.Map<CarInfo>(CarModel);
            if (newCar != null)
            {
                await _carRepo.AddCarAsync(newCar);
                return true;
            }
            return false;
        }

        public async Task<bool> DeleteCarAsync(int id)
        {
            var deleteCar = _carRepo.getAllCarAsync().Result.Where(c => c.Id == id).SingleOrDefault();
            if (deleteCar != null)
            {
                await _carRepo.DeleteCarAsync(deleteCar);
                return true;
            }
            return false;
        }

        public async Task<List<CarResponseModel>> getAllCarAsync()
        {
            var cars = await _carRepo.getAllCarAsync();
            var CarResponseModel = new List<CarResponseModel>();
            foreach (var car in cars)
            {
                CarResponseModel carResponseModel = new CarResponseModel()
                {
                    Id = car.Id,
                    Name = car.Name,
                    BrandName = _brandRepo.getAllBrandAsync().Result.Where(x => x.BrandId == car.BrandId).SingleOrDefault()!.BrandName,
                    Description = car.Description,
                    Price = car.Price,
                };
                CarResponseModel.Add(carResponseModel);
            }
            return CarResponseModel;
        }

        public async Task<CarResponseModel> getCarAsync(string carName)
        {
            var cars = await _carRepo.getAllCarAsync();
            var car = cars.Where(x => x.Name.ToLower().Contains(carName.ToLower())).SingleOrDefault();
            return new CarResponseModel
            {
                Id = car.Id,
                Name = car.Name,
                BrandName = _brandRepo.getAllBrandAsync().Result.Where(x => x.BrandId == car.BrandId).SingleOrDefault()!.BrandName,
                Description = car.Description,
                Price = car.Price,
            };
        }

        public async Task<List<CarResponseModel>> getCarFromBrandAsync(List<BrandResponseModel> brandModel)
        {
            var carListByBrand = new List<CarResponseModel>();
            var cars = await _carRepo.getAllCarAsync();
            foreach (var brand in brandModel)
            {
                var carByBrand = cars.Where(x => x.BrandId == brand.BrandId).ToList();
                if (carByBrand != null)
                {
                    foreach (var item in carByBrand)
                    {
                        var car = new CarResponseModel
                        {
                            BrandName = brand.BrandName,
                            Name = item.Name,
                            Description = item.Description,
                            Price = item.Price,
                            Id = item.Id
                        };
                        carListByBrand.Add(car);
                    }
                }
            }
            return carListByBrand;
        }

        public async Task<bool> UpdateCarAsync(int id, CreateCarRequestModel CarModel)
        {
            var cars = await _carRepo.getAllCarAsync();
            var updateCar = cars.Where(x => x.Id == id).SingleOrDefault();
            if (updateCar != null)
            {
                updateCar.Name = CarModel.Name;
                updateCar.Description = CarModel.Description;
                updateCar.Price = CarModel.Price;
                updateCar.BrandId = CarModel.BrandId;
                await _carRepo.UpdateCarAsync(updateCar);
                return true;
            }
            return false;
        }
    }
}
