using Core.Entities;
using IKitaplik.BlazorUI.Cosntant;
using IKitaplik.BlazorUI.Responses;
using IKitaplik.BlazorUI.Services.Abstract;
using IKitaplik.Entities.DTOs;
using IKitaplik.Entities.DTOs.BookDTOs;
using IKitaplik.Entities.DTOs.WriterDTOs;
using System.Net.Http.Headers;
using System.Text.Json;

namespace IKitaplik.BlazorUI.Services.Concrete
{
    public class WriterService : IWriterService
    {
        private readonly HttpClient _httpClient;
        private readonly IAuthService _authService;
        private readonly JsonSerializerOptions _jsonOptions = new() { PropertyNameCaseInsensitive = true };
        public WriterService(HttpClient httpClient, IAuthService authService)
        {
            _httpClient = httpClient;
            _authService = authService;
            _httpClient.BaseAddress = new Uri(Settings.apiUrl);
        }

        public async Task<Response> AddAsync(WriterAddDto writerAddDto)
        {
            string token = await _authService.GetToken() ?? "";
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
            string token = await _authService.GetToken() ?? "";
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var res = await _httpClient.PostAsJsonAsync("writer/delete", new DeleteDto { Id = id });
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
            string token = await _authService.GetToken();
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
            try
            {
                string token = await _authService.GetToken();
                if (!string.IsNullOrEmpty(token))
                {
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var response = await _httpClient.GetAsync($"writer/getById?id={id}");
                    response.EnsureSuccessStatusCode();
                    return await response.Content.ReadFromJsonAsync<Response<WriterGetDto>>(_jsonOptions)!;
                }
                else
                {
                    return await Task.FromException<Response<WriterGetDto>>(new Exception("Login Olunamadı"));
                }
            }
            catch (Exception ex)
            {
                return await Task.FromException<Response<WriterGetDto>>(new Exception(ex.Message));
            }
        }

        public async Task<Response> UpdateAsync(WriterUpdateDto writerUpdateDto)
        {
            string token = await _authService.GetToken() ?? "";
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
