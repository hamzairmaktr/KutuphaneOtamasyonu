using Blazored.LocalStorage;
using IKitaplik.BlazorUI.Cosntant;
using IKitaplik.BlazorUI.Responses;
using IKitaplik.BlazorUI.Services.Abstract;
using IKitaplik.Entities.DTOs.BookDTOs;
using IKitaplik.Entities.DTOs.UserDTOs;
using Microsoft.AspNetCore.Components;
using System.Text.Json;

namespace IKitaplik.BlazorUI.Services.Concrete
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly JwtAuthenticationStateProvider _jwtAuthenticationStateProvider;
        private readonly JsonSerializerOptions _jsonOptions = new() { PropertyNameCaseInsensitive = true };
        NavigationManager _navigationManager;
        ILocalStorageService _localStorage;
        JwtAuthenticationStateProvider _authProvider;
        public AuthService(HttpClient httpClient, JwtAuthenticationStateProvider jwtAuthenticationStateProvider, NavigationManager navigationManager, ILocalStorageService localStorageService, JwtAuthenticationStateProvider authProvider)
        {
            _httpClient = httpClient;
            _jwtAuthenticationStateProvider = jwtAuthenticationStateProvider;
            _httpClient.BaseAddress = new Uri(Settings.apiUrl);
            _navigationManager = navigationManager;
            this._localStorage = localStorageService;
            _authProvider = authProvider;
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
                _authProvider.NotifyUserAuthentication(response.Data.AccessToken);
                _navigationManager.NavigateTo("/");
            }
            return response;
        }

        public Task<Response> Logout()
        {
            throw new NotImplementedException();
        }

        public Task<Response<LoginResponse>> RefresToken(RefreshTokenDto userRegisterDto)
        {
            throw new NotImplementedException();
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
