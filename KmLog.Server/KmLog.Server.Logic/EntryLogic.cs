using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KmLog.Server.Dal;
using KmLog.Server.Domain;
using KmLog.Server.Dto;
using KmLog.Server.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace KmLog.Server.Logic
{
    public class EntryLogic
    {
        private readonly ILogger<EntryLogic> _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public EntryLogic(ILogger<EntryLogic> logger, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<RefuelEntryDto> AddRefuel(RefuelEntryDto refuelEntry)
        {
            try
            {
                if (refuelEntry.CarId == Guid.Empty
                 || refuelEntry.Distance == 0
                 || refuelEntry.TotalDistance == 0)
                {
                    return null;
                }

                using var transaction = _unitOfWork.BeginTransaction();

                var entity = _mapper.Map<RefuelEntry>(refuelEntry);

                await _unitOfWork.RefuelEntryRepository.Add(entity);

                await _unitOfWork.Save();
                transaction.Commit();

                _mapper.Map(entity, refuelEntry);

                return refuelEntry;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding new refuel entry");
                throw;
            }
        }

        public async Task<ServiceEntryDto> AddService(ServiceEntryDto serviceEntry)
        {
            try
            {
                if (serviceEntry.CarId == Guid.Empty
                 || serviceEntry.TotalDistance == 0)
                {
                    return null;
                }

                using var transaction = _unitOfWork.BeginTransaction();

                var entity = _mapper.Map<ServiceEntry>(serviceEntry);
                await _unitOfWork.ServiceEntryRepository.Add(entity);

                await _unitOfWork.Save();
                transaction.Commit();

                _mapper.Map(entity, serviceEntry);

                return serviceEntry;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding new service entry");
                throw;
            }
        }

        public async Task<RefuelEntryInfoDto> LoadLatestRefuel(string licensePlate)
        {
            try
            {
                using var transaction = _unitOfWork.BeginTransaction();

                var query = _unitOfWork.RefuelEntryRepository.Query();
                return await LoadLatest<RefuelEntryInfoDto, RefuelEntry>(query, licensePlate);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading latest refuel entry");
                throw;
            }
        }

        public async Task<ServiceEntryInfoDto> LoadLatestService(string licensePlate)
        {
            try
            {
                using var transaction = _unitOfWork.BeginTransaction();

                var query = _unitOfWork.ServiceEntryRepository.Query();
                return await LoadLatest<ServiceEntryInfoDto, ServiceEntry>(query, licensePlate);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading latest service entry");
                throw;
            }
        }

        public async Task<IEnumerable<RefuelEntryInfoDto>> LoadRefuelsByCarLicensePlate(string licensePlate)
        {
            try
            {
                using var transaction = _unitOfWork.BeginTransaction();

                var refuelEntries = await _unitOfWork.RefuelEntryRepository.Query()
                    .Where(re => re.Car.LicensePlate == licensePlate)
                    .ToListAsync();

                return _mapper.Map<IEnumerable<RefuelEntryDto>>(refuelEntries);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading all actions by license plate");
                throw;
            }
        }

        public async Task<PagingResult<RefuelEntryDto>> LoadRefuelsPaged(PagingParameters pagingParameters, string licensePlate)
        {
            try
            {
                using var transaction = _unitOfWork.BeginTransaction();

                var query = _unitOfWork.RefuelEntryRepository.Query()
                    .Where(re => re.Car.LicensePlate == licensePlate);

                return await LoadPaged<RefuelEntryDto, RefuelEntry>(query, pagingParameters);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading paged actions");
                throw;
            }
        }

        public async Task<PagingResult<ServiceEntryDto>> LoadServicesPaged(PagingParameters pagingParameters, string licensePlate)
        {
            try
            {
                using var transaction = _unitOfWork.BeginTransaction();

                var query = _unitOfWork.ServiceEntryRepository.Query()
                    .Where(se => se.Car.LicensePlate == licensePlate);

                return await LoadPaged<ServiceEntryDto, ServiceEntry>(query, pagingParameters);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading paged actions");
                throw;
            }
        }

        private async Task<PagingResult<TDto>> LoadPaged<TDto, TEntity>(IQueryable<TEntity> query, PagingParameters pagingParameters)
            where TDto : EntryDto
            where TEntity : Entry
        {
            var result = await query
                .OrderByDescending(re => re.Date)
                .Skip(pagingParameters.ItemsToSkip)
                .Take(pagingParameters.PageSize)
                .ToListAsync();

            return new PagingResult<TDto>
            {
                Count = await query.CountAsync(),
                Items = _mapper.Map<IEnumerable<TDto>>(result)
            };
        }

        private async Task<TDto> LoadLatest<TDto, TEntity>(IQueryable<TEntity> query, string licensePlate)
            where TDto : EntryDto
            where TEntity : Entry
        {
            var result = await query
                .OrderByDescending(re => re.Date)
                    .FirstOrDefaultAsync(re => re.Car.LicensePlate == licensePlate);

            return _mapper.Map<TDto>(result);
        }
    }
}
