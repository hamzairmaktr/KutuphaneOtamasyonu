using IKitaplik.BlazorUI.Cosntant;
using IKitaplik.BlazorUI.Responses;
using IKitaplik.BlazorUI.Services.Abstract;
using IKitaplik.Entities.DTOs;
using System.Net.Http.Headers;
using System.Text.Json;

namespace IKitaplik.BlazorUI.Services.Concrete
{
    public class StudentService : IStudentService
    {
        private readonly HttpClient _httpClient;
        private readonly IAuthService _authService;
        private readonly JsonSerializerOptions _jsonOptions = new() { PropertyNameCaseInsensitive = true };

        public StudentService(HttpClient httpClient, IAuthService authService)
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

        public async Task<Response> AddAsync(StudentAddDto studentAddDto)
        {
            try
            {
                await SetAuthorizationHeader();
                var res = await _httpClient.PostAsJsonAsync("Student/add", studentAddDto);
                var content = await res.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Response>(content, _jsonOptions)!;
            }
            catch (Exception ex)
            {
                return new Response { Success = false, Message = $"Öğrenci eklenirken hata oluştu: {ex.Message}" };
            }
        }

        public async Task<Response> DeleteAsync(int id)
        {
            try
            {
                await SetAuthorizationHeader();
                // API'niz PostAsJsonAsync ile int kabul ediyorsa bu şekilde devam edilebilir.
                // Genellikle DELETE operasyonları için HTTP DELETE metodu veya Query/Route parametresi tercih edilir.
                var res = await _httpClient.PostAsJsonAsync("Student/delete", new DeleteDto { Id = id });
                var content = await res.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Response>(content, _jsonOptions)!;
            }
            catch (Exception ex)
            {
                return new Response { Success = false, Message = $"Öğrenci silinirken hata oluştu: {ex.Message}" };
            }
        }

        public async Task<Response<List<StudentGetDto>>> GetAllAsync()
        {
            try
            {
                await SetAuthorizationHeader();
                var res = await _httpClient.GetAsync("Student/getall");
                res.EnsureSuccessStatusCode(); // HTTP olmayan bir durum kodu varsa hata fırlatır
                var content = await res.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Response<List<StudentGetDto>>>(content, _jsonOptions)!;
            }
            catch (Exception ex)
            {
                return new Response<List<StudentGetDto>> { Success = false, Message = $"Öğrenciler getirilirken hata oluştu: {ex.Message}", Data = new List<StudentGetDto>() };
            }
        }

        public async Task<Response<List<StudentGetDto>>> GetAllByNameAsync(string name)
        {
            try
            {
                await SetAuthorizationHeader();
                // Query parametresi olarak 'name' gönderiyoruz
                var res = await _httpClient.GetAsync($"Student/getallbyname?name={name}");
                res.EnsureSuccessStatusCode();
                var content = await res.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Response<List<StudentGetDto>>>(content, _jsonOptions)!;
            }
            catch (Exception ex)
            {
                return new Response<List<StudentGetDto>> { Success = false, Message = $"İsme göre öğrenciler getirilirken hata oluştu: {ex.Message}", Data = new List<StudentGetDto>() };
            }
        }

        public async Task<Response<StudentGetDto>> GetByIdAsync(int id)
        {
            try
            {
                await SetAuthorizationHeader();
                var response = await _httpClient.GetAsync($"Student/getbyid?id={id}");
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Response<StudentGetDto>>(content, _jsonOptions)!;
            }
            catch (Exception ex)
            {
                return new Response<StudentGetDto> { Success = false, Message = $"Öğrenci getirilirken hata oluştu: {ex.Message}" };
            }
        }

        public async Task<Response> UpdateAsync(StudentUpdateDto studentUpdateDto)
        {
            try
            {
                await SetAuthorizationHeader();
                var res = await _httpClient.PostAsJsonAsync("Student/update", studentUpdateDto);
                var content = await res.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Response>(content, _jsonOptions)!;
            }
            catch (Exception ex)
            {
                return new Response { Success = false, Message = $"Öğrenci güncellenirken hata oluştu: {ex.Message}" };
            }
        }

        public async Task<Response<List<StudentGetDto>>> GetAllActiveAsync()
        {
            try
            {
                await SetAuthorizationHeader();
                var res = await _httpClient.GetAsync("Student/getallisactive");
                res.EnsureSuccessStatusCode(); // HTTP olmayan bir durum kodu varsa hata fırlatır
                var content = await res.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Response<List<StudentGetDto>>>(content, _jsonOptions)!;
            }
            catch (Exception ex)
            {
                return new Response<List<StudentGetDto>> { Success = false, Message = $"Öğrenciler getirilirken hata oluştu: {ex.Message}", Data = new List<StudentGetDto>() };
            }
        }
    }
}