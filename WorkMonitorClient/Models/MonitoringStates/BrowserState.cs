using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkMonitorClient.Models.Abstractions;
using WorkMonitorTypes.Requests;

namespace WorkMonitorClient.Models.MonitoringStates
{
    public abstract class BrowserState :  ApplicationState, IMonitoringState, IWebComponent
    {
        public abstract string GetURL();
    }
}
