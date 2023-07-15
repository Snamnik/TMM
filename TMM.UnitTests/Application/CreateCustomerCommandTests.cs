namespace TMM.UnitTests.Application
{
    public class CreateCustomerCommandTests
    {
        [Test]
        public async Task CreateCustomer_SuccessScenario()
        {
            // Arrange
            string title = "Sam", forename = "Saman", surname = "Namnik", emailAddress = "Saman.Namnik@gmail.com", mobileNo = "+989302794244";
            string addressLine1 = "No 36, Northen Bahar street", addressLine2 = "Taleghani avenue", town = "Tehran", county = "Tehran",
                postcode = "1234569875", country = "Iran";

            CreateCustomerCommand command = new CreateCustomerCommand(title, forename, surname, emailAddress, mobileNo, new
                AddAddressDTO(addressLine1, addressLine2, town, county, postcode, country));

            var mockCustomerRepository = new Mock<IRepository<Customer>>();
            mockCustomerRepository.Setup(x => x.FirstOrDefaultAsync(It.IsAny<ISpecification<Customer>>(), CancellationToken.None))
                .ReturnsAsync((Customer)null);

            var handler = new CreateCustomerCommandHandler(mockCustomerRepository.Object);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.That(result.Title, Is.EqualTo(command.Title));
            Assert.That(result.Surname, Is.EqualTo(command.Surname));
            Assert.That(result.Forename, Is.EqualTo(command.Forename));
            Assert.That(result.MobileNo, Is.EqualTo(command.MobileNo));
            Assert.That(result.Address.AddressLine1, Is.EqualTo(command.Address.AddressLine1));
            Assert.That(result.Address.AddressLine2, Is.EqualTo(command.Address.AddressLine2));
            Assert.That(result.Address.County, Is.EqualTo(command.Address.County));
            Assert.That(result.Address.Country, Is.EqualTo(command.Address.Country));
            Assert.That(result.Address.Town, Is.EqualTo(command.Address.Town));
            Assert.That(result.Address.Postcode, Is.EqualTo(command.Address.Postcode));
        }

        [Test]
        public async Task CreateCustomer_CustomerMobileNoExistsException()
        {
            //Arrange 
            string title = "Sam", forename = "Saman", surname = "Namnik", emailAddress = "Saman.Namnik@gmail.com", mobileNo = "+989302794244";
            string addressLine1 = "No 36, Northen Bahar street", addressLine2 = "Taleghani avenue", town = "Tehran", county = "Tehran", postcode = "1234569875", country = "Iran";

            CreateCustomerCommand command = new CreateCustomerCommand(title, forename, surname, emailAddress, mobileNo, new
                AddAddressDTO(addressLine1, addressLine2, town, county, postcode, country));

            var mockCustomerRepository = new Mock<IRepository<Customer>>();
            mockCustomerRepository.Setup(x => x.FirstOrDefaultAsync(It.IsAny<ISpecification<Customer>>(), CancellationToken.None))
                .ReturnsAsync(new Customer(title, forename, surname, emailAddress, mobileNo,
                new Address(addressLine1, addressLine2, town, county, postcode, country)));

            var handler = new CreateCustomerCommandHandler(mockCustomerRepository.Object);

            //Act and Assert
            Assert.ThrowsAsync<CustomerMobileNumberExistsException>(async () => await handler.Handle(command, CancellationToken.None));
        }

        [Test]
        public async Task CreateCustomer_CustomerEmailAddressExistsException()
        {
            //Arrange 
            string title = "Sam", forename = "Saman", surname = "Namnik", emailAddress = "Saman.Namnik@gmail.com", mobileNo = "+989302794244";
            string addressLine1 = "No 36, Northen Bahar street", addressLine2 = "Taleghani avenue", town = "Tehran", county = "Tehran", postcode = "1234569875", country = "Iran";

            CreateCustomerCommand command = new CreateCustomerCommand(title, forename, surname, emailAddress, mobileNo, new
                AddAddressDTO(addressLine1, addressLine2, town, county, postcode, country));


            var mockCustomerRepository = new Mock<IRepository<Customer>>();
            mockCustomerRepository.Setup(x => x.FirstOrDefaultAsync(It.IsAny<ISpecification<Customer>>(), CancellationToken.None))
                .ReturnsAsync(new Customer(title, forename, surname, emailAddress, "+44101010101", new Address(addressLine1, addressLine2, town, county, postcode, country)));

            var handler = new CreateCustomerCommandHandler(mockCustomerRepository.Object);

            //Act and Assert
            Assert.ThrowsAsync<CustomerEmailAddressExistsException>(async () => await handler.Handle(command, CancellationToken.None));
        }

        [Test]
        public void CreateCustomer_InputValidation_Valid()
        {
            // Arrange
            string title = "Sam", forename = "Saman", surname = "Namnik", emailAddress = "Saman.Namnik@gmail.com", mobileNo = "+989302794244";
            string addressLine1 = "No 36, Northen Bahar street", addressLine2 = "Taleghani avenue", town = "Tehran", county = "Tehran", postcode = "1234569875", country = "Iran";

            CreateCustomerCommand command = new CreateCustomerCommand(title, forename, surname, emailAddress, mobileNo, new
                AddAddressDTO(addressLine1, addressLine2, town, county, postcode, country));

            var validator = new CreateCustomerCommandValidator();


            // Act
            var result = validator.Validate(command);

            // Assert
            Assert.IsTrue(result.IsValid);
        }

        [Test]
        public void CreateCustomer_InputValidation_Invalid()
        {
            // Arrange
            string title = "", forename = "", surname = "", emailAddress = "", mobileNo = "";
            string addressLine1 = "", addressLine2 = "", town = "", county = "Tehran", postcode = "", country = "";

            CreateCustomerCommand command = new CreateCustomerCommand(title, forename, surname, emailAddress, mobileNo, new
                AddAddressDTO(addressLine1, addressLine2, town, county, postcode, country));

            var validator = new CreateCustomerCommandValidator();

            // Act
            var result = validator.Validate(command);

            // Assert
            Assert.IsFalse(result.IsValid); //With result.Errors object, can have different assertions.
        }

    }
}
