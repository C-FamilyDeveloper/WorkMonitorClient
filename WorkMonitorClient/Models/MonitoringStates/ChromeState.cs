using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation;
using WorkMonitorTypes.Requests;

namespace WorkMonitorClient.Models.MonitoringStates
{
    public class ChromeState : BrowserState
    {
        public override string GetURL()
        {
            AutomationElement url = Element.FindFirst(TreeScope.Descendants, new PropertyCondition
                                                    (AutomationElement.NameProperty, @"Адресная строка и строка поиска"));
            if (url != null)
            {
                return url.GetCurrentPropertyValue(ValuePatternIdentifiers.ValueProperty).ToString()??string.Empty;
            }
            else
            {
                return string.Empty;
            }
        }
        public override MonitorObject GetMonitoringInfo()
        {            
            return new MonitorObject { Application = GetProcessName(), URL = GetURL() };
        }
    }
}
