using URLS.MVC.Infrastructure.Exceptions;
using URLS.MVC.Models;

namespace URLS.MVC.Infrastructure.Extensions
{
    public static class HttpClientExtensions
    {
        public static async Task<T> DeleteFromJsonAsync<T>(this HttpClient httpClient, string url)
        {
            var response = await httpClient.DeleteAsync(url);
            response.CheckResponse();
            return await response.Content.ReadFromJsonAsync<T>();
        }

        public static async Task<T> PostFromJsonAsync<T>(this HttpClient httpClient, string uri, object body)
        {
            var response = await httpClient.PostAsync(uri, body.GetHttpContent());
            response.CheckResponse();
            return await response.Content.ReadFromJsonAsync<T>();
        }

        public static async Task<T> GetFromJsonAsync<T>(this HttpClient httpClient, string url)
        {
            var response = await httpClient.GetAsync(url);
            response.CheckResponse();
            return await response.Content.ReadFromJsonAsync<T>();
        }

        public static async Task<T> PutFromJsonAsync<T>(this HttpClient httpClient, string url, object body)
        {
            var response = await httpClient.PutAsync(url, body.GetHttpContent());
            response.CheckResponse();
            return await response.Content.ReadFromJsonAsync<T>();
        }

        private static void CheckResponse(this HttpResponseMessage httpResponse)
        {
            if (!httpResponse.IsSuccessStatusCode)
            {
                var resposne = httpResponse.Content.ReadFromJsonAsync<Result<Dictionary<string, string[]>>>().Result;

                if (httpResponse.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    throw new UnauthorizedException(resposne.GetError());
                throw new Exception(resposne.GetError());
            }
        }
    }
}
