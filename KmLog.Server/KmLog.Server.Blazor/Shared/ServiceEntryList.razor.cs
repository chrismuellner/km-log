using KmLog.Server.Domain;
using KmLog.Server.Dto;

namespace KmLog.Server.Blazor.Shared
{
    public partial class ServiceEntryList : EntryListBase<ServiceEntryInfoDto>
    {
        protected override PagingResult<ServiceEntryInfoDto> Entries { get; set; }

        protected override string GetPath => $"api/entry/service/{LicensePlate}";
    }
}
