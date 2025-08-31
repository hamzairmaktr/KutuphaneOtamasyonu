namespace IKitaplik.BlazorUI.Helpers
{
    public class AccessTokenService
    {
        private readonly CookieService _cookieService;
        private readonly string token_key = "access_token";
        public AccessTokenService(CookieService cookieService)
        {
            _cookieService = cookieService;
        }

        public async Task SetToken(string accessToken) => await _cookieService.Set(token_key, accessToken, 1);
        public async Task<string> GetToken() => await _cookieService.Get(token_key);
        public async Task RemoveToken() => await _cookieService.Remove(token_key);
    }
}
