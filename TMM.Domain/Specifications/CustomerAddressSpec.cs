namespace TMM.Domain.Specifications
{
    public class CustomerAddressSpec : Specification<Customer>, ISingleResultSpecification
    {
        public CustomerAddressSpec(int customerId, string Postcode)
        {
            Query.Where(c => c.Id == customerId && c.Addresses.Any(a => a.Postcode == Postcode));
        }
    }
}
