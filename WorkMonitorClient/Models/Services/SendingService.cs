using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Reflection.PortableExecutable;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows;
using WorkMonitorTypes.Requests;

namespace WorkMonitorClient.Models.Services
{
    public static class SendingService
    {
        /*public static async Task Send <T>(T data, string url)
        {
            using HttpClient httpClient = new();
            await httpClient.PostAsJsonAsync(url, data);
        }*/
        public static async Task Send(WorkerInfo workerInfo) 
        {
            using HttpClient httpClient = new();
            await httpClient.PostAsJsonAsync("https://localhost:7261/api/activities",workerInfo);
        }
        public static async Task Send(Log log)
        {
            using HttpClient httpClient = new();
            await httpClient.PostAsJsonAsync("https://localhost:7261/api/logs",log);
        }

        public static async Task Send(Screenshot screenshot)
        {
            using HttpClient httpClient = new();
            await httpClient.PostAsJsonAsync("https://localhost:7261/api/screenshots",screenshot);
        }

    }
}
