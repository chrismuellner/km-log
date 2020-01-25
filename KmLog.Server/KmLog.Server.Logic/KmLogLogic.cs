using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KmLog.Server.Dal;
using KmLog.Server.DTO;
using KmLog.Server.Model;
using Microsoft.Extensions.Logging;

namespace KmLog.Server.Logic
{
    public class KmLogLogic
    {
        private readonly ILogger<KmLogLogic> _logger;
        private readonly IJourneyRepository _journeyRepository;

        public KmLogLogic(ILogger<KmLogLogic> logger, IJourneyRepository journeyRepository)
        {
            _logger = logger;
            _journeyRepository = journeyRepository;
        }

        public async Task<JourneyDTO> Add(JourneyDTO journey)
        {
            try
            {
                await _journeyRepository.Add(journey);
                return journey;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding new journey");
                throw;
            }
        }

        public async Task<IEnumerable<JourneyDTO>> LoadAll()
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
