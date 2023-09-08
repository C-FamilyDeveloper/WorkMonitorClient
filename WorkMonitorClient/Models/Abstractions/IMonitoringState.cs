using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;
using WorkMonitorTypes.Requests;

namespace WorkMonitorClient.Models.Abstractions
{
    public interface IMonitoringState
    {
        public AutomationElement Element { get; set; }
        void CheckState (MonitoringContext monitoringContext);
        MonitorObject GetMonitoringInfo();
    }
}
