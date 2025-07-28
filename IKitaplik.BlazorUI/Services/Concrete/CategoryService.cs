using IKitaplik.BlazorUI.Cosntant;
using IKitaplik.BlazorUI.Responses;
using IKitaplik.BlazorUI.Services.Abstract;
using IKitaplik.Entities.DTOs;
using IKitaplik.Entities.DTOs.CategoryDTOs;
using System.Net.Http.Headers;
using System.Net.Http.Json; // Added for PostAsJsonAsync and ReadFromJsonAsync
using System.Text.Json;

namespace IKitaplik.BlazorUI.Services.Concrete
{
    public class CategoryService : ICategoryService
    {
        private readonly HttpClient _httpClient;
        private readonly JwtAuthenticationStateProvider _jwtAuthenticationStateProvider;
        private readonly JsonSerializerOptions _jsonOptions = new() { PropertyNameCaseInsensitive = true };

        public CategoryService(HttpClient httpClient, JwtAuthenticationStateProvider jwtAuthenticationStateProvider)
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
                throw new Exception("Login Olunamadı. Token bulunamadı.");
            }
        }

        public async Task<Response> AddAsync(CategoryAddDto categoryAddDto)
        {
            try
            {
                await SetAuthorizationHeader();
                var res = await _httpClient.PostAsJsonAsync("Category/add", categoryAddDto);
                var content = await res.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Response>(content, _jsonOptions)!;
            }
            catch (Exception ex)
            {
                return new Response { Success = false, Message = $"Kategori eklenirken hata oluştu: {ex.Message}" };
            }
        }

        public async Task<Response> DeleteAsync(int id)
        {
            try
            {
                await SetAuthorizationHeader();

                var res = await _httpClient.PostAsJsonAsync("Category/delete", new DeleteDto { Id = id });
                var content = await res.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Response>(content, _jsonOptions)!;
            }
            catch (Exception ex)
            {
                return new Response { Success = false, Message = $"Kategori silinirken hata oluştu: {ex.Message}" };
            }
        }

        public async Task<Response<List<CategoryGetDto>>> GetAllAsync()
        {
            try
            {
                await SetAuthorizationHeader();
                var res = await _httpClient.GetAsync("Category/getall");
                res.EnsureSuccessStatusCode();
                var content = await res.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Response<List<CategoryGetDto>>>(content, _jsonOptions)!;
            }
            catch (Exception ex)
            {
                return new Response<List<CategoryGetDto>> { Success = false, Message = $"Kategoriler getirilirken hata oluştu: {ex.Message}", Data = new List<CategoryGetDto>() };
            }
        }

        public async Task<Response<CategoryGetDto>> GetByIdAsync(int id)
        {
            try
            {
                await SetAuthorizationHeader();
                var response = await _httpClient.GetAsync($"Category/getbyid?id={id}");
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Response<CategoryGetDto>>(content, _jsonOptions)!;
            }
            catch (Exception ex)
            {
                return new Response<CategoryGetDto> { Success = false, Message = $"Kategori getirilirken hata oluştu: {ex.Message}" };
            }
        }

        public async Task<Response> UpdateAsync(CategoryUpdateDto categoryUpdateDto)
        {
            try
            {
                await SetAuthorizationHeader();
                var res = await _httpClient.PostAsJsonAsync("Category/update", categoryUpdateDto);
                var content = await res.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Response>(content, _jsonOptions)!;
            }
            catch (Exception ex)
            {
                return new Response { Success = false, Message = $"Kategori güncellenirken hata oluştu: {ex.Message}" };
            }
        }
    }
}