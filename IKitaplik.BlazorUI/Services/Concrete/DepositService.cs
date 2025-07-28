using IKitaplik.BlazorUI.Cosntant;
using IKitaplik.BlazorUI.Responses;
using IKitaplik.BlazorUI.Services.Abstract;
using IKitaplik.Entities.DTOs;
using IKitaplik.Entities.DTOs.DepositDTOs; 
using System.Net.Http.Headers;
using System.Text.Json;

namespace IKitaplik.BlazorUI.Services.Concrete
{
    public class DepositService : IDepositService
    {
        private readonly HttpClient _httpClient;
        private readonly JwtAuthenticationStateProvider _jwtAuthenticationStateProvider;
        private readonly JsonSerializerOptions _jsonOptions = new() { PropertyNameCaseInsensitive = true };

        public DepositService(HttpClient httpClient, JwtAuthenticationStateProvider jwtAuthenticationStateProvider)
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

        public async Task<Response> DepositGivenAsync(DepositAddDto depositAddDto)
        {
            try
            {
                await SetAuthorizationHeader();
                var res = await _httpClient.PostAsJsonAsync("Deposit/depositGiven", depositAddDto);
                var content = await res.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Response>(content, _jsonOptions)!;
            }
            catch (Exception ex)
            {
                return new Response { Success = false, Message = $"Depozito verilirken hata oluştu: {ex.Message}" };
            }
        }

        public async Task<Response> DepositReceivedAsync(DepositUpdateDto depositUpdateDto)
        {
            try
            {
                await SetAuthorizationHeader();
                var res = await _httpClient.PostAsJsonAsync("Deposit/depositReceived", depositUpdateDto);
                var content = await res.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Response>(content, _jsonOptions)!;
            }
            catch (Exception ex)
            {
                return new Response { Success = false, Message = $"Depozito alınırken hata oluştu: {ex.Message}" };
            }
        }

        public async Task<Response> DeleteAsync(int id)
        {
            try
            {
                await SetAuthorizationHeader();
                var res = await _httpClient.PostAsJsonAsync("Deposit/delete", new DeleteDto { Id = id });
                var content = await res.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Response>(content, _jsonOptions)!;
            }
            catch (Exception ex)
            {
                return new Response { Success = false, Message = $"Depozito silinirken hata oluştu: {ex.Message}" };
            }
        }

        public async Task<Response> ExtendDueDateAsync(DepositExtentDueDateDto depositExtentDueDateDto)
        {
            try
            {
                await SetAuthorizationHeader();
                var res = await _httpClient.PostAsJsonAsync("Deposit/extendDuDate", depositExtentDueDateDto);
                var content = await res.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Response>(content, _jsonOptions)!;
            }
            catch (Exception ex)
            {
                return new Response { Success = false, Message = $"Depozito vadesi uzatılırken hata oluştu: {ex.Message}" };
            }
        }

        public async Task<Response<List<DepositGetDTO>>> GetAllAsync()
        {
            try
            {
                await SetAuthorizationHeader();
                var res = await _httpClient.GetAsync("Deposit/getAll");
                res.EnsureSuccessStatusCode();
                var content = await res.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Response<List<DepositGetDTO>>>(content, _jsonOptions)!;
            }
            catch (Exception ex)
            {
                return new Response<List<DepositGetDTO>> { Success = false, Message = $"Tüm depozitolar getirilirken hata oluştu: {ex.Message}", Data = new List<DepositGetDTO>() };
            }
        }

        public async Task<Response<DepositGetDTO>> GetByIdAsync(int id)
        {
            try
            {
                await SetAuthorizationHeader();
                var response = await _httpClient.GetAsync($"Deposit/getById?id={id}");
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Response<DepositGetDTO>>(content, _jsonOptions)!;
            }
            catch (Exception ex)
            {
                return new Response<DepositGetDTO> { Success = false, Message = $"Depozito getirilirken hata oluştu: {ex.Message}" };
            }
        }
    }
}