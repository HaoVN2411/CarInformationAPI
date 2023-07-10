using CarCategoriesApi.Models;
using CarCategoriesApi.Repositories;
using CarCategoriesApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Serilog;

namespace CarCategoriesApi.Controllers
{
    [ApiController]
    public class CarsController : ControllerBase
    {
        private readonly Serilog.ILogger _logger;
        private readonly ICarService _carServices;
        private readonly IBrandService _brandServices;

        public CarsController(ICarService carService, IBrandService brandService)
        {
            _carServices = carService;
            _brandServices = brandService;
            _logger = Log.Logger;
        }
        [Authorize]
        [HttpGet]
        [Route("api/[controller]/GetAllCars")]
        public async Task<IActionResult> GetAllCars()
        {
            try
            {
                _logger.Information("Get All Car: ");
                var listCar = await _carServices.getAllCarAsync();
                if (listCar.Any())
                {
                    var json = JsonConvert.SerializeObject(listCar);
                    _logger.Information(json);
                    return Ok(listCar);
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
        [Route("api/[controller]/GetCarByName")]
        public async Task<IActionResult> GetCarByName(string carName)
        {
            try
            {
                _logger.Information("Get Car By Name: ");
                var car = await _carServices.getCarAsync(carName);
                if (car == null)
                {
                    return NotFound();
                }
                var json = JsonConvert.SerializeObject(carName);
                _logger.Information(json);
                return Ok(car);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("api/[controller]/CreateNewCar")]
        public async Task<IActionResult> addNewCar(CreateCarRequestModel carModel)
        {
            try
            {
                _logger.Information("Create New Car: ");
                if (_carServices.getAllCarAsync().Result.Where(x => x.Name == carModel.Name).Any())
                {
                    return BadRequest("Car Name is existed");
                }
                var checkCreate = await _carServices.AddCarAsync(carModel);
                if (checkCreate == true)
                {
                    var json = JsonConvert.SerializeObject(carModel);
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

        [HttpPut]
        [Route("api/[controller]/UpdateCar")]
        public async Task<IActionResult> UpdateCar(int id, [FromBody] CreateCarRequestModel carModel)
        {
            try
            {
                _logger.Information("Update Car: ");
                var carExisted = _carServices.getAllCarAsync().Result.ToList();
                if (!carExisted.Where(x => x.Id == id).Any())
                {
                    return NotFound();
                }
                if (carExisted.Where(x => x.Id == id).SingleOrDefault()!.Name != carModel.Name)
                {
                    if (carExisted.Where(x => x.Name == carModel.Name).Any())
                    {
                        return BadRequest("Car Name is existed");
                    }
                }
                var checkUpdate = await _carServices.UpdateCarAsync(id, carModel);
                if (checkUpdate == true)
                {
                    var json = JsonConvert.SerializeObject(carModel);
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

        [HttpDelete]
        [Route("api/[controller]/DeleteCar")]
        public async Task<IActionResult> DeleteCar(int id)
        {
            try
            {
                _logger.Information("Delete Car: ");
                var existedCar = _carServices.getAllCarAsync().Result.Where(x => x.Id == id).SingleOrDefault();
                if (existedCar == null)
                {
                    return NotFound();
                }
                var checkDelete = await _carServices.DeleteCarAsync(existedCar.Id);
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

        [HttpGet]
        [Route("api/[controller]/GetCarFromBrandName")]
        public async Task<IActionResult> GetCarFromBrandName (string brandName)
        {
            try
            {
                var brandInfo = _brandServices.getAllBrandAsync().Result.Where(x => x.BrandName.Contains(brandName)).ToList();
                if (!brandInfo.Any())
                {
                    return NotFound();
                }
                var carListFromBrand = await _carServices.getCarFromBrandAsync(brandInfo);
                var json = JsonConvert.SerializeObject(brandName);
                _logger.Information(json);
                return Ok(carListFromBrand);
            } catch
            {
                return BadRequest(); 
            }
        }

    }
}
