using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using KmLog.Server.Blazor.Validation.Models;
using Microsoft.AspNetCore.Components;

namespace KmLog.Server.Blazor.Shared
{
    public partial class AddCarComponent
    {
        [Inject]
        private HttpClient HttpClient { get; set; }

        private CarModel Car { get; set; } = new CarModel();

        private async Task FormSubmitted()
        {
            try
            {
                await HttpClient.PutAsJsonAsync("api/car", Car);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.StackTrace);
            }
            Car = new CarModel();
        }
    }
}
