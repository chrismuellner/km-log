using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KmLog.Server.Dal;
using KmLog.Server.Dto;
using Microsoft.Extensions.Logging;

namespace KmLog.Server.Logic
{
    public class FuelActionLogic
    {
        private readonly ILogger<FuelActionLogic> _logger;
        private readonly IJourneyRepository _journeyRepository;

        public FuelActionLogic(ILogger<FuelActionLogic> logger, IJourneyRepository journeyRepository)
        {
            _logger = logger;
            _journeyRepository = journeyRepository;
        }

        public async Task<RefuelActionDto> Add(RefuelActionDto refuelAction)
        {
            try
            {
                await _journeyRepository.Add(refuelAction);
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
                var journeys = await _journeyRepository.LoadAll();

                return journeys;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading all journeys");
                throw;
            }
        }
    }
}
