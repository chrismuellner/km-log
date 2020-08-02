using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Authentication;
using System.Threading.Tasks;
using AutoMapper;
using CsvHelper;
using CsvHelper.Configuration;
using KmLog.Server.Dal;
using KmLog.Server.Domain;
using KmLog.Server.Dto;
using KmLog.Server.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace KmLog.Server.Logic
{
    public class CarLogic
    {
        private readonly ILogger<CarLogic> _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public CarLogic(ILogger<CarLogic> logger, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<CarDto> Add(CarDto car, string email)
        {
            try
            {
                using var transaction = _unitOfWork.BeginTransaction();
                var user = await CheckUser(email);

                car.UserId = user.Id;

                var entity = _mapper.Map<Car>(car);
                await _unitOfWork.CarRepository.Add(entity);

                await _unitOfWork.Save();
                transaction.Commit();

                _mapper.Map(entity, car);

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

                var cars = await _unitOfWork.CarRepository.Query()
                    .Where(c => user.GroupId.HasValue ? c.User.GroupId == user.GroupId : c.UserId == user.Id)
                    .ToListAsync();

                return _mapper.Map<IEnumerable<CarDto>>(cars);
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

                var statistic = await _unitOfWork.RefuelEntryRepository.Query()
                    .Where(r => r.Car.LicensePlate == licensePlate)
                    .GroupBy(r => r.Car.LicensePlate, (k, g) => new CarStatisticDto
                    {
                        AvgCost = g.Average(r => r.Cost),
                        AvgDistance = g.Average(r => r.Distance),
                        TotalCost = g.Sum(r => r.Cost),
                        TotalDistance = g.Max(r => r.TotalDistance)
                    })
                    .FirstOrDefaultAsync();

                return statistic;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading car statistics");
                throw;
            }
        }

        public async Task<IEnumerable<EntryDto>> ImportCsv(string email, Stream fileStream, IDictionary<string, string> formDict, EntryType entryType)
        {
            try
            {
                using var transaction = _unitOfWork.BeginTransaction();

                var user = await CheckUser(email);

                // load or add car
                var licensePlate = formDict[nameof(CarDto.LicensePlate)];
                var car = await _unitOfWork.CarRepository.Query()
                    .FirstOrDefaultAsync(c => c.LicensePlate == licensePlate);
                if (car == null)
                {
                    car = new Car
                    {
                        UserId = user.Id,
                        LicensePlate = licensePlate
                    };
                    await _unitOfWork.CarRepository.Add(car);
                }

                var entries = new List<EntryDto>();

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
                    EntryDto entry = entryType switch
                    {
                        EntryType.Refuel => new RefuelEntryDto(),
                        EntryType.Service => new ServiceEntryDto(),
                        _ => throw new ArgumentException()
                    };

                    entry.Date = DateTime.ParseExact( // todo: datetime format as parameter
                            csv.GetField<string>(indexes[nameof(EntryDto.Date)]), "yyyy_MM_dd", CultureInfo.InvariantCulture, DateTimeStyles.None);
                    entry.Cost = double.Parse(
                            csv.GetField<string>(indexes[nameof(EntryDto.Cost)]));
                    entry.TotalDistance = long.Parse(
                            csv.GetField<string>(indexes[nameof(EntryDto.TotalDistance)]), NumberStyles.Any);

                    switch (entryType)
                    {
                        case EntryType.Refuel:
                            var refuelEntry = entry as RefuelEntryDto;
                            refuelEntry.Amount = double.Parse(
                                csv.GetField<string>(indexes[nameof(RefuelEntryDto.Amount)]));
                            refuelEntry.PricePerLiter = double.Parse(
                                csv.GetField<string>(indexes[nameof(RefuelEntryDto.PricePerLiter)]));
                            refuelEntry.TankStatus = csv.GetField<string>(indexes[nameof(RefuelEntryDto.TankStatus)]) switch
                                {
                                    "tank full" => TankStatus.Full,
                                    "tank partial" => TankStatus.Partial,
                                    _ => throw new InvalidOperationException("Invalid tank status")
                                };
                            refuelEntry.CarId = car.Id;
                            break;

                        case EntryType.Service:
                            var serviceEntry = entry as ServiceEntryDto;
                            serviceEntry.ServiceType = csv.GetField<string>(indexes[nameof(ServiceEntryDto.ServiceType)]) switch
                                {
                                    "Autowäsche" => ServiceType.CarWash,
                                    "Ölwechsel" => ServiceType.OilChange,
                                    "Inspektion" => ServiceType.Inspection,
                                    _ => ServiceType.Repairs
                                };
                            serviceEntry.CarId = car.Id;
                            break;
                    }
                    entries.Add(entry);
                }

                switch (entryType)
                {
                    case EntryType.Refuel:
                        // calculate distances
                        long previousTotalDistance = 0;
                        var orderedEntries = entries
                            .OrderByDescending(e => e.Date)
                            .Cast<RefuelEntryDto>()
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

                        var refuelEntries = _mapper.Map<IEnumerable<RefuelEntry>>(entries);
                        await _unitOfWork.RefuelEntryRepository.Add(refuelEntries);

                        break;

                    case EntryType.Service:
                        var serviceEntries = _mapper.Map<IEnumerable<ServiceEntry>>(entries);
                        await _unitOfWork.ServiceEntryRepository.Add(serviceEntries);

                        break;
                }

                await _unitOfWork.Save();
                transaction.Commit();

                return entries;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error importing file");
                throw;
            }
        }

        private async Task<UserDto> CheckUser(string email)
        {
            var user = await _unitOfWork.UserRepository.Query().FirstOrDefaultAsync(u => u.Email == email);
            return user != null 
                ? _mapper.Map<UserDto>(user) 
                : throw new AuthenticationException("Unknown user");
        }
    }
}
