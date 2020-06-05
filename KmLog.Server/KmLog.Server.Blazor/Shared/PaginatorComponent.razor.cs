using System.Threading.Tasks;
using KmLog.Server.Domain;
using Microsoft.AspNetCore.Components;

namespace KmLog.Server.Blazor.Shared
{
    public partial class PaginatorComponent<T>
    {
        [Parameter]
        public PagingParameters PagingParams { get; set; }

        [Parameter]
        public PagingResult<T> PagingResult { get; set; }

        [Parameter]
        public EventCallback<int> OnPagingChanged { get; set; }

        private int TotalPages => PagingResult != null
            ? PagingResult.Count % PagingParams.PageSize > 0
                ? (PagingResult.Count / PagingParams.PageSize) + 1
                : PagingResult.Count / PagingParams.PageSize
            : 0;

        private int CurrentPage => PagingParams.PageIndex + 1;

        private async Task PagingChanged(int page)
        {
            await OnPagingChanged.InvokeAsync(page);
        }
    }
}
