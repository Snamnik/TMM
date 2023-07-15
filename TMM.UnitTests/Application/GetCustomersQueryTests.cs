using FluentValidation;

namespace TMM.UnitTests.Application
{
    public class GetCustomersQueryTests
    {

        [Test]
        public async Task GetCustomers_SuccessScenario()
        {
            // Arrange
            int lastCustomerId = 0, size = 10;
            string title = "Sam", forename = "Saman", surname = "Namnik", emailAddress = "Saman.Namnik@gmail.com", mobileNo = "+989302794244";
            string addressLine1 = "No 36, Northen Bahar street", addressLine2 = "Taleghani avenue", town = "Tehran", county = "Tehran", postcode = "1234569875", country = "Iran";

            string title_2 = "Sam2", forename_2 = "Saman2", surname_2 = "Namnik2", emailAddress_2 = "Saman.Namnik2@gmail.com", mobileNo_2 = "+989302794242";
            string addressLine1_2 = "No 36, Northen Bahar street2", addressLine2_2 = "Taleghani avenue2", town_2 = "Tehran2", county_2 = "Tehran2", postcode_2 = "1234569872", country_2 = "Iran2";

            var customer_1 = new Customer(title, forename, surname, emailAddress, mobileNo, new
                Address(addressLine1, addressLine2, town, county, postcode, country));

            var customer_2 = new Customer(title_2, forename_2, surname_2, emailAddress_2, mobileNo_2, new
                Address(addressLine1_2, addressLine2_2, town_2, county_2, postcode_2, country_2));

            customer_2.Deactivate();

            GetCustomersQuery CustomersQuery_all = new GetCustomersQuery(lastCustomerId, size, CustomerState.All);

            var mockCustomerRepository = new Mock<IReadOnlyRepository<Customer>>();

            mockCustomerRepository.Setup(x => x.ListAsync(It.IsAny<ISpecification<Customer>>(), CancellationToken.None)).ReturnsAsync(
                new List<Customer> { customer_1, customer_2 });

            // Act
            var result = await new GetCustomersQueryHandler(mockCustomerRepository.Object).Handle(CustomersQuery_all, CancellationToken.None);


            // Assert
            Assert.That(result.Count, Is.EqualTo(2));

            var item_1 = result.FirstOrDefault(c => c.MobileNo == mobileNo);

            Assert.That(item_1.MobileNo, Is.EqualTo(customer_1.MobileNo));
            Assert.That(item_1.IsActive, Is.EqualTo(customer_1.IsActive));
            Assert.That(item_1.Title, Is.EqualTo(customer_1.Title));
            Assert.That(item_1.EmailAddress, Is.EqualTo(customer_1.EmailAddress));
            Assert.That(item_1.Forename, Is.EqualTo(customer_1.Forename));
            Assert.That(item_1.Surname, Is.EqualTo(customer_1.Surname));

            var item_2 = result.FirstOrDefault(c => c.MobileNo == mobileNo_2);

            Assert.That(item_2.MobileNo, Is.EqualTo(customer_2.MobileNo));
            Assert.That(item_2.IsActive, Is.EqualTo(customer_2.IsActive));
            Assert.That(item_2.Title, Is.EqualTo(customer_2.Title));
            Assert.That(item_2.EmailAddress, Is.EqualTo(customer_2.EmailAddress));
            Assert.That(item_2.Forename, Is.EqualTo(customer_2.Forename));
            Assert.That(item_2.Surname, Is.EqualTo(customer_2.Surname));
        }

        [Test]
        public void GetCustomers_InputValidation_Valid()
        {
            // Arrange
            int lastCustomerId = 1;
            int size = 10;
            CustomerState customerState = CustomerState.Active;

            GetCustomersQuery query = new GetCustomersQuery(lastCustomerId, size, customerState);
            var validator = new GetCustomersQueryValidator();

            // Act
            var result = validator.Validate(query);

            // Assert
            Assert.IsTrue(result.IsValid);
        }

        [Test]
        public void GetCustomers_InputValidation_Invalid()
        {
            // Arrange
            int lastCustomerId = 0;
            int size = 0;
            CustomerState customerState = (CustomerState)5;

            GetCustomersQuery query = new GetCustomersQuery(lastCustomerId, size, customerState);
            var validator = new GetCustomersQueryValidator();

            // Act
            var result = validator.Validate(query);

            // Assert
            Assert.IsFalse(result.IsValid);
        }
    }
}
