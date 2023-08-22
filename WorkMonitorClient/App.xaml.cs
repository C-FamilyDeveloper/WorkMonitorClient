using Microsoft.Extensions.DependencyInjection;
using MVVMUtilities.Abstractions;
using MVVMUtilities.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using WorkMonitorClient.ViewModels;
using WorkMonitorClient.Views;

namespace WorkMonitorClient
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IServiceProvider serviceProvider;

        public App()
        {
            ServiceCollection services = new();
            ConfigureServices(services);
            serviceProvider = services.BuildServiceProvider();
            if (!serviceProvider.GetRequiredService<ICheckInstanceService>().IsOneApplicationInstance())
            {
                serviceProvider.GetRequiredService<IDialogService>().ShowWarningMessage(
                    "Work Monitor is already running." + Environment.NewLine + Environment.NewLine
                        + "You can only run one instance at a time.", "Work Monitor Error");
                Process.GetCurrentProcess().Kill();
                Environment.Exit(0);
            }
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<ICheckInstanceService, CheckInstanceService>();
            services.AddSingleton<IDialogService, DialogService>();
            services.AddSingleton<INavigationService, NavigationService>(serviceProvider =>
            {
                var navigationService = new NavigationService(serviceProvider);
                //navigationService.Initialize(serviceProvider);
                navigationService.ConfigureWindow<MainViewModel, MainWindow>();
                return navigationService;
            });
            services.AddScoped<MainViewModel>();
        }

        private void OnStartup(object sender, StartupEventArgs e)
        {
            var service = serviceProvider.GetRequiredService<INavigationService>();
            service.Show<MainViewModel>();
        }
       
    }
}
