namespace TMM.Application.DTOs
{
    public record CustomerDTO(int CustomerId, string Title, string Forename, string Surname, string EmailAddress, string MobileNo, bool IsActive);
    public record CretaeCustomerDTO(int CustomerId, string Title, string Forename, string Surname, string EmailAddress, string MobileNo, AddressDTO Address);

    public static class CustomerMapper
    {
        public static CretaeCustomerDTO Map(this Customer customer) =>
            new CretaeCustomerDTO(customer.Id, customer.Title, customer.Forename,customer.Surname, customer.EmailAddress, customer.MobileNo, customer.MainAddress.Map());

        public static CustomerDTO MapToCustomerDTO(this Customer customer) =>
            new CustomerDTO(customer.Id, customer.Title, customer.Forename, customer.Surname, customer.EmailAddress, customer.MobileNo, customer.IsActive);
    }
}
