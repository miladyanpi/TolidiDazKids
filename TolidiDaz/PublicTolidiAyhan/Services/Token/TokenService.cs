namespace PublicTolidiAyhan.Services.Token
{
    public class TokenService
    {
        public string? Token;
        public string? RefreshToken;
        public string? DeviceID;

        // متدی برای تنظیم توکن پس از ورود موفق
        public void SetToken(string token, string refreshToken, string deviceID)
        {
            Token = token;
            RefreshToken = refreshToken;
            DeviceID = deviceID;
        }
        public void ClearToken()
        {
            Token = null;
            RefreshToken = null;
            DeviceID = null;
        }
    }
}
