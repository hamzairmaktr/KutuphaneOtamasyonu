using IKitaplik.BlazorUI.Cosntant;
using IKitaplik.BlazorUI.Responses;
using IKitaplik.BlazorUI.Services.Abstract;
using IKitaplik.Entities.DTOs;
using System.Net.Http.Headers;
using System.Net.Http.Json; // PostAsJsonAsync ve ReadFromJsonAsync için
using System.Text.Json;

namespace IKitaplik.BlazorUI.Services.Concrete
{
    public class MovementService : IMovementService
    {
        private readonly HttpClient _httpClient;
        private readonly JwtAuthenticationStateProvider _jwtAuthenticationStateProvider;
        private readonly JsonSerializerOptions _jsonOptions = new() { PropertyNameCaseInsensitive = true };

        public MovementService(HttpClient httpClient, JwtAuthenticationStateProvider jwtAuthenticationStateProvider)
        {
            _httpClient = httpClient;
            _jwtAuthenticationStateProvider = jwtAuthenticationStateProvider;
            _httpClient.BaseAddress = new Uri(Settings.apiUrl);
        }

        private async Task SetAuthorizationHeader()
        {
            string token = await _jwtAuthenticationStateProvider.GetToken() ?? "";
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            else
            {
                throw new Exception("Oturum açılamadı. Kimlik doğrulama token'ı bulunamadı.");
            }
        }

        public async Task<Response<List<MovementGetDTO>>> GetAllAsync()
        {
            try
            {
                await SetAuthorizationHeader();
                var res = await _httpClient.GetAsync("Movement/getAll");
                res.EnsureSuccessStatusCode();
                var content = await res.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Response<List<MovementGetDTO>>>(content, _jsonOptions)!;
            }
            catch (Exception ex)
            {
                return new Response<List<MovementGetDTO>> { Success = false, Message = $"Tüm hareketler getirilirken hata oluştu: {ex.Message}", Data = new List<MovementGetDTO>() };
            }
        }

        public async Task<Response<List<MovementGetDTO>>> GetAllByBookIdAsync(int id)
        {
            try
            {
                await SetAuthorizationHeader();
                var res = await _httpClient.GetAsync($"Movement/getAllBookId?id={id}");
                res.EnsureSuccessStatusCode();
                var content = await res.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Response<List<MovementGetDTO>>>(content, _jsonOptions)!;
            }
            catch (Exception ex)
            {
                return new Response<List<MovementGetDTO>> { Success = false, Message = $"Kitap ID'ye göre hareketler getirilirken hata oluştu: {ex.Message}", Data = new List<MovementGetDTO>() };
            }
        }

        public async Task<Response<List<MovementGetDTO>>> GetAllByBookNameAsync(string bookName)
        {
            try
            {
                await SetAuthorizationHeader();
                var res = await _httpClient.GetAsync($"Movement/getAllBookName?bookName={bookName}");
                res.EnsureSuccessStatusCode();
                var content = await res.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Response<List<MovementGetDTO>>>(content, _jsonOptions)!;
            }
            catch (Exception ex)
            {
                return new Response<List<MovementGetDTO>> { Success = false, Message = $"Kitap adına göre hareketler getirilirken hata oluştu: {ex.Message}", Data = new List<MovementGetDTO>() };
            }
        }

        public async Task<Response<List<MovementGetDTO>>> GetAllByDepositIdAsync(int id)
        {
            try
            {
                await SetAuthorizationHeader();
                var res = await _httpClient.GetAsync($"Movement/getAllDepositId?id={id}");
                res.EnsureSuccessStatusCode();
                var content = await res.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Response<List<MovementGetDTO>>>(content, _jsonOptions)!;
            }
            catch (Exception ex)
            {
                return new Response<List<MovementGetDTO>> { Success = false, Message = $"Depozito ID'ye göre hareketler getirilirken hata oluştu: {ex.Message}", Data = new List<MovementGetDTO>() };
            }
        }

        public async Task<Response<List<MovementGetDTO>>> GetAllByStudentIdAsync(int id)
        {
            try
            {
                await SetAuthorizationHeader();
                var res = await _httpClient.GetAsync($"Movement/getAllStudentId?id={id}");
                res.EnsureSuccessStatusCode();
                var content = await res.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Response<List<MovementGetDTO>>>(content, _jsonOptions)!;
            }
            catch (Exception ex)
            {
                return new Response<List<MovementGetDTO>> { Success = false, Message = $"Öğrenci ID'ye göre hareketler getirilirken hata oluştu: {ex.Message}", Data = new List<MovementGetDTO>() };
            }
        }

        public async Task<Response<List<MovementGetDTO>>> GetAllByStudentNameAsync(string studentName)
        {
            try
            {
                await SetAuthorizationHeader();
                var res = await _httpClient.GetAsync($"Movement/getAllStudentName?studentName={studentName}");
                res.EnsureSuccessStatusCode();
                var content = await res.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Response<List<MovementGetDTO>>>(content, _jsonOptions)!;
            }
            catch (Exception ex)
            {
                return new Response<List<MovementGetDTO>> { Success = false, Message = $"Öğrenci adına göre hareketler getirilirken hata oluştu: {ex.Message}", Data = new List<MovementGetDTO>() };
            }
        }

        public async Task<Response<List<MovementGetDTO>>> GetAllByDonationIdAsync(int id)
        {
            try
            {
                await SetAuthorizationHeader();
                var res = await _httpClient.GetAsync($"Movement/getAllDonationId?id={id}");
                res.EnsureSuccessStatusCode();
                var content = await res.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Response<List<MovementGetDTO>>>(content, _jsonOptions)!;
            }
            catch (Exception ex)
            {
                return new Response<List<MovementGetDTO>> { Success = false, Message = $"Bağış ID'ye göre hareketler getirilirken hata oluştu: {ex.Message}", Data = new List<MovementGetDTO>() };
            }
        }

        public async Task<Response<MovementGetDTO>> GetByIdAsync(int id)
        {
            try
            {
                await SetAuthorizationHeader();
                var response = await _httpClient.GetAsync($"Movement/getById?id={id}");
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Response<MovementGetDTO>>(content, _jsonOptions)!;
            }
            catch (Exception ex)
            {
                return new Response<MovementGetDTO> { Success = false, Message = $"Hareket getirilirken hata oluştu: {ex.Message}" };
            }
        }
    }
}