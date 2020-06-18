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
        private readonly IUnitOfWork _unitOfWork;

        public RefuelEntryLogic(ILogger<RefuelEntryLogic> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<RefuelEntryDto> Add(RefuelEntryDto refuelEntry)
        {
            try
            {
                using var transaction = _unitOfWork.BeginTransaction();
                await _unitOfWork.RefuelEntryRepository.Add(refuelEntry);

                await _unitOfWork.Save();
                transaction.Commit();

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
                using var transaction = _unitOfWork.BeginTransaction();

                var result = await _unitOfWork.RefuelEntryRepository
                    .LoadLatest(licensePlate);
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
                using var transaction = _unitOfWork.BeginTransaction();

                var refuelEntries = await _unitOfWork.RefuelEntryRepository
                    .LoadByCarLicensePlate(licensePlate);
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
                using var transaction = _unitOfWork.BeginTransaction();

                var result = await _unitOfWork.RefuelEntryRepository
                    .LoadPaged(pagingParameters, r => r.Date, r => r.Car.LicensePlate == licensePlate);
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
