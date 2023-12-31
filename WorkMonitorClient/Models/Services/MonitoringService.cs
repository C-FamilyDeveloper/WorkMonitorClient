﻿using System.Timers;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System;
using WorkMonitorClient.Models.PInvoke;
using System.Windows.Automation;
using WorkMonitorTypes.Requests;

namespace WorkMonitorClient.Models.Services
{
    public class MonitoringService
    {
        private System.Timers.Timer timerScreen = new ();
        private ScreenshotService screenshotService = new();
        private HookService hookService = new();
        private AutomationElement element;
        private MonitoringContext monitoringContext = new();
        private IntPtr currentHWND = IntPtr.Zero;
        public string UserName { get; set; }
        private int intervalMin;
        private int intervalMax;
        private readonly HttpService httpService;
        private readonly LoggingService loggingService;

        public MonitoringService(HttpService httpService, LoggingService loggingService)
        {
            UserName = Environment.UserName;
            this.httpService = httpService;
            this.loggingService = loggingService;
        }
        public void SetInterval(int intervalMinMilliseconds, int intervalMaxMilliseconds)
        {
            intervalMin = intervalMinMilliseconds;
            intervalMax = intervalMaxMilliseconds;
            timerScreen.Interval = CalculateTimerInterval();
            timerScreen.AutoReset = true;
            timerScreen.Elapsed += OnTimerElapsed;
        }
        private int CalculateTimerInterval()
        {
            return new Random().Next(intervalMin, intervalMax);
        }
        private async void OnTimerElapsed(object? sender, ElapsedEventArgs e)
        {
            timerScreen.Stop();
            try
            {
                await httpService.Send(new Screenshot
                {
                    ClientName = UserName,
                    Image = screenshotService.GetScreenshot(),
                    ScreenshotDateTime = DateTime.Now
                });
            }
            catch (Exception ex)
            {
                loggingService.SaveLog(ex.Message);
            }
            timerScreen.Interval = CalculateTimerInterval(); 
            timerScreen.Start();          
        }

        public IntPtr GetForegroundWindow()
        {
            return NativeMethods.GetForegroundWindow();
        }
        private string? GetActiveWindowTitle(IntPtr handle)
        {
            const int nChars = 256;
            StringBuilder Buff = new (nChars);
            if (NativeMethods.GetWindowText(handle, Buff, nChars) > 0)
            {
                return Buff.ToString();
            }
            return null;
        }
        public async Task StartMonitoring(CancellationToken cancellationToken)
        {
            hookService.Start();
            timerScreen.Start();
            await Task.Run(async() =>
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    try
                    {
                        IntPtr hwnd = NativeMethods.GetForegroundWindow();
                        if (currentHWND != hwnd && hwnd != IntPtr.Zero)
                        {
                            currentHWND = hwnd;
                            element = AutomationElement.FromHandle(hwnd);
                            if (element != null)
                            {
                                monitoringContext.AutomationElement = element;
                                var result = monitoringContext.GetMonitoringResult();
                                var timeresult = hookService.Restart();
                                await httpService.Send(
                                    new WorkerInfo
                                    {
                                        Application = result.Application,
                                        EventDateTime = DateTime.Now,
                                        Site = result.URL,
                                        IdleTime = timeresult.FullTime-timeresult.WorkTime,
                                        WorkTime = timeresult.WorkTime,
                                        Worker = UserName
                                    });
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        //await httpService.Send(new Log { ClientName = UserName, LogDateTime = DateTime.Now, LogMessage = ex.Message });
                        loggingService.SaveLog(ex.Message);
                    }
                }
            }, cancellationToken);                       
        }      
    }
}
