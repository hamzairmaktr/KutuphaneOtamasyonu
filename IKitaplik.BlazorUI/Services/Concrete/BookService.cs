using IKitaplik.BlazorUI.Cosntant;
using IKitaplik.BlazorUI.Responses;
using IKitaplik.BlazorUI.Services.Abstract;
using IKitaplik.Entities.DTOs.BookDTOs;
using System.Net.Http.Headers;
using System.Text.Json;

namespace IKitaplik.BlazorUI.Services.Concrete
{
    public class BookService : IBookService
    {
        private readonly HttpClient _httpClient;
        private readonly JwtAuthenticationStateProvider _jwtAuthenticationStateProvider;
        private readonly JsonSerializerOptions _jsonOptions = new() { PropertyNameCaseInsensitive = true };
        public BookService(HttpClient httpClient, JwtAuthenticationStateProvider jwtAuthenticationStateProvider)
        {
            _httpClient = httpClient;
            _jwtAuthenticationStateProvider = jwtAuthenticationStateProvider;
            _httpClient.BaseAddress = new Uri(Settings.apiUrl);
        }

        public Task<Response> AddBookAsync<T>(BookAddDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<Response> DeleteBookAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Response<List<BookGetDTO>>> GetAllBooksAsync()
        {
            string token = await _jwtAuthenticationStateProvider.GetToken();
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var res = await _httpClient.GetAsync("book/getall");
                var content = await res.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Response<List<BookGetDTO>>>(content, _jsonOptions)!;
            }
            else
            {
                return await Task.FromException<Response<List<BookGetDTO>>>(new Exception("Login Olunamadı"));
            }
        }


        public async Task<Response<BookGetDTO>> GetBookDetailsAsync(int id)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await _jwtAuthenticationStateProvider.GetToken());
            var response = await _httpClient.GetAsync($"api/books/{id}/details");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Response<BookGetDTO>>(_jsonOptions)!;
        }

        public Task<Response> UpdateBookAsync(BookUpdateDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
