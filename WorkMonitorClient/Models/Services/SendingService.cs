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
            await httpClient.PostAsync(url, new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json"));
        }*/
        public  static async Task Send(WorkerInfo workerInfo) 
        {
            using HttpClient httpClient = new();
            await httpClient.PostAsync("https://localhost:7261/api/activities",
                new StringContent(JsonSerializer.Serialize(workerInfo),Encoding.UTF8,"application/json"));
        }
        public static async Task Send(Log log)
        {
            using HttpClient httpClient = new();
            await httpClient.PostAsync("https://localhost:7261/api/logs",
              new StringContent(JsonSerializer.Serialize(log), Encoding.UTF8, "application/json"));
        }
        /*public static async Task Send(byte [] image)
        {
            using HttpClient httpClient = new();
            ByteArrayContent content = new(image);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            await httpClient.PostAsync("https://localhost:7261/api/workerinfo", content);
        }*/
        public static async Task Send(Screenshot screenshot)
        {
            using HttpClient httpClient = new();
            await httpClient.PostAsync("https://localhost:7261/api/screenshots",
                new StringContent(JsonSerializer.Serialize(screenshot), Encoding.UTF8, "application/json"));
        }

    }
}
