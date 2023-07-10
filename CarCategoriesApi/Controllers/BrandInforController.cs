using CarCategoriesApi.Models;
using CarCategoriesApi.Repositories;
using CarCategoriesApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Serilog;

namespace CarCategoriesApi.Controllers
{
    [ApiController]
    public class BrandInforController : ControllerBase
    {
        private readonly IBrandService _brandServices;
        private readonly Serilog.ILogger _logger;

        public BrandInforController(IBrandService brandService)
        {
            _brandServices = brandService;
            _logger = Log.Logger;
        }

        [HttpGet]
        [Route("api/[controller]/GetAllBrand")]
        public async Task<IActionResult> GetAllBrand()
        {
            try
            {
                _logger.Information("Get All Brand: ");
                var listBrand = await _brandServices.getAllBrandAsync();
                if (listBrand.Any())
                {
                    var json = JsonConvert.SerializeObject(listBrand);
                    _logger.Information(json);
                    return Ok(listBrand);
                }
                else
                {
                    return NotFound();
                }
            }
            catch
            {
                return BadRequest();
            }
        }


        [HttpGet]
        [Route("api/[controller]/GetBrandByName")]
        public async Task<IActionResult> GetBrandByName(string brandName)
        {
            try
            {
                _logger.Information("Get Brand By Name: ");
                var brand = await _brandServices.getSingleBrandAsync(brandName);
                if (brand == null)
                {
                    return NotFound();
                }
                var json = JsonConvert.SerializeObject(brandName);
                _logger.Information(json);
                return Ok(brand);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("api/[controller]/CreateNewBrand")]
        public async Task<IActionResult> CreateNewBrand(CreateBrandInfoModel brandModel)
        {
            try
            {
                _logger.Information("Create New Brand: ");
                if (_brandServices.getAllBrandAsync().Result.Where(x => x.BrandName == brandModel.BrandName).Any())
                {
                    return BadRequest("Brand Name is existed");
                }
                var checkCreate = await _brandServices.AddBrandAsync(brandModel);
                if (checkCreate == true)
                {
                    var json = JsonConvert.SerializeObject(brandModel);
                    _logger.Information(json);
                    return Ok("Created Successfully");
                }
                else return BadRequest("Created Fail");
            }
            catch
            {
                return BadRequest();
            }
        }


        [HttpDelete]
        [Route("api/[controller]/DeleteBrand")]
        public async Task<IActionResult> DeleteBrand(int id)
        {
            try
            {
                _logger.Information("DeleteBrand Brand: ");
                var existedBrand = _brandServices.getAllBrandAsync().Result.Where(x => x.BrandId == id).SingleOrDefault();
                if (existedBrand == null)
                {
                    return NotFound();
                }
                var checkDelete = await _brandServices.DeleteBrandAsync(existedBrand.BrandId);
                if (checkDelete == true)
                {
                    var json = JsonConvert.SerializeObject(id);
                    _logger.Information(json);
                    return Ok("Deleted Successfully");
                }
                else
                {
                    return BadRequest("Deleted Faild");
                }
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPut]
        [Route("api/[controller]/UpdateBrand")]
        public async Task<IActionResult> UpdateBrand(int id, [FromBody] CreateBrandInfoModel brandModel)
        {
            try
            {
                _logger.Information("Update Brand: ");
                var brandExisted = _brandServices.getAllBrandAsync().Result.ToList();
                if (!brandExisted.Where(x => x.BrandId == id).Any())
                {
                    return NotFound();
                }
                if (brandExisted.Where(x => x.BrandId == id).SingleOrDefault()!.BrandName != brandModel.BrandName)
                {
                    if (brandExisted.Where(x => x.BrandName == brandModel.BrandName).Any())
                    {
                        return BadRequest("Car Name is existed");
                    }
                }
                var checkUpdate = await _brandServices.UpdateBrandAsync(id, brandModel);
                if (checkUpdate == true)
                {
                    var json = JsonConvert.SerializeObject(brandModel);
                    _logger.Information(json);
                    return Ok("Updated Successfully");
                }
                else return BadRequest("Updated Fail");
            }
            catch
            {
                return BadRequest();
            }
        }

    }
}
