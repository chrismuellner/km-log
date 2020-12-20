using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KmLog.Server.Blazor.Shared
{
    public interface IErrorComponent
    {
        void ShowError(string message, Exception e);
    }
}
