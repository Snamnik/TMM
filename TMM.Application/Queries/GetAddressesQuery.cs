namespace TMM.Application.Commands
{
    public record GetAddressesQuery(int CustomerId) : IRequest<List<AddressDTO>>;

    public class GetAddressesQueryValidator : AbstractValidator<GetAddressesQuery>
    {
        public GetAddressesQueryValidator()
        {
            RuleFor(c => c.CustomerId).GreaterThan(0);
        }
    }

    public class GetAddressesQueryHandler : IRequestHandler<GetAddressesQuery, List<AddressDTO>>
    {
        private readonly IReadOnlyRepository<Customer> _customerRepository;

        public GetAddressesQueryHandler(IReadOnlyRepository<Customer> customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<List<AddressDTO>> Handle(GetAddressesQuery request, CancellationToken cancellationToken)
        {
            var customer = await _customerRepository.GetByIdAsync(request.CustomerId, cancellationToken);

            if (customer == null)
            {
                throw new CustomerDoesNotExistException(request.CustomerId);
            }
            if (!customer.IsActive)
            {
                throw new CustomerIsInactiveException(request.CustomerId);
            }

            return customer.Addresses.Select(address => address.Map()).ToList();
        }
    }
}
