using IKitaplik.BlazorUI.Services.Abstract;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

public class JwtAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly IAuthService _authService;
    public JwtAuthenticationStateProvider(IAuthService authService)
    {
        _authService = authService;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            var token = await _authService.GetToken();
            if (string.IsNullOrWhiteSpace(token))
                return await MarkAsUnauthorize();
            var readJwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            var expClaim = readJwt.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Exp)?.Value;
            if (expClaim != null && long.TryParse(expClaim, out var expUnix))
            {
                var expDate = DateTimeOffset.FromUnixTimeSeconds(expUnix).UtcDateTime;
                if (expDate < DateTime.UtcNow)
                {
                    await _authService.RefresToken();
                    return await GetAuthenticationStateAsync();
                }
            }
            var identity = new ClaimsIdentity(readJwt.Claims, "JWT");
            var principal = new ClaimsPrincipal(identity);
            return await Task.FromResult(new AuthenticationState(principal));
        }
        catch (Exception)
        {
            return await MarkAsUnauthorize();
        }
    }

    private async Task<AuthenticationState> MarkAsUnauthorize()
    {
        try
        {
            var state = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            NotifyAuthenticationStateChanged(Task.FromResult(state));
            return state;
        }
        catch (Exception)
        {
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }
    }
}
