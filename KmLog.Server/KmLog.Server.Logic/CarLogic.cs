using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Authentication;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
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

                if (user.GroupId.HasValue)
                {
                    return await _unitOfWork.CarRepository.LoadByGroup(user.GroupId.Value);
                }

                return await _unitOfWork.CarRepository.LoadByUser(user.Id);
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

        public async Task<IEnumerable<RefuelEntryDto>> ImportCsv(string email, Stream fileStream, IDictionary<string, string> formDict)
        {
            try
            {
                using var transaction = _unitOfWork.BeginTransaction();

                var user = await CheckUser(email);

                // load or add car
                var licensePlate = formDict[nameof(CarDto.LicensePlate)];
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

                // read csv
                using var reader = new StreamReader(fileStream);
                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    Delimiter = ";",
                    BadDataFound = null,
                    MissingFieldFound = null
                };

                var indexes = formDict
                    .Where(d => d.Key != nameof(CarDto.LicensePlate))
                    .ToDictionary(d => d.Key, d => int.Parse(d.Value));

                using var csv = new CsvReader(reader, config);
                csv.Read();
                csv.ReadHeader();
                while (csv.Read())
                {
                    var refuelEntry = new RefuelEntryDto
                    {
                        Date = DateTime.ParseExact( // todo: datetime format as parameter
                            csv.GetField<string>(indexes[nameof(RefuelEntryDto.Date)]), "yyyy_MM_dd", CultureInfo.InvariantCulture, DateTimeStyles.None),
                        Amount = double.Parse(
                            csv.GetField<string>(indexes[nameof(RefuelEntryDto.Amount)])),
                        Cost = double.Parse(
                            csv.GetField<string>(indexes[nameof(RefuelEntryDto.Cost)])),
                        TotalDistance = long.Parse(
                            csv.GetField<string>(indexes[nameof(RefuelEntryDto.TotalDistance)]), NumberStyles.Any),
                        PricePerLiter = double.Parse(
                            csv.GetField<string>(indexes[nameof(RefuelEntryDto.PricePerLiter)])),
                        TankStatus = csv.GetField<string>(indexes[nameof(RefuelEntryDto.TankStatus)]) switch
                        {
                            "tank full" => TankStatus.Full,
                            "tank partial" => TankStatus.Partial,
                            _ => throw new InvalidOperationException("Invalid tank status!")
                        },
                        CarId = car.Id
                    };

                    refuelEntries.Add(refuelEntry);
                }

                // calculate distances
                long previousTotalDistance = 0;
                var orderedEntries = refuelEntries
                    .OrderByDescending(e => e.Date)
                    .ToArray();
                for (int i = orderedEntries.Count() - 1; i >= 0; i--)
                {
                    var refuelEntry = orderedEntries[i];

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
