using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkMonitorClient.Models
{
    public class MonitorTime
    {
        public TimeSpan WorkTime { get; set; }
        public TimeSpan FullTime { get; set; }
    }
}
