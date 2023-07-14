namespace TMM.Domain.Exceptions
{
    public class AddressDoesNotExistException : TMMException
    {
        public AddressDoesNotExistException(int customerId, int addressId) :
            base($"for the customer {customerId} doesn't exist an address with id {addressId}.")
        { }
    }
}
