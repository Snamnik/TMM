namespace TMM.Domain.Specifications
{
    //Keep and encapsulate data access query logic in the domain layer, and reuse them.
    //http://specification.ardalis.com/

    public class CustomerSpec : Specification<Customer>, ISingleResultSpecification
    {
        public CustomerSpec(string mobileNo, string emailAddress)
        {
            Query.Where(c => c.MobileNo == mobileNo || c.EmailAddress == emailAddress);
        }

        public CustomerSpec(string mobileNo)
        {
            Query.Where(c => c.MobileNo == mobileNo && c.IsActive);
        }
    }


}
