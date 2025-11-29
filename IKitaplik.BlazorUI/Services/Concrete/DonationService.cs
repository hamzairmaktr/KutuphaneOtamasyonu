using IKitaplik.BlazorUI.Cosntant;
using IKitaplik.BlazorUI.Responses;
using IKitaplik.BlazorUI.Services.Abstract;
using IKitaplik.Entities.DTOs.DonationDTOs; 
using System.Net.Http.Headers;
using System.Text.Json;

namespace IKitaplik.BlazorUI.Services.Concrete
{
    public class DonationService : IDonationService
    {
        private readonly HttpClient _httpClient;
        private readonly IAuthService _authService;
        private readonly JsonSerializerOptions _jsonOptions = new() { PropertyNameCaseInsensitive = true };

        public DonationService(HttpClient httpClient, IAuthService authService)
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

        public async Task<Response> AddAsync(DonationAddDto donationAddDto)
        {
            try
            {
                await SetAuthorizationHeader();
                var res = await _httpClient.PostAsJsonAsync("Donation/add", donationAddDto);
                var content = await res.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Response>(content, _jsonOptions)!;
            }
            catch (Exception ex)
            {
                return new Response { Success = false, Message = $"Bağış eklenirken hata oluştu: {ex.Message}" };
            }
        }

        public async Task<Response<List<DonationGetDTO>>> GetAllAsync()
        {
            try
            {
                await SetAuthorizationHeader();
                var res = await _httpClient.GetAsync("Donation/getAll");
                var content = await res.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Response<List<DonationGetDTO>>>(content, _jsonOptions)!;
            }
            catch (Exception ex)
            {
                return new Response<List<DonationGetDTO>> { Success = false, Message = $"Tüm bağışlar getirilirken hata oluştu: {ex.Message}", Data = new List<DonationGetDTO>() };
            }
        }

        public async Task<Response<DonationGetDTO>> GetByIdAsync(int id)
        {
            try
            {
                await SetAuthorizationHeader();
                var response = await _httpClient.GetAsync($"Donation/getById?id={id}");
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Response<DonationGetDTO>>(content, _jsonOptions)!;
            }
            catch (Exception ex)
            {
                return new Response<DonationGetDTO> { Success = false, Message = $"Bağış getirilirken hata oluştu: {ex.Message}" };
            }
        }
    }
}