using AutoMapper;
using CarCategoriesApi.Data;
using CarCategoriesApi.Models;
using CarCategoriesApi.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CarCategoriesApi.Services
{
    public interface IBrandService
    {
        public Task<bool> AddBrandAsync(CreateBrandInfoModel brandModel);
        public Task<bool> DeleteBrandAsync(int id);
        public Task<List<BrandResponseModel>> getAllBrandAsync();
        public Task<bool> UpdateBrandAsync(int id, CreateBrandInfoModel brandModel);
        public Task<BrandResponseModel> getSingleBrandAsync(string brandName);
    }
    public class BrandService : IBrandService
    {
        private readonly IBrandRepository _brandRepo;
        private readonly IMapper _mapper;

        public BrandService(IBrandRepository brandRepository, IMapper mapper)
        {
            _brandRepo = brandRepository;
            _mapper = mapper;
        }

        public async Task<bool> AddBrandAsync(CreateBrandInfoModel brandModel)
        {
            var newBrand = _mapper.Map<BrandInfo>(brandModel);
            if (newBrand != null)
            {
                await _brandRepo.AddBrandAsync(newBrand);
                return true;
            }
            return false;
        }

        public async Task<bool> DeleteBrandAsync(int id)
        {
            var brands = await _brandRepo.getAllBrandAsync();
            var deleteBrand = brands.SingleOrDefault(c => c.BrandId == id);
            if (deleteBrand != null)
            {
                await _brandRepo.DeleteBrandAsync(deleteBrand);
                return true;
            }
            else return false;
        }

        public async Task<List<BrandResponseModel>> getAllBrandAsync()
        {
            var brands = await _brandRepo.getAllBrandAsync();
            return _mapper.Map<List<BrandResponseModel>>(brands);
        }

        public async Task<bool> UpdateBrandAsync(int id, CreateBrandInfoModel brandModel)
        {
            var brands = await _brandRepo.getAllBrandAsync();
            var updateModel = brands.Where(x => x.BrandId == id).SingleOrDefault();
            if (updateModel != null)
            {
                updateModel.BrandName = brandModel.BrandName;
                updateModel.BrandDescription = brandModel.BrandDescription;
                await _brandRepo.UpdateBrandAsync(updateModel);
                return true;
            }
            return false;
        }
        public async Task<BrandResponseModel> getSingleBrandAsync(string brandName)
        {
            var brands = await _brandRepo.getAllBrandAsync();
            var brand = brands.Where(x => x.BrandName.ToLower().Contains(brandName.ToLower())).SingleOrDefault();
            return _mapper.Map<BrandResponseModel>(brand);
        }
    }
}
