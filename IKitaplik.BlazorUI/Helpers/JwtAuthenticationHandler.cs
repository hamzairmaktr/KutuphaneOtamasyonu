using Microsoft.AspNetCore.Authentication;

namespace IKitaplik.BlazorUI.Helpers
{
    public class JwtAuthenticationHandler : AuthenticationHandler<CustomOption>
    {
        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            Request.Cookies.
        }
    }
    public class CustomOption:AuthenticationSchemeOptions
    {

    }
}
