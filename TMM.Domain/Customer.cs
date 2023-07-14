namespace TMM.Domain
{
    public class Customer : Entity, IAggregateRoot
    {
        public string Title { get; private set; }
        public string Forename { get; private set; }
        public string Surname { get; private set; }
        public string EmailAddress { get; private set; }
        public string MobileNo { get; private set; }
        public bool IsActive { get; private set; }

        public Address MainAddress { get { return _addresses.FirstOrDefault(a => a.IsMain); } }

        private readonly List<Address> _addresses;
        public IReadOnlyList<Address> Addresses => _addresses;

        private Customer()
        {
            _addresses = new List<Address>();
            IsActive = true;
        }

        public Customer(string title, string forename, string surname, string emailAddress, string mobileNo, Address address) : this()
        {
            Title = title;
            Forename = forename;
            Surname = surname;
            EmailAddress = emailAddress;
            MobileNo = mobileNo;

            address.SetAsMainAddress();
            _addresses.Add(address);
        }

        public Address GetMainAddress() => _addresses.FirstOrDefault(a => a.IsMain);

        public Address GetAddress(string country, string postcode) => _addresses.FirstOrDefault(a => a.Country == country && a.Postcode == postcode);
        public Address GetAddress(int addressId) => _addresses.FirstOrDefault(a => a.Id == addressId);

        public void AddAddress(Address address) => _addresses.Add(address);

        public void RemoveAddress(Address address)
        {
            if (_addresses.Count == 1)
            {
                throw new CustomerMustHaveAtLeastOneAddressException(Id);
            }
            if (address.IsMain)
            {
                throw new CustomerMustHaveOneMainAddressException(Id);
            }

            _addresses.Remove(address);
        }

        public void SetAsMainAddress(Address address)
        {
            var currentMainAddress = GetMainAddress();

            if (currentMainAddress.Id == address.Id)
            {
                throw new MainAddressIsTheSameAsBeforeException(Id, address.Id);
            }

            currentMainAddress.SetAsSecondaryAddress();
            address.SetAsMainAddress();
        }

        public void Deactivate() => IsActive = false;
    }
}