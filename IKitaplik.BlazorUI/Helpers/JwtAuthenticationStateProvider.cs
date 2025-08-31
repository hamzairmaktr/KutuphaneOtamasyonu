using Blazored.LocalStorage;
using IKitaplik.BlazorUI.Services.Abstract;
using IKitaplik.Entities.DTOs.UserDTOs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

public class JwtAuthenticationStateProvider : AuthenticationStateProvider
{
    private ClaimsPrincipal _anonymous = new ClaimsPrincipal(new ClaimsIdentity());
    public string? _jwtToken;
    private bool _isInitialized;
    ILocalStorageService localStorageService;
    IAuthService _authService;
    public JwtAuthenticationStateProvider(ILocalStorageService localStorage, IAuthService authService)
    {

        localStorageService = localStorage;
        _authService = authService;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {

        if (!_isInitialized)
        {
            try
            {
            _jwtToken = await localStorageService.GetItemAsStringAsync("authToken");
            _isInitialized = true;

            }
            catch { }
        }

        if (string.IsNullOrWhiteSpace(_jwtToken))
            return new AuthenticationState(_anonymous);

        var claims = ParseClaimsFromJwt(_jwtToken);
        if (IsTokenExpired(claims))
        {
            string refreshToken = await localStorageService.GetItemAsStringAsync("refreshToken");
            var res = await _authService.RefresToken(new RefreshTokenDto { RefreshToken = refreshToken });
            if (res.Success)
            {
                await NotifyUserAuthentication(res.Data.AccessToken);
                return await GetAuthenticationStateAsync();
            }
            else
            {
                _jwtToken = null;
                return new AuthenticationState(_anonymous);
            }
        }

        var identity2 = new ClaimsIdentity(claims, JwtBearerDefaults.AuthenticationScheme);
        var user2 = new ClaimsPrincipal(identity2);
        return new AuthenticationState(user2);
    }

    private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
    {
        var payload = jwt.Split('.')[1];
        var jsonBytes = ParseBase64WithoutPadding(payload);

        try
        {
            var keyValuePairs = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
            return keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value?.ToString() ?? ""));
        }
        catch
        {
            return Enumerable.Empty<Claim>();
        }
    }
    private bool IsTokenExpired(IEnumerable<Claim> claims)
    {
        var expClaim = claims.FirstOrDefault(c => c.Type == "exp");
        if (expClaim != null && long.TryParse(expClaim.Value, out long exp))
        {
            var expiryDate = DateTimeOffset.FromUnixTimeSeconds(exp);
            return expiryDate < DateTimeOffset.UtcNow;
        }
        return false;
    }


    private byte[] ParseBase64WithoutPadding(string base64)
    {
        switch (base64.Length % 4)
        {
            case 2: base64 += "=="; break;
            case 3: base64 += "="; break;
        }
        return Convert.FromBase64String(base64);
    }

    // Token güncelleme ve bildirme metodu
    public async Task NotifyUserAuthentication(string token)
    {
        _jwtToken = token;
        _isInitialized = true;
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    public async Task NotifyUserLogout()
    {
        _jwtToken = null;
        _isInitialized = true;
        await localStorageService.RemoveItemAsync("authToken");
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_anonymous)));
    }

    public async Task<string?> GetToken()
    {
        try
        {
            string token = await localStorageService.GetItemAsStringAsync("authToken") ?? "";
            _jwtToken = token;
            var cliam = ParseClaimsFromJwt(token);
            if (IsTokenExpired(cliam))
            {
                string refreshToken = await localStorageService.GetItemAsStringAsync("refreshToken") ?? "";
                var res = await _authService.RefresToken(new RefreshTokenDto { RefreshToken = refreshToken });
                if (res.Success)
                {
                    await NotifyUserAuthentication(res.Data.AccessToken);
                }
                else
                {
                    return null;
                }
            }
            return _jwtToken?.Replace('"', ' ').Trim();
        }
        catch (Exception)
        {
            return null;
        }
    }
}
