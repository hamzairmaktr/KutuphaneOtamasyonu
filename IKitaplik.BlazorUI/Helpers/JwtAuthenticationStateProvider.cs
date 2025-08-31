using Blazored.LocalStorage;
using IKitaplik.BlazorUI.Helpers;
using IKitaplik.BlazorUI.Services.Abstract;
using IKitaplik.Entities.DTOs.UserDTOs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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
