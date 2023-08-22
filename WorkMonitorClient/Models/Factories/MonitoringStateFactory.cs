using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkMonitorClient.Models.Abstractions;
using WorkMonitorClient.Models.MonitoringStates;

namespace WorkMonitorClient.Models.Factories
{
    public class MonitoringStateFactory
    {
        public  static IMonitoringState GetState(string processName)
        {
            return processName switch
            {
                "chrome" => new ChromeState(),
                _ => new ApplicationState(),
            };
        }
    }
}
