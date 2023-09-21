using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using WorkMonitorTypes.Requests;

namespace WorkMonitorClient.Models.Services
{
    public class HttpService
    {
        private HttpClient httpClient;
        public HttpService()
        {
                httpClient = new();
        }
        /*public static async Task Send <T>(T data, string url)
        {
            await httpClient.PostAsJsonAsync(url, data);
        }*/
        public async Task Send(WorkerInfo workerInfo) 
        {
            await httpClient.PostAsJsonAsync("https://localhost:7261/api/activities",workerInfo);
        }
        public async Task Send(Log log)
        {
            await httpClient.PostAsJsonAsync("https://localhost:7261/api/logs",log);
        }

        public async Task Send(Screenshot screenshot)
        {
            await httpClient.PostAsJsonAsync("https://localhost:7261/api/screenshots",screenshot);
        }

    }
}
