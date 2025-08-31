using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace IKitaplik.BlazorUI.Helpers
{
    public class RefreshTokenService
    {
        private readonly ProtectedLocalStorage protectedLocalStorage;
        private readonly string token_key = "refresh_token";
        public RefreshTokenService(ProtectedLocalStorage protectedLocalStorage)
        {
            this.protectedLocalStorage = protectedLocalStorage;
        }

        public async Task SetToken(string refreshToken) => await protectedLocalStorage.SetAsync(token_key, refreshToken);
        public async Task<string> GetToken()
        {
            var res =  await protectedLocalStorage.GetAsync<string>(token_key);
            return res.Success ? res.Value : string.Empty;
        }
        public async Task RemoveToken() => await protectedLocalStorage.DeleteAsync(token_key);
    }
}
