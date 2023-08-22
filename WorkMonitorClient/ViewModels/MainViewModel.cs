using MVVMUtilities.Abstractions;
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

        public RelayCommand StartMonitor { get; set; }
        private MonitoringService monitoringService = new(intervalMinMilliseconds: 10000, intervalMaxMilliseconds: 20000);
        private CancellationTokenSource cancellationTokenSource = new();
       
        public MainViewModel(IDialogService dialogService, INavigationService navigationService) 
        {
            this.dialogService = dialogService;
            this.navigationService = navigationService;
            StartMonitor = new(async () =>
            {
                navigationService.Hide<MainViewModel>();
                try
                {
                    await monitoringService.StartMonitoring(cancellationTokenSource.Token);
                }
                catch (Exception ex)
                {
                    dialogService.ShowErrorMessage(ex.Message, "Ошибка");
                }
            });
        }
    }
}
