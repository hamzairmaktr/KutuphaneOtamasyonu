using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.JSInterop;

namespace IKitaplik.BlazorUI.Helpers
{
    public class AuthMessageHandler : DelegatingHandler
    {
        JwtAuthenticationStateProvider _jwt;
        IHttpContextAccessor HttpContext;
        public AuthMessageHandler(JwtAuthenticationStateProvider jwt, IHttpContextAccessor httpContext)
        {
            _jwt = jwt;
            HttpContext = httpContext;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            try
            {
                //var token = _jwt._jwtToken ?? "";
                var token = HttpContext.HttpContext?.Session.GetString("authToken");
                if (!string.IsNullOrWhiteSpace(token))
                    request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                return await base.SendAsync(request, cancellationToken);

            }
            catch (Exception)
            {
                return new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized)
                {
                    Content = new StringContent("Unauthorized access. Please log in again.")
                };
            }
        }
    }
}
