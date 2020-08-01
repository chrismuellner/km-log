using KmLog.Server.Domain;
using KmLog.Server.Dto;

namespace KmLog.Server.Blazor.Shared
{
    public partial class RefuelEntryList : EntryListBase<RefuelEntryInfoDto>
    {
        protected override PagingResult<RefuelEntryInfoDto> Entries { get; set; }

        protected override string GetPath => $"api/entry/refuel/{LicensePlate}";
    }
}
