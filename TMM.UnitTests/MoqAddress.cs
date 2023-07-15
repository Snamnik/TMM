namespace TMM.UnitTests
{
    public class MoqAddress : Address
    {
        public MoqAddress(string addressLine1, string addressLine2, string town, string county, string postcode, string country)
            : base(addressLine1, addressLine2, town, county, postcode, country)
        {
        }
        public void SetIdJustForUnitTest(int id) => Id = id;
    }
}
