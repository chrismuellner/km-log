using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using KmLog.Server.Dto;

namespace KmLog.Server.Blazor.Services
{
    public class AppState
    {
        private readonly HttpClient _httpClient;

        public IEnumerable<CarInfoDto> Cars { get; set; }

        public event Action OnCarsChanged;

        public AppState(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task UpdateCars()
        {
            Cars = await _httpClient.GetFromJsonAsync<IEnumerable<CarInfoDto>>("api/car");
            OnCarsChanged?.Invoke();
        }
    }
}
