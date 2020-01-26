using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using KmLog.Server.Dal;
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

        public async Task<RefuelActionDto> Add(Guid carId, RefuelActionDto refuelAction)
        {
            try
            {
                refuelAction.CarId = carId;
                await _refuelActionRepository.Add(refuelAction);
                return refuelAction;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding new journey");
                throw;
            }
        }

        public async Task<IEnumerable<RefuelActionDto>> LoadAll()
        {
            try
            {
                var fuelActions = await _refuelActionRepository.LoadAll();
                return fuelActions;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading all journeys");
                throw;
            }
        }

        public async Task<IEnumerable<RefuelActionDto>> ImportCsv(Guid carId, Stream fileStream, string fileName)
        {
            var car = await _carRepository.GetById(carId);
            if (car == null)
            {
                var licensePlate = fileName.Split("_").First();
                car = new CarDto
                {
                    LicensePlate = licensePlate
                };
                await _carRepository.Add(car);
            }
            
            var refuelActions = new List<RefuelActionDto>();
            
            using var reader = new StreamReader(fileStream);
            while (!reader.EndOfStream)
            {
                var line = await reader.ReadLineAsync();
                var attr = line
                    .Split(";")
                    .Select(str => str.Replace("\"", ""))
                    .ToArray();

                var culture = CultureInfo.CreateSpecificCulture("de-AT");
                if (!DateTime.TryParseExact(attr[0], "yyyy_MM_dd", 
                        CultureInfo.InvariantCulture, DateTimeStyles.None, out var date) ||
                    !double.TryParse(attr[2], NumberStyles.Any, culture, out var totalDistance) ||
                    !double.TryParse(attr[3], NumberStyles.Any, culture, out var totalCost) ||
                    !double.TryParse(attr[4], NumberStyles.Any, culture, out var amount) ||
                    !double.TryParse(attr[5], NumberStyles.Any, culture, out var pricePerLiter))
                {
                    continue;
                }

                var refuelAction = new RefuelActionDto
                {
                    Date = date,
                    Amount = Math.Round(amount, 2),
                    Cost = Math.Round(totalCost, 2),
                    TotalDistance = Math.Round(totalDistance, 2),
                    CarId = car.Id
                };
                refuelActions.Add(refuelAction);
            }

            await _refuelActionRepository.Add(refuelActions);
            return refuelActions;
        }
    }
}
