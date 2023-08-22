using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkMonitorClient.Models.Abstractions
{
    public interface IDesktopComponent
    {
        string GetProcessName();
    }
}
