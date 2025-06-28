using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Security.Claims;

public class JwtAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly v _sessionStorage;
    private ClaimsPrincipal _anonymous = new ClaimsPrincipal(new ClaimsIdentity());
    public string? _jwtToken;
    private bool _isInitialized;

    public JwtAuthenticationStateProvider(ProtectedSessionStorage sessionStorage)
    {
        _sessionStorage = sessionStorage;
    }

    public async Task InitializeAsync()
    {
        try
        {
            var result = await _sessionStorage.GetAsync<string>("authToken");
            _jwtToken = result.Success ? result.Value : null;
            _isInitialized = true;
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
        catch (InvalidOperationException)
        {
        }
    }

    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        if (!_isInitialized)
        {
            // Eğer henüz Init edilmediyse, anonim kullanıcı döner.
            return Task.FromResult(new AuthenticationState(_anonymous));
        }

        if (string.IsNullOrWhiteSpace(_jwtToken))
        {
            return Task.FromResult(new AuthenticationState(_anonymous));
        }

        var claims = ParseClaimsFromJwt(_jwtToken);
        if (IsTokenExpired(claims))
        {
            _jwtToken = null;
            return Task.FromResult(new AuthenticationState(_anonymous));
        }
        var identity = new ClaimsIdentity(claims, "jwt");
        var user = new ClaimsPrincipal(identity);
        return Task.FromResult(new AuthenticationState(user));
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
    public async void NotifyUserAuthentication(string token)
    {
        _jwtToken = token;
        _isInitialized = true;
        await _sessionStorage.SetAsync("authToken", token);
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    public async void NotifyUserLogout()
    {
        _jwtToken = null;
        _isInitialized = true;
        await _sessionStorage.DeleteAsync("authToken");
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_anonymous)));
    }

    public async Task<string?> GetToken()
    {
        return _jwtToken;
    }
}
