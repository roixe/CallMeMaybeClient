using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;


namespace CallMeMaybeClient
{
    internal class ApiLink
    {
        private HttpClient _httpClient;

        public ApiLink()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(System.Configuration.ConfigurationManager.AppSettings["ApiBaseUrl"])
            };
        }

        public async Task<T> GetAsync<T>(string endpoint)
        {
            var response = await _httpClient.GetAsync(endpoint);
            response.EnsureSuccessStatusCode();
            var jsonResponse = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(jsonResponse);
        }


        public async Task<T> PostAsync<T>(string endpoint, object payload)
        {
            var jsonPayload = JsonSerializer.Serialize(payload);
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(endpoint, content);
            response.EnsureSuccessStatusCode();
            var jsonResponse = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(jsonResponse);
        }

        public async Task<bool> PingAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("");
                response.EnsureSuccessStatusCode(); // Throw if HTTP response is an error
                Console.WriteLine("Code 200");
                return true;
            }
            catch
            {
                return false;
            }
        }
    }

}

