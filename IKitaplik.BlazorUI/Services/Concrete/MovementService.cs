using Core.Utilities.Results;
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
        private readonly IAuthService _authService;
        private readonly JsonSerializerOptions _jsonOptions = new() { PropertyNameCaseInsensitive = true };

        public MovementService(HttpClient httpClient, IAuthService authService)
        {
            _httpClient = httpClient;
            _authService = authService;
            _httpClient.BaseAddress = new Uri(Settings.apiUrl);
        }

        private async Task SetAuthorizationHeader()
        {
            string token = await _authService.GetToken() ?? "";
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            else
            {
                throw new Exception("Oturum açılamadı. Kimlik doğrulama token'ı bulunamadı.");
            }
        }

        public async Task<Response<PagedResult<MovementGetDTO>>> GetAllAsync(int page,int pageSize)
        {
            try
            {
                await SetAuthorizationHeader();
                var res = await _httpClient.GetAsync($"Movement/getAll?page={page}&pageSize={pageSize}");
                var content = await res.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Response<PagedResult<MovementGetDTO>>>(content, _jsonOptions)!;
            }
            catch (Exception ex)
            {
                return new Response<PagedResult<MovementGetDTO>> { Success = false, Message = $"Tüm hareketler getirilirken hata oluştu: {ex.Message}", Data = new PagedResult<MovementGetDTO>() };
            }
        }

        public async Task<Response<PagedResult<MovementGetDTO>>> GetAllByBookIdAsync(int id, int page, int pageSize)
        {
            try
            {
                await SetAuthorizationHeader();
                var res = await _httpClient.GetAsync($"Movement/getAllBookId?id={id}&page={page}&pageSize={pageSize}");
                var content = await res.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Response<PagedResult<MovementGetDTO>>>(content, _jsonOptions)!;
            }
            catch (Exception ex)
            {
                return new Response<PagedResult<MovementGetDTO>> { Success = false, Message = $"Kitap ID'ye göre hareketler getirilirken hata oluştu: {ex.Message}", Data = new PagedResult<MovementGetDTO>() };
            }
        }

        public async Task<Response<PagedResult<MovementGetDTO>>> GetAllByBookNameAsync(string bookName, int page, int pageSize)
        {
            try
            {
                await SetAuthorizationHeader();
                var res = await _httpClient.GetAsync($"Movement/getAllBookName?bookName={bookName}&page={page}&pageSize={pageSize}");
                var content = await res.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Response<PagedResult<MovementGetDTO>>>(content, _jsonOptions)!;
            }
            catch (Exception ex)
            {
                return new Response<PagedResult<MovementGetDTO>> { Success = false, Message = $"Kitap adına göre hareketler getirilirken hata oluştu: {ex.Message}", Data = new PagedResult<MovementGetDTO>() };
            }
        }

        public async Task<Response<PagedResult<MovementGetDTO>>> GetAllByDepositIdAsync(int id, int page, int pageSize)
        {
            try
            {
                await SetAuthorizationHeader();
                var res = await _httpClient.GetAsync($"Movement/getAllDepositId?id={id}&page={page}&pageSize={pageSize}");
                var content = await res.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Response<PagedResult<MovementGetDTO>>>(content, _jsonOptions)!;
            }
            catch (Exception ex)
            {
                return new Response<PagedResult<MovementGetDTO>> { Success = false, Message = $"Depozito ID'ye göre hareketler getirilirken hata oluştu: {ex.Message}", Data = new PagedResult<MovementGetDTO>() };
            }
        }

        public async Task<Response<PagedResult<MovementGetDTO>>> GetAllByStudentIdAsync(int id, int page, int pageSize)
        {
            try
            {
                await SetAuthorizationHeader();
                var res = await _httpClient.GetAsync($"Movement/getAllStudentId?id={id}&page={page}&pageSize={pageSize}");
                var content = await res.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Response<PagedResult<MovementGetDTO>>>(content, _jsonOptions)!;
            }
            catch (Exception ex)
            {
                return new Response<PagedResult<MovementGetDTO>> { Success = false, Message = $"Öğrenci ID'ye göre hareketler getirilirken hata oluştu: {ex.Message}", Data = new PagedResult<MovementGetDTO>() };
            }
        }

        public async Task<Response<PagedResult<MovementGetDTO>>> GetAllByStudentNameAsync(string studentName, int page, int pageSize)
        {
            try
            {
                await SetAuthorizationHeader();
                var res = await _httpClient.GetAsync($"Movement/getAllStudentName?studentName={studentName}&page={page}&pageSize={pageSize}");
                var content = await res.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Response<PagedResult<MovementGetDTO>>>(content, _jsonOptions)!;
            }
            catch (Exception ex)
            {
                return new Response<PagedResult<MovementGetDTO>> { Success = false, Message = $"Öğrenci adına göre hareketler getirilirken hata oluştu: {ex.Message}", Data = new PagedResult<MovementGetDTO>() };
            }
        }

        public async Task<Response<PagedResult<MovementGetDTO>>> GetAllByDonationIdAsync(int id, int page, int pageSize)
        {
            try
            {
                await SetAuthorizationHeader();
                var res = await _httpClient.GetAsync($"Movement/getAllDonationId?id={id}&page={page}&pageSize={pageSize}");
                var content = await res.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Response<PagedResult<MovementGetDTO>>>(content, _jsonOptions)!;
            }
            catch (Exception ex)
            {
                return new Response<PagedResult<MovementGetDTO>> { Success = false, Message = $"Bağış ID'ye göre hareketler getirilirken hata oluştu: {ex.Message}", Data = new PagedResult<MovementGetDTO>() };
            }
        }

        public async Task<Response<MovementGetDTO>> GetByIdAsync(int id)
        {
            try
            {
                await SetAuthorizationHeader();
                var response = await _httpClient.GetAsync($"Movement/getById?id={id}");
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