namespace TMM.Application.Commands
{
    public record SetAsMainAddressCommand(int CustomerId, int AddressId) : IRequest;

    public class SetAsMainAddressCommandValidator : AbstractValidator<SetAsMainAddressCommand>
    {
        public SetAsMainAddressCommandValidator()
        {
            RuleFor(c => c.CustomerId).GreaterThan(0);
            RuleFor(c => c.AddressId).GreaterThan(0);
        }
    }

    public class SetAsMainAddressCommandHandler : IRequestHandler<SetAsMainAddressCommand>
    {
        private readonly IRepository<Customer> _customerRepository;

        public SetAsMainAddressCommandHandler(IRepository<Customer> customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task Handle(SetAsMainAddressCommand request, CancellationToken cancellationToken)
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

            var address = customer.GetAddress(request.AddressId);
            if (address == null)
            {
                throw new AddressDoesNotExistException(request.CustomerId, request.AddressId);
            }

            customer.SetAsMainAddress(address);
            await _customerRepository.SaveChangesAsync(cancellationToken);
        }
    }
}
