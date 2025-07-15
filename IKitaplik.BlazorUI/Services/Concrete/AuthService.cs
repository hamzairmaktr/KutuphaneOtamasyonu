using Blazored.LocalStorage;
using IKitaplik.BlazorUI.Cosntant;
using IKitaplik.BlazorUI.Responses;
using IKitaplik.BlazorUI.Services.Abstract;
using IKitaplik.Entities.DTOs.BookDTOs;
using IKitaplik.Entities.DTOs.UserDTOs;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Text.Json;

namespace IKitaplik.BlazorUI.Services.Concrete
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions = new() { PropertyNameCaseInsensitive = true };
        NavigationManager _navigationManager;
        ILocalStorageService _localStorage;
        public AuthService(HttpClient httpClient, NavigationManager navigationManager, ILocalStorageService localStorageService)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(Settings.apiUrl);
            _navigationManager = navigationManager;
            this._localStorage = localStorageService;
        }
        public async Task<Response<LoginResponse>> Login(UserLoginDto userLoginDto)
        {
            var res = await _httpClient.PostAsJsonAsync("auth/login", userLoginDto);
            var content = await res.Content.ReadAsStringAsync();

            var response = JsonSerializer.Deserialize<Response<LoginResponse>>(content, _jsonOptions)!;
            if (response.Success)
            {
                await _localStorage.SetItemAsync("authToken", response.Data.AccessToken);
                await _localStorage.SetItemAsync("refreshToken", response.Data.RefreshToken);
                _navigationManager.NavigateTo("/",true);
            }
            return response;
        }
        public async Task<Response<LoginResponse>> RefresToken(RefreshTokenDto userRegisterDto)
        {
            string refreshToken = await _localStorage.GetItemAsync<string>("refreshToken");
            var res = await _httpClient.PostAsJsonAsync("auth/refresh-token", new RefreshTokenDto { RefreshToken = refreshToken });
            var content = await res.Content.ReadAsStringAsync();

            var response = JsonSerializer.Deserialize<Response<LoginResponse>>(content, _jsonOptions)!;
            if (response.Success)
            {
                await _localStorage.SetItemAsync("authToken", response.Data.AccessToken);
                await _localStorage.SetItemAsync("refreshToken", response.Data.RefreshToken);
            }
            else
            {
                await _localStorage.RemoveItemAsync("authToken");
                await _localStorage.RemoveItemAsync("refreshToken");
            }
            return response;
        }

        public async Task<Response> Register(UserRegisterDto userRegisterDto)
        {
            var res = await _httpClient.PostAsJsonAsync("auth/register", userRegisterDto);
            var content = await res.Content.ReadAsStringAsync();

            var response = JsonSerializer.Deserialize<Response>(content, _jsonOptions)!;
            if (response.Success)
            {
                _navigationManager.NavigateTo("/login");
            }
            else
            {

            }
            return response;
        }
    }
}
