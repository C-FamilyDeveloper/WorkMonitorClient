using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation;
using WorkMonitorClient.Models.Abstractions;
using WorkMonitorClient.Models.Factories;
using WorkMonitorTypes.Requests;

namespace WorkMonitorClient.Models.MonitoringStates
{
    public class ApplicationState : IMonitoringState, IDesktopComponent
    {
        public AutomationElement Element { get ; set ; }

        public void CheckHandle(MonitoringContext monitoringContext)
        {
            Element = monitoringContext.AutomationElement;
            monitoringContext.SetState(MonitoringStateFactory.GetState(GetProcessName()));
        }
        
        public virtual MonitorObject GetMonitoringInfo()
        {
            return new MonitorObject { Application = GetProcessName() };
        }

        public string GetProcessName()
        {
            using Process p = Process.GetProcessById(Element.Current.ProcessId);
            return p.ProcessName;
        }
    }
}
