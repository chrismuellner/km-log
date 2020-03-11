using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KmLog.Server.Dal;
using KmLog.Server.Domain;
using KmLog.Server.Dto;
using Microsoft.Extensions.Logging;

namespace KmLog.Server.Logic
{
    public class RefuelEntryLogic
    {
        private readonly ILogger<RefuelEntryLogic> _logger;
        private readonly IRefuelEntryRepository _refuelEntryRepository;

        public RefuelEntryLogic(ILogger<RefuelEntryLogic> logger, IRefuelEntryRepository refuelEntryRepository)
        {
            _logger = logger;
            _refuelEntryRepository = refuelEntryRepository;
        }

        public async Task<RefuelEntryDto> Add(RefuelEntryDto refuelEntry)
        {
            try
            {
                await _refuelEntryRepository.Add(refuelEntry);
                return refuelEntry;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding new action");
                throw;
            }
        }

        public async Task<RefuelEntryInfoDto> LoadLatest(string licensePlate)
        {
            try
            {
                var result = await _refuelEntryRepository.LoadLatest(licensePlate);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading latest entry");
                throw;
            }
        }

        public async Task<IEnumerable<RefuelEntryInfoDto>> LoadByCarLicensePlate(string licensePlate)
        {
            try
            {
                var refuelEntries = await _refuelEntryRepository.LoadByCarLicensePlate(licensePlate);
                return refuelEntries;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading all actions by license plate");
                throw;
            }
        }

        public async Task<PagingResult<RefuelEntryDto>> LoadPaged(PagingParameters pagingParameters, string licensePlate)
        {
            try
            {
                var result = await _refuelEntryRepository.LoadPaged(pagingParameters, r => r.Date, r => r.Car.LicensePlate == licensePlate);
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
