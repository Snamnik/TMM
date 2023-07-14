namespace TMM.Application.Commands
{
    public record GetCustomersQuery(int LastCustomerId, int Size, CustomerState CustomerState) : IRequest<List<CustomerDTO>>;

    public class GetCustomersQueryValidator : AbstractValidator<GetCustomersQuery>
    {
        public GetCustomersQueryValidator()
        {
            RuleFor(c => c.LastCustomerId).NotEmpty();
            RuleFor(c => c.Size).NotEmpty().GreaterThan(0).LessThanOrEqualTo(20);
            RuleFor(c => c.CustomerState).IsInEnum();
        }
    }

    public class GetCustomersQueryHandler : IRequestHandler<GetCustomersQuery, List<CustomerDTO>>
    {
        private readonly IReadOnlyRepository<Customer> _customerRepository;

        public GetCustomersQueryHandler(IReadOnlyRepository<Customer> customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<List<CustomerDTO>> Handle(GetCustomersQuery request, CancellationToken cancellationToken)
        {
            List<Customer> customers;
            switch (request.CustomerState)
            {
                case CustomerState.All:
                    customers = await _customerRepository.ListAsync(new CustomersSpec(request.LastCustomerId, request.Size), cancellationToken);
                    break;
                case CustomerState.Active:
                    customers = await _customerRepository.ListAsync(new ActiveCustomersSpec(request.LastCustomerId, request.Size), cancellationToken);

                    break;
                case CustomerState.Inactive:
                    customers = await _customerRepository.ListAsync(new InactiveCustomersSpec(request.LastCustomerId, request.Size), cancellationToken);
                    break;
                default:
                    throw new ArgumentNullException(nameof(request.CustomerState));
            }

            return customers.Select(customer => customer.MapToCustomerDTO()).ToList();
        }
    }
}
