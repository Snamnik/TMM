namespace TMM.Domain
{
    public class Address : Entity
    {
        public string AddressLine1 { get; private set; }
        public string AddressLine2 { get; private set; }
        public string Town { get; private set; }
        public string County { get; private set; }
        public string Postcode { get; private set; }
        public string Country { get; private set; }
        public bool IsMain { get; private set; }

        private Address()
        {
            IsMain = false;
        }

        public Address(string addressLine1, string addressLine2, string town, string county, string postcode, string country) : this()
        {
            AddressLine1 = addressLine1;
            AddressLine2 = addressLine2;
            Town = town;
            County = county;
            Postcode = postcode;
            Country = string.IsNullOrWhiteSpace(country) ? "UK" : country;
        }

        public void SetAsMainAddress() => IsMain = true;
        public void SetAsSecondaryAddress() => IsMain = false;
    }
}