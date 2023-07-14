namespace TMM.Domain.Exceptions
{
    public class MainAddressIsTheSameAsBeforeException : TMMException
    {
        public MainAddressIsTheSameAsBeforeException(int customerId, int addressId) :
            base($"the address is the same as before for customer {customerId} with address {addressId}.")
        { }
    }
}
