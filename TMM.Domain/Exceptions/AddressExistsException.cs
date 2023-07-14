namespace TMM.Domain.Exceptions
{
    public class AddressExistsException : TMMException
    {
        public AddressExistsException(int customerId, string country, string postcode) :
            base($"for the customer {customerId} exists an address with postcode {postcode} in {country}.")
        { }
    }
}
