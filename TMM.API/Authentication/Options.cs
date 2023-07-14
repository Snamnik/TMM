namespace TMM.API.Authentication
{
    public class JWTOptions
    {
        public string SecretKey { set; get; }
        public string Issuer { set; get; }
        public string Audience { set; get; }
        public int AccessTokenExpiryTime { set; get; }
    }
    public class Admin
    {
        public int UserId { set; get; }
        public string MobileNo { set; get; }
        public string Password { set; get; }
    }
}
