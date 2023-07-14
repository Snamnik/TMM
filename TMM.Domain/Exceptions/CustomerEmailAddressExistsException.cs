namespace TMM.Domain.Exceptions
{
    public class CustomerEmailAddressExistsException : TMMException
    {
        public CustomerEmailAddressExistsException(string emailAddress) :
            base($"a customer exists with email address {emailAddress}")
        { }
    }
}
