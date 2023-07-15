namespace TMM.UnitTests.Application
{
    public class DeleteCustomerCommandTests
    {

        [Test]
        public async Task DeleteCustomer_SuccessScenario()
        {
            // Arrange
            int customerId = 1;
            string title = "Sam", forename = "Saman", surname = "Namnik", emailAddress = "Saman.Namnik@gmail.com", mobileNo = "+989302794244";
            string addressLine1 = "No 36, Northen Bahar street", addressLine2 = "Taleghani avenue", town = "Tehran", county = "Tehran", postcode = "1234569875", country = "Iran";

            var customer = new Customer(title, forename, surname, emailAddress, mobileNo, new
                Address(addressLine1, addressLine2, town, county, postcode, country));

            DeleteCustomerCommand command = new DeleteCustomerCommand(customerId);

            var mockCustomerRepository = new Mock<IRepository<Customer>>();
            mockCustomerRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>(), CancellationToken.None)).ReturnsAsync(customer);

            var handler = new DeleteCustomerCommandHandler(mockCustomerRepository.Object);

            // Act & Assert
            Assert.DoesNotThrowAsync(() => handler.Handle(command, CancellationToken.None));
        }

        [Test]
        public async Task DeleteCustomer_CustomerDoesNotExistException()
        {
            //Arrange 
            int customerId = 1;
            DeleteCustomerCommand command = new DeleteCustomerCommand(customerId);

            var mockCustomerRepository = new Mock<IRepository<Customer>>();
            mockCustomerRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>(), CancellationToken.None)).ReturnsAsync((Customer)null);

            var handler = new DeleteCustomerCommandHandler(mockCustomerRepository.Object);

            //Act and Assert
            Assert.ThrowsAsync<CustomerDoesNotExistException>(async () => await handler.Handle(command, CancellationToken.None));
        }

        [Test]
        public void DeleteCustomer_InputValidation_Valid()
        {
            // Arrange
            DeleteCustomerCommand command = new DeleteCustomerCommand(1);

            var validator = new DeleteCustomerCommandValidator();


            // Act
            var result = validator.Validate(command);

            // Assert
            Assert.IsTrue(result.IsValid);
        }

        [Test]
        public void DeleteCustomer_InputValidation_Invalid()
        {
            // Arrange
            DeleteCustomerCommand command = new DeleteCustomerCommand(0);

            var validator = new DeleteCustomerCommandValidator();

            // Act
            var result = validator.Validate(command);

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.That(result.Errors.Count, Is.EqualTo(1));
        }

    }
}
