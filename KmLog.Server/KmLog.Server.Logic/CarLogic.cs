using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KmLog.Server.Dal;
using KmLog.Server.Dto;
using Microsoft.Extensions.Logging;

namespace KmLog.Server.Logic
{
    public class CarLogic
    {
        private readonly ILogger<CarLogic> _logger;
        private readonly ICarRepository _carRepository;

        public CarLogic(ILogger<CarLogic> logger, ICarRepository carRepository)
        {
            _logger = logger;
            _carRepository = carRepository;
        }

        public async Task<CarDto> Add(CarDto car)
        {
            try
            {
                await _carRepository.Add(car);
                return car;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding new car");
                throw;
            }
        }

        public async Task<IEnumerable<CarDto>> LoadAll()
        {
            try
            {
                var cars = await _carRepository.LoadAll();
                return cars;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading all cars");
                throw;
            }
        }
    }
}
