using Core.Entities;
using IKitaplik.BlazorUI.Cosntant;
using IKitaplik.BlazorUI.Responses;
using IKitaplik.BlazorUI.Services.Abstract;
using IKitaplik.Entities.DTOs.BookDTOs;
using IKitaplik.Entities.DTOs.WriterDTOs;
using System.Net.Http.Headers;
using System.Text.Json;

namespace IKitaplik.BlazorUI.Services.Concrete
{
    public class WriterService : IWriterService
    {
        private readonly HttpClient _httpClient;
        private readonly JwtAuthenticationStateProvider _jwtAuthenticationStateProvider;
        private readonly JsonSerializerOptions _jsonOptions = new() { PropertyNameCaseInsensitive = true };
        public WriterService(HttpClient httpClient, JwtAuthenticationStateProvider jwt)
        {
            _httpClient = httpClient;
            _jwtAuthenticationStateProvider = jwt;
            _httpClient.BaseAddress = new Uri(Settings.apiUrl);
        }

        public async Task<Response> AddAsync(WriterAddDto writerAddDto)
        {
            string token = await _jwtAuthenticationStateProvider.GetToken() ?? "";
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var res = await _httpClient.PostAsJsonAsync("writer/add", writerAddDto);
                var content = await res.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Response>(content, _jsonOptions)!;
            }
            else
            {
                return await Task.FromException<Response>(new Exception("Login Olunamadı"));
            }
        }

        public async Task<Response> DeleteAsync(int id)
        {
            string token = await _jwtAuthenticationStateProvider.GetToken() ?? "";
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var res = await _httpClient.PostAsJsonAsync("writer/delete", id);
                var content = await res.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Response>(content, _jsonOptions)!;
            }
            else
            {
                return await Task.FromException<Response>(new Exception("Login Olunamadı"));
            }
        }

        public async Task<Response<List<WriterGetDto>>> GetAllAsync()
        {
            string token = await _jwtAuthenticationStateProvider.GetToken();
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var res = await _httpClient.GetAsync("writer/getall");
                var content = await res.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Response<List<WriterGetDto>>>(content, _jsonOptions)!;
            }
            else
            {
                return await Task.FromException<Response<List<WriterGetDto>>>(new Exception("Login Olunamadı"));
            }
        }

        public async Task<Response<WriterGetDto>> GetAsync(int id)
        {
            string token = await _jwtAuthenticationStateProvider.GetToken();
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await _httpClient.GetAsync($"writer/getById/{id}");
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<Response<WriterGetDto>>(_jsonOptions)!;
            }
            else
            {
                return await Task.FromException<Response<WriterGetDto>>(new Exception("Login Olunamadı"));
            }
        }

        public async Task<Response> UpdateAsync(WriterUpdateDto writerUpdateDto)
        {
            string token = await _jwtAuthenticationStateProvider.GetToken() ?? "";
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var res = await _httpClient.PostAsJsonAsync("writer/update", writerUpdateDto);
                var content = await res.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Response>(content, _jsonOptions)!;
            }
            else
            {
                return await Task.FromException<Response>(new Exception("Login Olunamadı"));
            }
        }
    }
}
