namespace TMM.UnitTests.Application
{
    public class DeactivateCustomerCommandTests
    {

        [Test]
        public async Task DeactivateCustomer_SuccessScenario()
        {
            // Arrange
            string title = "Sam", forename = "Saman", surname = "Namnik", emailAddress = "Saman.Namnik@gmail.com", mobileNo = "+989302794244";
            string addressLine1 = "No 36, Northen Bahar street", addressLine2 = "Taleghani avenue", town = "Tehran", county = "Tehran", postcode = "1234569875", country = "Iran";

            var customer = new Customer(title, forename, surname, emailAddress, mobileNo, new
                Address(addressLine1, addressLine2, town, county, postcode, country));

            DeactivateCustomerCommand command = new DeactivateCustomerCommand();

            var mockCustomerRepository = new Mock<IRepository<Customer>>();
            var mockCurrentUser = new Mock<ICurrentUser>();

            mockCustomerRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>(), CancellationToken.None)).ReturnsAsync(customer);

            var handler = new DeactivateCustomerCommandHandler(mockCustomerRepository.Object, mockCurrentUser.Object);

            // Act & Assert
            Assert.DoesNotThrowAsync(() => handler.Handle(command, CancellationToken.None));
            Assert.That(customer.IsActive, Is.EqualTo(false));
        }

        [Test]
        public async Task DeactivateCustomer_CustomerDoesNotExistException()
        {
            //Arrange 
            DeactivateCustomerCommand command = new DeactivateCustomerCommand();

            var mockCustomerRepository = new Mock<IRepository<Customer>>();
            var mockCurrentUser = new Mock<ICurrentUser>();

            mockCustomerRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>(), CancellationToken.None)).ReturnsAsync((Customer)null);

            var handler = new DeactivateCustomerCommandHandler(mockCustomerRepository.Object, mockCurrentUser.Object);

            //Act and Assert
            Assert.ThrowsAsync<CustomerDoesNotExistException>(async () => await handler.Handle(command, CancellationToken.None));
        }

        [Test]
        public async Task DeactivateCustomer_CustomerIsInactiveException()
        {
            // Arrange
            string title = "Sam", forename = "Saman", surname = "Namnik", emailAddress = "Saman.Namnik@gmail.com", mobileNo = "+989302794244";
            string addressLine1 = "No 36, Northen Bahar street", addressLine2 = "Taleghani avenue", town = "Tehran", county = "Tehran", postcode = "1234569875", country = "Iran";

            var customer = new Customer(title, forename, surname, emailAddress, mobileNo, new
                Address(addressLine1, addressLine2, town, county, postcode, country));
            
            DeactivateCustomerCommand command = new DeactivateCustomerCommand();

            var mockCustomerRepository = new Mock<IRepository<Customer>>();
            var mockCurrentUser = new Mock<ICurrentUser>();

            mockCustomerRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>(), CancellationToken.None)).ReturnsAsync(customer);

            var handler = new DeactivateCustomerCommandHandler(mockCustomerRepository.Object, mockCurrentUser.Object);
            customer.Deactivate();


            // Act & Assert
            Assert.ThrowsAsync<CustomerIsInactiveException>(async () => await handler.Handle(command, CancellationToken.None));

        }
    }
}
