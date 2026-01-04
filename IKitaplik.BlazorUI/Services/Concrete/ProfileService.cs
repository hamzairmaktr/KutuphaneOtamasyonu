using IKitaplik.BlazorUI.Cosntant;
using IKitaplik.BlazorUI.Responses;
using IKitaplik.BlazorUI.Services.Abstract;
using IKitaplik.Entities.Concrete;
using IKitaplik.Entities.DTOs.UserDTOs;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace IKitaplik.BlazorUI.Services.Concrete
{
    public class ProfileService : IProfileService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions = new() { PropertyNameCaseInsensitive = true };
        private readonly IAuthService _authService;
        public ProfileService(HttpClient httpClient, IAuthService authService)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(Settings.apiUrl);
            _authService = authService;
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
                throw new Exception("Oturum açýlamadý. Kimlik doðrulama token'ý bulunamadý.");
            }
        }
        public async Task<Response<User>> GetProfile()
         {
            try
            {
                await SetAuthorizationHeader();
                var res = await _httpClient.GetAsync("profile");
                var content = await res.Content.ReadAsStringAsync();
                
                if (!res.IsSuccessStatusCode)
                {
                    try 
                    {
                        return JsonSerializer.Deserialize<Response<User>>(content, _jsonOptions)!;
                    }
                    catch 
                    {
                         return new Response<User> { Success = false, Message = $"Server Error: {res.StatusCode}" };
                    }
                }
                
                return JsonSerializer.Deserialize<Response<User>>(content, _jsonOptions)!;
            }
            catch (Exception ex)
            {
                return new Response<User> { Success = false, Message = $"Connection Error: {ex.Message}" };
            }
        }

        public async Task<Response> UpdateProfile(UserProfileUpdateDto profileDto)
        {
            try
            {
                await SetAuthorizationHeader();
                var res = await _httpClient.PostAsJsonAsync("profile/update", profileDto);
                var content = await res.Content.ReadAsStringAsync();
                
                if (!res.IsSuccessStatusCode)
                {
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

        public async Task<Response> ChangePassword(ChangePasswordDto passwordDto)
        {
            try
            {
                await SetAuthorizationHeader();
                var res = await _httpClient.PostAsJsonAsync("profile/change-password", passwordDto);
                var content = await res.Content.ReadAsStringAsync();

                 if (!res.IsSuccessStatusCode)
                {
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
    }
}
