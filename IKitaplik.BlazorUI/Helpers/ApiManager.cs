using IKitaplik.BlazorUI.Responses;
using System.Net.Http.Headers;
using System.Text.Json;

namespace IKitaplik.BlazorUI.Helpers
{
    public class ApiManager<T>
    {
        private readonly HttpClient _httpClient;
        private readonly JwtAuthenticationStateProvider _jwtAuthenticationStateProvider;
        public ApiManager(HttpClient httpClient, JwtAuthenticationStateProvider jwtAuthenticationStateProvider)
        {
            _httpClient = httpClient;
            _jwtAuthenticationStateProvider = jwtAuthenticationStateProvider;
        }

        public async Task<List<T>> GetAllListAsync(string request)
        {
            var httpRequest = new HttpRequestMessage(HttpMethod.Get, request);
            string token = await _jwtAuthenticationStateProvider.GetToken() ?? "";
            if (!string.IsNullOrEmpty(token))
                httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.SendAsync(httpRequest);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<Response<List<T>>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return result!.Data;
        }
    }
}