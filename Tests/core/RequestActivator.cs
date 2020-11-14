using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Weber.Core.Controllers;

namespace Tests.core
{
    public class RequestActivator
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        
        public static async Task<object> SendRequest(RequestType type, string query, byte[] data, string path)
        {
            const string host = "http://localhost:3000/";
            switch (type)
            {
                case RequestType.None:
                    break;
                case RequestType.GET:
                {
                    var response = await _httpClient.GetAsync($"{host}{query}");
                    if (response.IsSuccessStatusCode)
                    {
                        return await response.Content.ReadAsStringAsync();
                    }
                } break;
                case RequestType.POST:
                {
                    var content = new ByteArrayContent(data);
                    content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
                    var response = await _httpClient.PostAsync(new Uri($"{host}{query}"), content);
                    if (response.IsSuccessStatusCode)
                    {
                        return await response.Content.ReadAsStringAsync();
                    }
                } break;
                case RequestType.PUT:
                {
                    var content = new ByteArrayContent(data);
                    content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
                    var response = await _httpClient.PutAsync(new Uri($"{host}{query}"), content);
                    if (response.IsSuccessStatusCode)
                    {
                        return await response.Content.ReadAsStringAsync();
                    }
                } break;
                case RequestType.DELETE:
                {
                    var response = await _httpClient.DeleteAsync($"{host}{query}");
                    if (response.IsSuccessStatusCode)
                    {
                        return await response.Content.ReadAsStringAsync();
                    }
                } break;
            }
            return string.Empty;
        }
    }
}
