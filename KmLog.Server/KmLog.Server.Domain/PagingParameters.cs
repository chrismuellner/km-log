namespace KmLog.Server.Domain
{
    public class PagingParameters
    {
        public int PageIndex { get; set; } = 0;
        public int PageSize { get; set; } = 10;
        public int ItemsToSkip => PageIndex * PageSize;
    }
}
