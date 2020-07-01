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
    public class RefuelEntryLogic
    {
        private readonly ILogger<RefuelEntryLogic> _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public RefuelEntryLogic(ILogger<RefuelEntryLogic> logger, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<RefuelEntryDto> Add(RefuelEntryDto refuelEntry)
        {
            try
            {
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
                _logger.LogError(ex, "Error adding new action");
                throw;
            }
        }

        public async Task<RefuelEntryInfoDto> LoadLatest(string licensePlate)
        {
            try
            {
                using var transaction = _unitOfWork.BeginTransaction();

                var result = await _unitOfWork.RefuelEntryRepository.Query()
                    .OrderByDescending(re => re.Date)
                    .FirstOrDefaultAsync(re => re.Car.LicensePlate == licensePlate);

                return _mapper.Map<RefuelEntryInfoDto>(result);
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

        public async Task<PagingResult<RefuelEntryDto>> LoadPaged(PagingParameters pagingParameters, string licensePlate)
        {
            try
            {
                using var transaction = _unitOfWork.BeginTransaction();

                var query = _unitOfWork.RefuelEntryRepository.Query()
                    .Where(re => re.Car.LicensePlate == licensePlate);

                var result = await query
                    .OrderByDescending(re => re.Date)
                    .Skip(pagingParameters.ItemsToSkip)
                    .Take(pagingParameters.PageSize)
                    .ToListAsync();

                return new PagingResult<RefuelEntryDto>
                {
                    Count = await query.CountAsync(),
                    Items = _mapper.Map<IEnumerable<RefuelEntryDto>>(result)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading paged actions");
                throw;
            }
        }
    }
}
