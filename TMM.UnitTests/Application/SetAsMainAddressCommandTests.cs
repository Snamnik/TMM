namespace TMM.UnitTests.Application
{
    public class SetAsMainAddressCommandTests
    {
        [Test]
        public async Task SetAsMainAddress_SuccessScenario()
        {
            // Arrange
            int customerId = 1;
            int addressId = 2;

            string title = "Mr.", forename = "Saman", surname = "Namnik", emailAddress = "Saman.Namnik@gmail.com", mobileNo = "+989302794244";
            string addressLine1 = "No 36, Northen Bahar street", addressLine2 = "Taleghani avenue", town = "Tehran", county = "Tehran", postcode = "1234569875", country = "Iran";

            var address_1 = new MoqAddress(addressLine1, addressLine2, town, county, postcode, country);
            address_1.SetId(1);

            var address_2 = new MoqAddress(addressLine1, addressLine2, town, county, postcode, country);
            address_2.SetId(addressId);

            var customer = new Customer(title, forename, surname, emailAddress, mobileNo, address_1);
            customer.AddAddress(address_2);

            SetAsMainAddressCommand command = new SetAsMainAddressCommand(customerId, addressId);

            var mockCustomerRepository = new Mock<IRepository<Customer>>();
            mockCustomerRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>(), CancellationToken.None)).ReturnsAsync(customer);

            var handler = new SetAsMainAddressCommandHandler(mockCustomerRepository.Object);

            // Act & Assert
            Assert.DoesNotThrowAsync(() => handler.Handle(command, CancellationToken.None));
            Assert.That(customer.MainAddress.Id, Is.EqualTo(addressId));
        }

        [Test]
        public async Task SetAsMainAddress_MainAddressIsTheSameAsBeforeException()
        {
            // Arrange
            int customerId = 1;
            int addressId = 1;

            string title = "Mr.", forename = "Saman", surname = "Namnik", emailAddress = "Saman.Namnik@gmail.com", mobileNo = "+989302794244";
            string addressLine1 = "No 36, Northen Bahar street", addressLine2 = "Taleghani avenue", town = "Tehran", county = "Tehran", postcode = "1234569875", country = "Iran";

            var address_1 = new MoqAddress(addressLine1, addressLine2, town, county, postcode, country);
            address_1.SetId(1);

            var customer = new Customer(title, forename, surname, emailAddress, mobileNo, address_1);
            customer.AddAddress(address_1);

            SetAsMainAddressCommand command = new SetAsMainAddressCommand(customerId, addressId);

            var mockCustomerRepository = new Mock<IRepository<Customer>>();
            mockCustomerRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>(), CancellationToken.None)).ReturnsAsync(customer);

            var handler = new SetAsMainAddressCommandHandler(mockCustomerRepository.Object);

            // Act & Assert
            Assert.ThrowsAsync<MainAddressIsTheSameAsBeforeException>(async () => await handler.Handle(command, CancellationToken.None));
        }

        [Test]
        public async Task SetAsMainAddress_CustomerDoesNotExistException()
        {
            //Arrange 
            int customerId = 1;
            int addressId = 2;
            SetAsMainAddressCommand command = new SetAsMainAddressCommand(customerId, addressId);

            var mockCustomerRepository = new Mock<IRepository<Customer>>();
            mockCustomerRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>(), CancellationToken.None)).ReturnsAsync((Customer)null);

            var handler = new SetAsMainAddressCommandHandler(mockCustomerRepository.Object);

            //Act and Assert
            Assert.ThrowsAsync<CustomerDoesNotExistException>(async () => await handler.Handle(command, CancellationToken.None));
        }

        [Test]
        public async Task SetAsMainAddress_CustomerIsInactiveException()
        {
            // Arrange
            int customerId = 1;
            int addressId = 2;

            string title = "Mr.", forename = "Saman", surname = "Namnik", emailAddress = "Saman.Namnik@gmail.com", mobileNo = "+989302794244";
            string addressLine1 = "No 36, Northen Bahar street", addressLine2 = "Taleghani avenue", town = "Tehran", county = "Tehran", postcode = "1234569875", country = "Iran";

            var customer = new Customer(title, forename, surname, emailAddress, mobileNo, new Address(addressLine1, addressLine2, town, county, postcode, country));

            SetAsMainAddressCommand command = new SetAsMainAddressCommand(customerId, addressId);

            var mockCustomerRepository = new Mock<IRepository<Customer>>();
            mockCustomerRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>(), CancellationToken.None)).ReturnsAsync(customer);

            var handler = new SetAsMainAddressCommandHandler(mockCustomerRepository.Object);
            customer.Deactivate();

            // Act & Assert
            Assert.ThrowsAsync<CustomerIsInactiveException>(async () => await handler.Handle(command, CancellationToken.None));
        }

        [Test]
        public async Task SetAsMainAddress_AddressDoesNotExistException()
        {
            // Arrange
            int customerId = 1;
            int addressId = 2;

            string title = "Mr.", forename = "Saman", surname = "Namnik", emailAddress = "Saman.Namnik@gmail.com", mobileNo = "+989302794244";
            string addressLine1 = "No 36, Northen Bahar street", addressLine2 = "Taleghani avenue", town = "Tehran", county = "Tehran", postcode = "1234569875", country = "Iran";

            var customer = new Customer(title, forename, surname, emailAddress, mobileNo, new Address(addressLine1, addressLine2, town, county, postcode, country));

            SetAsMainAddressCommand command = new SetAsMainAddressCommand(customerId, addressId);

            var mockCustomerRepository = new Mock<IRepository<Customer>>();
            mockCustomerRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>(), CancellationToken.None)).ReturnsAsync(customer);

            var handler = new SetAsMainAddressCommandHandler(mockCustomerRepository.Object);

            // Act & Assert
            Assert.ThrowsAsync<AddressDoesNotExistException>(async () => await handler.Handle(command, CancellationToken.None));
        }

        [Test]
        public void SetAsMainAddress_InputValidation_Valid()
        {
            // Arrange
            int customerId = 1;
            int addressId = 2;

            SetAsMainAddressCommand command = new SetAsMainAddressCommand(customerId, addressId);

            var validator = new SetAsMainAddressCommandValidator();


            // Act
            var result = validator.Validate(command);

            // Assert
            Assert.IsTrue(result.IsValid);
        }

        [Test]
        public void SetAsMainAddress_InputValidation_Invalid()
        {
            // Arrange
            int customerId = 0;
            int addressId = 0;

            SetAsMainAddressCommand command = new SetAsMainAddressCommand(customerId, addressId);

            var validator = new SetAsMainAddressCommandValidator();

            // Act
            var result = validator.Validate(command);

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.That(result.Errors.Count, Is.EqualTo(2));
        }

    }
}
