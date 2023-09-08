using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;
using WorkMonitorClient.Models.Abstractions;
using WorkMonitorClient.Models.MonitoringStates;
using WorkMonitorTypes.Requests;

namespace WorkMonitorClient.Models
{
    public class MonitoringContext 
    {
        private IMonitoringState monitoringState;

        public AutomationElement AutomationElement { get; set; }
        public MonitoringContext()
        {
            monitoringState = new ApplicationState();
        }

        public MonitorObject GetMonitoringResult()
        {
            monitoringState.CheckState(this);
            return monitoringState.GetMonitoringInfo();
        }
        public void SetState(IMonitoringState state)
        {
            monitoringState = state;
            state.Element = AutomationElement; 
        }
    }
}
