namespace TMM.Domain.Exceptions
{
    public class CustomerMobileNumberExistsException : TMMException
    {
        public CustomerMobileNumberExistsException(string mobileNo) :
            base($"a customer exists with mobile number {mobileNo}")
        { }
    }
}
