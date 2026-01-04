using IKitaplik.BlazorUI.Cosntant;
using IKitaplik.BlazorUI.Helpers;
using IKitaplik.BlazorUI.Responses;
using IKitaplik.BlazorUI.Services.Abstract;
using IKitaplik.Entities.DTOs.UserDTOs;
using Microsoft.AspNetCore.Components;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;

namespace IKitaplik.BlazorUI.Services.Concrete
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions = new() { PropertyNameCaseInsensitive = true };
        private readonly NavigationManager _navigationManager;
        private readonly AccessTokenService accessTokenService;
        private readonly RefreshTokenService refreshTokenService;
        public AuthService(HttpClient httpClient, NavigationManager navigationManager, AccessTokenService accessTokenService, RefreshTokenService refreshTokenService)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(Settings.apiUrl);
            _navigationManager = navigationManager;
            this.accessTokenService = accessTokenService;
            this.refreshTokenService = refreshTokenService;
        }

        public async Task<string> GetToken()
        {
            string accessToken = await accessTokenService.GetToken();
            if (string.IsNullOrWhiteSpace(accessToken)) return string.Empty;
            var claims = new JwtSecurityTokenHandler().ReadJwtToken(accessToken).Claims;
            var expValue = claims.First(p => p.Type == "exp").Value;
            var expDate = DateTimeOffset.FromUnixTimeSeconds(long.Parse(expValue)).UtcDateTime;
            if (DateTime.Now < expDate) return accessToken;
            var res = await RefresToken();
            return res.Data?.AccessToken ?? string.Empty;
        }

        public async Task<Response<LoginResponse>> Login(UserLoginDto userLoginDto)
        {
            var res = await _httpClient.PostAsJsonAsync("auth/login", userLoginDto);
            var content = await res.Content.ReadAsStringAsync();

            var response = JsonSerializer.Deserialize<Response<LoginResponse>>(content, _jsonOptions)!;
            if (response.Success)
            {
                if (response.Data != null && response.Data.IsTwoFactor)
                {
                    // Do nothing, let the component handle 2FA navigation
                    return response;
                }

                await accessTokenService.SetToken(response.Data.AccessToken);
                await refreshTokenService.SetToken(response.Data.RefreshToken);
                _navigationManager.NavigateTo("/", true);
            }
            return response;
        }

        public async Task LogOut()
        {
            await accessTokenService.RemoveToken();
            await refreshTokenService.RemoveToken();
            _navigationManager.NavigateTo("/login", true);
        }

        public async Task<Response<LoginResponse>> RefresToken()
        {
            string refreshToken = await refreshTokenService.GetToken();
            var res = await _httpClient.PostAsJsonAsync("auth/refresh-token", new RefreshTokenDto { RefreshToken = refreshToken });
            var content = await res.Content.ReadAsStringAsync();

            var response = JsonSerializer.Deserialize<Response<LoginResponse>>(content, _jsonOptions)!;
            if (response.Success)
            {
                await accessTokenService.SetToken(response.Data.AccessToken);
                await refreshTokenService.SetToken(response.Data.RefreshToken);
            }
            else
            {
                await accessTokenService.RemoveToken();
                await refreshTokenService.RemoveToken();
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
            return response;
        }

        public async Task<Response<LoginResponse>> VerifyTwoFactor(VerifyTwoFactorDto verifyTwoFactorDto)
        {
             var res = await _httpClient.PostAsJsonAsync("auth/verify-2fa", verifyTwoFactorDto);
             var content = await res.Content.ReadAsStringAsync();
             var response = JsonSerializer.Deserialize<Response<LoginResponse>>(content, _jsonOptions)!;

             if(response.Success)
             {
                await accessTokenService.SetToken(response.Data.AccessToken);
                await refreshTokenService.SetToken(response.Data.RefreshToken);
                _navigationManager.NavigateTo("/", true);
             }
             return response;
        }

        public async Task<Response> ForgotPassword(ForgotPasswordDto forgotPasswordDto)
        {
            try
            {
                var res = await _httpClient.PostAsJsonAsync("auth/forgot-password", forgotPasswordDto);
                var content = await res.Content.ReadAsStringAsync();
                
                if (!res.IsSuccessStatusCode)
                {
                    // Try to parse error response, fallback to content
                    try 
                    {
                        return JsonSerializer.Deserialize<Response>(content, _jsonOptions)!;
                    }
                    catch 
                    {
                         return new Response { Success = false, Message = $"Server Error: {res.StatusCode}" };
                    }
                }

                return JsonSerializer.Deserialize<Response>(content, _jsonOptions)!;
            }
            catch (Exception ex)
            {
                return new Response { Success = false, Message = $"Connection Error: {ex.Message}" };
            }
        }

        public async Task<Response> ResetPassword(ResetPasswordDto resetPasswordDto)
        {
             var res = await _httpClient.PostAsJsonAsync("auth/reset-password", resetPasswordDto);
             var content = await res.Content.ReadAsStringAsync();
             return JsonSerializer.Deserialize<Response>(content, _jsonOptions)!;
        }
    }
}
