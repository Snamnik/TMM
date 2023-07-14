namespace TMM.Domain.Exceptions
{
    public class CustomerMustHaveAtLeastOneAddressException : TMMException
    {
        public CustomerMustHaveAtLeastOneAddressException(int customerId) :
            base($"the customer{customerId} must have at least one address")
        { }
    }
}
