namespace TMM.UnitTests.Application
{
    public class AddAddressCommandTests
    {

        [Test]
        public async Task AddAddress_SuccessScenario()
        {
            // Arrange
            int customerId = 1;
            string title = "Sam", forename = "Saman", surname = "Namnik", emailAddress = "Saman.Namnik@gmail.com", mobileNo = "+989302794244";
            string addressLine1 = "No 36, Northen Bahar street", addressLine2 = "Taleghani avenue", town = "Tehran",
                county = "Tehran", postcode = "1234569875", country = "Iran", newPostCode = "9874569875";

            AddAddressCommand command = new AddAddressCommand(customerId, new
                AddAddressDTO(addressLine1, addressLine2, town, county, newPostCode, country));

            var customer = new Customer(title, forename, surname, emailAddress, mobileNo, new
                 Address(addressLine1, addressLine2, town, county, postcode, country));

            var mockCustomerRepository = new Mock<IRepository<Customer>>();
            mockCustomerRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>(), CancellationToken.None)).ReturnsAsync(customer);

            var handler = new AddAddressCommandHandler(mockCustomerRepository.Object);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.That(result.AddressLine1, Is.EqualTo(command.Address.AddressLine1));
            Assert.That(result.AddressLine2, Is.EqualTo(command.Address.AddressLine2));
            Assert.That(result.County, Is.EqualTo(command.Address.County));
            Assert.That(result.Country, Is.EqualTo(command.Address.Country));
            Assert.That(result.Town, Is.EqualTo(command.Address.Town));
            Assert.That(result.Postcode, Is.EqualTo(command.Address.Postcode));
        }

        [Test]
        public async Task AddAddress__CustomerDoesNotExistException()
        {
            //Arrange 
            int customerId = 1;
            string addressLine1 = "No 36, Northen Bahar street", addressLine2 = "Taleghani avenue", town = "Tehran",
                county = "Tehran", postcode = "1234569875", country = "Iran", newPostCode = "9874569875";

            AddAddressCommand command = new AddAddressCommand(customerId, new
                AddAddressDTO(addressLine1, addressLine2, town, county, newPostCode, country));

            var mockCustomerRepository = new Mock<IRepository<Customer>>();
            mockCustomerRepository.Setup(x => x.FirstOrDefaultAsync(It.IsAny<ISpecification<Customer>>(), CancellationToken.None))
                .ReturnsAsync((Customer)null);

            var handler = new AddAddressCommandHandler(mockCustomerRepository.Object);

            //Act and Assert
            Assert.ThrowsAsync<CustomerDoesNotExistException>(async () => await handler.Handle(command, CancellationToken.None));
        }

        [Test]
        public async Task AddAddress_CustomerIsInactiveException()
        {
            //Arrange 
            int customerId = 1;
            string title = "Sam", forename = "Saman", surname = "Namnik", emailAddress = "Saman.Namnik@gmail.com", mobileNo = "+989302794244";
            string addressLine1 = "No 36, Northen Bahar street", addressLine2 = "Taleghani avenue", town = "Tehran",
                county = "Tehran", postcode = "1234569875", country = "Iran", newPostCode = "9874569875";

            AddAddressCommand command = new AddAddressCommand(customerId, new
                AddAddressDTO(addressLine1, addressLine2, town, county, newPostCode, country));

            var customer = new Customer(title, forename, surname, emailAddress, mobileNo, new
                 Address(addressLine1, addressLine2, town, county, postcode, country));

            var mockCustomerRepository = new Mock<IRepository<Customer>>();
            mockCustomerRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>(), CancellationToken.None)).ReturnsAsync(customer);

            var handler = new AddAddressCommandHandler(mockCustomerRepository.Object);

            customer.Deactivate();

            // Act & Assert
            Assert.ThrowsAsync<CustomerIsInactiveException>(async () => await handler.Handle(command, CancellationToken.None));
        }

        [Test]
        public async Task AddAddress_AddressExistsException()
        {
            //Arrange 
            int customerId = 1;
            string title = "Sam", forename = "Saman", surname = "Namnik", emailAddress = "Saman.Namnik@gmail.com", mobileNo = "+989302794244";
            string addressLine1 = "No 36, Northen Bahar street", addressLine2 = "Taleghani avenue", town = "Tehran",
                county = "Tehran", postcode = "1234569875", country = "Iran";

            AddAddressCommand command = new AddAddressCommand(customerId, new
                AddAddressDTO(addressLine1, addressLine2, town, county, postcode, country));

            var customer = new Customer(title, forename, surname, emailAddress, mobileNo, new
                 Address(addressLine1, addressLine2, town, county, postcode, country));

            var mockCustomerRepository = new Mock<IRepository<Customer>>();
            mockCustomerRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>(), CancellationToken.None)).ReturnsAsync(customer);

            var handler = new AddAddressCommandHandler(mockCustomerRepository.Object);

            // Act & Assert
            Assert.ThrowsAsync<AddressExistsException>(async () => await handler.Handle(command, CancellationToken.None));
        }

        [Test]
        public void AddAddress_InputValidation_Valid()
        {
            // Arrange
            int customerId = 1;
            string addressLine1 = "No 36, Northen Bahar street", addressLine2 = "Taleghani avenue", town = "Tehran", county = "Tehran", postcode = "1234569875", country = "Iran";

            AddAddressCommand command = new AddAddressCommand(customerId, new
                AddAddressDTO(addressLine1, addressLine2, town, county, postcode, country));

            var validator = new AddAddressCommandValidator();


            // Act
            var result = validator.Validate(command);

            // Assert
            Assert.IsTrue(result.IsValid);
        }

        [Test]
        public void AddAddress_InputValidation_Invalid()
        {
            // Arrange
            int customerId = 0;
            string addressLine1 = "", addressLine2 = "", town = "", county = "Tehran", postcode = "", country = "";

            AddAddressCommand command = new AddAddressCommand(customerId, new
                AddAddressDTO(addressLine1, addressLine2, town, county, postcode, country));

            var validator = new AddAddressCommandValidator();

            // Act
            var result = validator.Validate(command);

            // Assert
            Assert.IsFalse(result.IsValid); //With result.Errors object, can have different assertions.
        }

    }
}
