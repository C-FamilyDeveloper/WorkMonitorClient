using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkMonitorClient.Models.Services
{
    public class LoggingService
    {
        public void SaveLog(string log)
        {
            File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(),DateTime.Now.ToString("yyyyMMddhhmmssffff")+".log"),log);
        }
    }
}
