using Core.Entities;
using IKitaplik.BlazorUI.Cosntant;
using IKitaplik.BlazorUI.Responses;
using IKitaplik.BlazorUI.Services.Abstract;
using IKitaplik.Entities.Concrete;
using IKitaplik.Entities.DTOs;
using IKitaplik.Entities.DTOs.BookDTOs;
using System.Net.Http.Headers;
using System.Text.Json;

namespace IKitaplik.BlazorUI.Services.Concrete
{
    public class BookService : IBookService
    {
        private readonly HttpClient _httpClient;
        private readonly IAuthService _authService;
        private readonly JsonSerializerOptions _jsonOptions = new() { PropertyNameCaseInsensitive = true };
        public BookService(HttpClient httpClient, IAuthService authService)
        {
            _httpClient = httpClient;
            _authService = authService;
            _httpClient.BaseAddress = new Uri(Settings.apiUrl);
        }

        public async Task<Response> AddBookAsync(BookAddDto dto)
        {
            string token = await _authService.GetToken() ?? "";
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var res = await _httpClient.PostAsJsonAsync("book/add", dto);
                var content = await res.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Response>(content, _jsonOptions)!;
            }
            else
            {
                return await Task.FromException<Response>(new Exception("Login Olunamadı"));
            }
        }

        public async Task<Response> BookAddPieceAsync(BookAddPieceDto dto)
        {
            string token = await _authService.GetToken() ?? "";
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var res = await _httpClient.PostAsJsonAsync("book/bookAddPiece", dto);
                var content = await res.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Response>(content, _jsonOptions)!;
            }
            else
            {
                return await Task.FromException<Response>(new Exception("Login Olunamadı"));
            }
        }

        public async Task<Response> DeleteBookAsync(int id)
        {
            string token = await _authService.GetToken() ?? "";
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var res = await _httpClient.PostAsJsonAsync("book/delete", new DeleteDto { Id = id});
                var content = await res.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Response>(content, _jsonOptions)!;
            }
            else
            {
                return await Task.FromException<Response>(new Exception("Login Olunamadı"));
            }
        }

        public async Task<Response<List<BookGetDTO>>> GetAllActiveBooksAsync()
        {
            string token = await _authService.GetToken();
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var res = await _httpClient.GetAsync("book/getallisactive");
                var content = await res.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Response<List<BookGetDTO>>>(content, _jsonOptions)!;
            }
            else
            {
                return await Task.FromException<Response<List<BookGetDTO>>>(new Exception("Login Olunamadı"));
            }
        }

        public async Task<Response<List<BookGetDTO>>> GetAllBooksAsync()
        {
            string token = await _authService.GetToken();
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
        //getByBarcode
        public async Task<Response<Book>> GetBookByBarcodeAsync(string barcode)
        {
            string token = await _authService.GetToken();
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var res = await _httpClient.GetAsync("book/getByBarcode?barcode?" + barcode);
                var content = await res.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Response<Book>>(content, _jsonOptions)!;
            }
            else
            {
                return await Task.FromException<Response<Book>>(new Exception("Login Olunamadı"));
            }
        }

        public async Task<Response<Book>> GetBookByIdAsync(int id)
        {
            string token = await _authService.GetToken();
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await _httpClient.GetAsync($"book/getById?id={id}");
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<Response<Book>>(_jsonOptions)!;
            }
            else
            {
                return await Task.FromException<Response<Book>>(new Exception("Login Olunamadı"));
            }
        }

        public async Task<Response> UpdateBookAsync(BookUpdateDto dto)
        {
            string token = await _authService.GetToken() ?? "";
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var res = await _httpClient.PostAsJsonAsync("book/update", dto);
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
