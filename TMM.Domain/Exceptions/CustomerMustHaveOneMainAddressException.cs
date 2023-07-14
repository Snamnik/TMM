namespace TMM.Domain.Exceptions
{
    public class CustomerMustHaveOneMainAddressException : TMMException
    {
        public CustomerMustHaveOneMainAddressException(int customerId) :
            base($"the customer{customerId} must have one main address")
        { }
    }
}
