namespace TMM.Domain.Exceptions
{
    public class LoginFailedException : TMMException
    {
        public LoginFailedException(string mobileNo) :
            base($"There isn't a user with mobile number {mobileNo} or the given password.")
        { }
    }
}
