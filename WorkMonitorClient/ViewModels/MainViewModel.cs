﻿using MVVMUtilities.Abstractions;
using MVVMUtilities.Core;
using System;
using System.Threading;
using System.Windows;
using WorkMonitorClient.Models.Services;


namespace WorkMonitorClient.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly IDialogService dialogService;
        private readonly INavigationService navigationService;
        private MonitoringService monitoringService;
        private CancellationTokenSource cancellationTokenSource = new();
        public RelayCommand StartMonitor { get; set; }
       
        public MainViewModel(IDialogService dialogService, INavigationService navigationService, MonitoringService monitoringService) 
        {
            this.dialogService = dialogService;
            this.navigationService = navigationService;
            this.monitoringService = monitoringService;
            monitoringService.SetInterval(intervalMinMilliseconds:10000, intervalMaxMilliseconds:20000);
            StartMonitor = new(async () =>
            {
                navigationService.Hide<MainViewModel>();
                await monitoringService.StartMonitoring(cancellationTokenSource.Token);
            });
        }
    }
}
