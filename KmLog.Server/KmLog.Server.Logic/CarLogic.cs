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
    public class CarLogic
    {
        private readonly ILogger<CarLogic> _logger;
        private readonly ICarRepository _carRepository;
        private readonly IRefuelActionRepository _refuelActionRepository;
        private readonly IUserRepository _userRepository;

        public CarLogic(ILogger<CarLogic> logger, ICarRepository carRepository, IRefuelActionRepository refuelActionRepository,
                        IUserRepository userRepository)
        {
            _logger = logger;
            _carRepository = carRepository;
            _refuelActionRepository = refuelActionRepository;
            _userRepository = userRepository;
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

        public async Task<IEnumerable<CarDto>> LoadByUser(string email)
        {
            try
            {
                var user = await _userRepository.LoadByEmail(email);
                if (user == null)
                {
                    return null;
                }

                var cars = await _carRepository.LoadByUser(user.Id);
                return cars;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading all cars by user");
                throw;
            }
        }

        public async Task<IEnumerable<RefuelActionDto>> ImportCsv(string email, Stream fileStream, string fileName)
        {
            try
            {
                var user = await _userRepository.LoadByEmail(email);
                if (user == null)
                {
                    return null;
                }

                var licensePlate = fileName.Split("_").First();
                var car = new CarDto
                {
                    UserId = user.Id,
                    LicensePlate = licensePlate
                };
                await _carRepository.Add(car);

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
            } catch (Exception ex)
            {
                _logger.LogError(ex, "Error importing file");
                throw;
            }
        }
    }
}
