using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using KmLog.Server.Domain;
using Microsoft.AspNetCore.Components;

namespace KmLog.Server.Blazor.Shared
{
    public abstract class EntryListBase<T> : ComponentBase
        where T : class
    {
        [Inject]
        protected HttpClient HttpClient { get; set; }

        [Parameter]
        public string LicensePlate { get; set; }

        protected PagingParameters PagingParams { get; } = new PagingParameters { PageIndex = 0, PageSize = 10 };

        protected abstract PagingResult<T> Entries { get; set; }

        protected abstract string GetPath { get; }

        protected override async Task OnParametersSetAsync()
        {
            try
            {
                Entries = await HttpClient.GetFromJsonAsync<PagingResult<T>>(GetPath);
            }
            catch (Exception)
            {
                Debug.WriteLine("Could not parse json for refuel entries!");
            }
        }

        protected async Task PagingChanged(int page)
        {
            try
            {
                PagingParams.PageIndex = page;
                Entries = await HttpClient.GetFromJsonAsync<PagingResult<T>>(
                    $"{GetPath}?PageIndex={PagingParams.PageIndex}&PageSize={PagingParams.PageSize}");
            }
            catch (Exception)
            {
                Debug.WriteLine("Could not load refuel entries");
            }
        }
    }
}
