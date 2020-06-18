using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Authentication;
using System.Threading.Tasks;
using KmLog.Server.Dal;
using KmLog.Server.Domain;
using KmLog.Server.Dto;
using Microsoft.Extensions.Logging;

namespace KmLog.Server.Logic
{
    public class CarLogic
    {
        private readonly ILogger<CarLogic> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public CarLogic(ILogger<CarLogic> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<CarDto> Add(CarDto car, string email)
        {
            try
            {
                using var transaction = _unitOfWork.BeginTransaction();
                var user = await CheckUser(email);

                car.UserId = user.Id;

                await _unitOfWork.CarRepository.Add(car);

                await _unitOfWork.Save();
                transaction.Commit();

                return car;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding new car");
                throw;
            }
        }

        public async Task<IEnumerable<CarInfoDto>> LoadByUser(string email)
        {
            try
            {
                using var transaction = _unitOfWork.BeginTransaction();
                var user = await CheckUser(email);

                var cars = await _unitOfWork.CarRepository.LoadByUser(user.Id);
                return cars;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading all cars by user");
                throw;
            }
        }

        public async Task<CarStatisticDto> LoadStatistics(string licensePlate)
        {
            try
            {
                using var transaction = _unitOfWork.BeginTransaction();

                var statistic = await _unitOfWork.CarRepository.LoadStatisticByLicensePlate(licensePlate);
                return statistic;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading car statistics");
                throw;
            }
        }

        public async Task<IEnumerable<RefuelEntryDto>> ImportCsv(string email, Stream fileStream, string fileName)
        {
            try
            {
                using var transaction = _unitOfWork.BeginTransaction();

                var user = await CheckUser(email);

                var licensePlate = fileName.Split("_").First();
                var car = await _unitOfWork.CarRepository.LoadByLicensePlate(licensePlate);
                if (car == null)
                {
                    car = new CarDto
                    {
                        UserId = user.Id,
                        LicensePlate = licensePlate
                    };
                    await _unitOfWork.CarRepository.Add(car);
                }

                var refuelEntries = new List<RefuelEntryDto>();

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
                        !long.TryParse(attr[2], NumberStyles.Any, culture, out var totalDistance) ||
                        !double.TryParse(attr[3], NumberStyles.Any, culture, out var totalCost) ||
                        !double.TryParse(attr[4], NumberStyles.Any, culture, out var amount) ||
                        !double.TryParse(attr[5], NumberStyles.Any, culture, out var pricePerLiter))
                    {
                        continue;
                    }

                    var refuelEntry = new RefuelEntryDto
                    {
                        Date = date,
                        Amount = amount,
                        Cost = totalCost,
                        TotalDistance = totalDistance,
                        PricePerLiter = pricePerLiter,
                        TankStatus = attr[7] switch
                        {
                            "tank full" => TankStatus.Full,
                            "tank partial" => TankStatus.Partial,
                            _ => throw new InvalidOperationException("Invalid tank status!")
                        },
                        CarId = car.Id
                    };

                    refuelEntries.Add(refuelEntry);
                }

                long previousTotalDistance = 0;
                for (int i = refuelEntries.Count - 1; i >= 0; i--)
                {
                    var refuelEntry = refuelEntries[i];

                    long distance = 0;
                    if (previousTotalDistance > 0)
                    {
                        distance = refuelEntry.TotalDistance - previousTotalDistance;
                    }
                    previousTotalDistance = refuelEntry.TotalDistance;

                    refuelEntry.Distance = distance > 0 ? distance : 0; // todo: mark as invalid
                }

                await _unitOfWork.RefuelEntryRepository.Add(refuelEntries);

                await _unitOfWork.Save();
                transaction.Commit();
                return refuelEntries;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error importing file");
                throw;
            }
        }

        private async Task<UserDto> CheckUser(string email)
        {
            var user = await _unitOfWork.UserRepository.LoadByEmail(email);
            return user ?? throw new AuthenticationException("Unknown user");
        }
    }
}
