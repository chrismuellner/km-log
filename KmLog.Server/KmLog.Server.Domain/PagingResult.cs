using System.Collections.Generic;

namespace KmLog.Server.Domain
{
    public class PagingResult<T>
    {
        public int Count { get; set; }
        public IEnumerable<T> Items { get; set; }
    }
}
