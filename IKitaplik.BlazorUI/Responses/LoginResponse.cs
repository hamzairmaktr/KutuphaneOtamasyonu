namespace IKitaplik.BlazorUI.Responses
{
    public class LoginResponse
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public bool IsTwoFactor { get; set; }
    }
}
