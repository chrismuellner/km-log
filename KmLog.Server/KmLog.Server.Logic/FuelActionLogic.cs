using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using KmLog.Server.Dal;
using KmLog.Server.Domain;
using KmLog.Server.Dto;
using Microsoft.Extensions.Logging;

namespace KmLog.Server.Logic
{
    public class FuelActionLogic
    {
        private readonly ILogger<FuelActionLogic> _logger;
        private readonly IRefuelActionRepository _refuelActionRepository;
        private readonly ICarRepository _carRepository;

        public FuelActionLogic(ILogger<FuelActionLogic> logger, IRefuelActionRepository refuelActionRepository,
                               ICarRepository carRepository)
        {
            _logger = logger;
            _refuelActionRepository = refuelActionRepository;
            _carRepository = carRepository;
        }

        public async Task<RefuelActionDto> Add(RefuelActionDto refuelAction)
        {
            try
            {
                await _refuelActionRepository.Add(refuelAction);
                return refuelAction;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding new action");
                throw;
            }
        }

        public async Task<IEnumerable<RefuelActionInfoDto>> LoadByCarId(Guid carId)
        {
            try
            {
                var fuelActions = await _refuelActionRepository.LoadByCarId(carId);
                return fuelActions;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading all actions by car id");
                throw;
            }
        }

        public async Task<IEnumerable<RefuelActionInfoDto>> LoadByCarLicensePlate(string licensePlate)
        {
            try
            {
                var fuelActions = await _refuelActionRepository.LoadByCarLicensePlate(licensePlate);
                return fuelActions;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading all actions by license plate");
                throw;
            }
        }

        public async Task<PagingResult<RefuelActionDto>> LoadPaged(PagingParameters pagingParameters, string licensePlate)
        {
            try
            {
                var result = await _refuelActionRepository.LoadPaged(pagingParameters, r => r.Date, r => r.Car.LicensePlate == licensePlate);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading paged actions");
                throw;
            }
        }
    }
}
