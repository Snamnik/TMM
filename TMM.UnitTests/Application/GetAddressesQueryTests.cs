using FluentValidation;

namespace TMM.UnitTests.Application
{
    public class GetAddressesQueryTests
    {
        [Test]
        public async Task GetAddresses_SuccessScenario()
        {
            // Arrange
            int customerId = 1;
            int addressId = 2;

            string title = "Sam", forename = "Saman", surname = "Namnik", emailAddress = "Saman.Namnik@gmail.com", mobileNo = "+989302794244";
            string addressLine1 = "No 36, Northen Bahar street", addressLine2 = "Taleghani avenue", town = "Tehran", county = "Tehran", postcode = "1234569875", country = "Iran";
            string addressLine1_2 = "No 36, Northen Bahar street2", addressLine2_2 = "Taleghani avenue2", town_2 = "Tehran2", county_2 = "Tehran2", postcode_2 = "1234569872", country_2 = "Iran2";


            var address_1 = new MoqAddress(addressLine1, addressLine2, town, county, postcode, country);
            address_1.SetId(1);

            var address_2 = new MoqAddress(addressLine1_2, addressLine2_2, town_2, county_2, postcode_2, country_2);
            address_2.SetId(2);

            var customer = new Customer(title, forename, surname, emailAddress, mobileNo, address_1);
            customer.AddAddress(address_2);

            GetAddressesQuery query = new GetAddressesQuery(customerId);

            var mockCustomerRepository = new Mock<IReadOnlyRepository<Customer>>();
            mockCustomerRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>(), CancellationToken.None)).ReturnsAsync(customer);

            // Act
            var result = await new GetAddressesQueryHandler(mockCustomerRepository.Object).Handle(query, CancellationToken.None);


            // & Assert
            Assert.That(result.Count, Is.EqualTo(2));

            var item_1 = result.FirstOrDefault(c => c.AddressId == address_1.Id);

            Assert.That(item_1.IsMain, Is.EqualTo(address_1.IsMain));
            Assert.That(item_1.County, Is.EqualTo(address_1.County));
            Assert.That(item_1.Town, Is.EqualTo(address_1.Town));
            Assert.That(item_1.AddressLine1, Is.EqualTo(address_1.AddressLine1));
            Assert.That(item_1.AddressLine2, Is.EqualTo(address_1.AddressLine2));
            Assert.That(item_1.Country, Is.EqualTo(address_1.Country));
            Assert.That(item_1.Postcode, Is.EqualTo(address_1.Postcode));

            var item_2 = result.FirstOrDefault(c => c.AddressId == address_2.Id);

            Assert.That(item_2.IsMain, Is.EqualTo(address_2.IsMain));
            Assert.That(item_2.County, Is.EqualTo(address_2.County));
            Assert.That(item_2.Town, Is.EqualTo(address_2.Town));
            Assert.That(item_2.AddressLine1, Is.EqualTo(address_2.AddressLine1));
            Assert.That(item_2.AddressLine2, Is.EqualTo(address_2.AddressLine2));
            Assert.That(item_2.Country, Is.EqualTo(address_2.Country));
            Assert.That(item_2.Postcode, Is.EqualTo(address_2.Postcode));
        }

        [Test]
        public async Task GetAddresses_CustomerDoesNotExistException()
        {
            //Arrange 
            int customerId = 1;
            GetAddressesQuery query = new GetAddressesQuery(customerId);

            var mockCustomerRepository = new Mock<IReadOnlyRepository<Customer>>();
            mockCustomerRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>(), CancellationToken.None)).ReturnsAsync((Customer)null);

            var handler = new GetAddressesQueryHandler(mockCustomerRepository.Object);

            //Act and Assert
            Assert.ThrowsAsync<CustomerDoesNotExistException>(async () => await handler.Handle(query, CancellationToken.None));
        }

        [Test]
        public async Task GetAddresses_CustomerIsInactiveException()
        {
            // Arrange
            int customerId = 1;

            string title = "Sam", forename = "Saman", surname = "Namnik", emailAddress = "Saman.Namnik@gmail.com", mobileNo = "+989302794244";
            string addressLine1 = "No 36, Northen Bahar street", addressLine2 = "Taleghani avenue", town = "Tehran", county = "Tehran", postcode = "1234569875", country = "Iran";

            var customer = new Customer(title, forename, surname, emailAddress, mobileNo, new Address(addressLine1, addressLine2, town, county, postcode, country));

            GetAddressesQuery query = new GetAddressesQuery(customerId);

            var mockCustomerRepository = new Mock<IReadOnlyRepository<Customer>>();
            mockCustomerRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>(), CancellationToken.None)).ReturnsAsync(customer);

            var handler = new GetAddressesQueryHandler(mockCustomerRepository.Object);
            customer.Deactivate();

            // Act & Assert
            Assert.ThrowsAsync<CustomerIsInactiveException>(async () => await handler.Handle(query, CancellationToken.None));
        }


        [Test]
        public void GetAddresses_InputValidation_Valid()
        {
            // Arrange
            int customerId = 1;

            GetAddressesQuery query = new GetAddressesQuery(customerId);
            var validator = new GetAddressesQueryValidator();

            // Act
            var result = validator.Validate(query);

            // Assert
            Assert.IsTrue(result.IsValid);
        }

        [Test]
        public void GetAddresses_InputValidation_Invalid()
        {
            // Arrange
            int customerId = 0;

            GetAddressesQuery query = new GetAddressesQuery(customerId);
            var validator = new GetAddressesQueryValidator();

            // Act
            var result = validator.Validate(query);

            // Assert
            Assert.IsFalse(result.IsValid);
        }

    }
}
