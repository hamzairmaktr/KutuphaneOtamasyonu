using Microsoft.JSInterop;

namespace IKitaplik.BlazorUI.Helpers
{
    public class CookieService
    {
        private readonly IJSRuntime jSRuntime;
        public CookieService(IJSRuntime jSRuntime)
        {
            this.jSRuntime = jSRuntime;
        }

        public async Task<string> Get(string key)
        {
            return await jSRuntime.InvokeAsync<string>("getCookie", key);
        }

        public async Task Remove(string key)
        {
            await jSRuntime.InvokeVoidAsync("deleteCookie", key);
        }

        public async Task Set(string key, string value, int days)
        {
            await jSRuntime.InvokeVoidAsync("setCookie", key, value, days);
        }
    }
}
