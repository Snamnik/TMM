namespace TMM.Application.Commands
{
    public record DeleteAddressCommand(int CustomerId, int AddressId) : IRequest;

    public class DeleteAddressCommandValidator : AbstractValidator<DeleteAddressCommand>
    {
        public DeleteAddressCommandValidator()
        {
            RuleFor(c => c.CustomerId).GreaterThan(0);
            RuleFor(c => c.AddressId).GreaterThan(0);
        }
    }

    public class DeleteAddressCommandHandler : IRequestHandler<DeleteAddressCommand>
    {
        private readonly IRepository<Customer> _customerRepository;

        public DeleteAddressCommandHandler(IRepository<Customer> customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task Handle(DeleteAddressCommand request, CancellationToken cancellationToken)
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

            customer.RemoveAddress(address);
            await _customerRepository.SaveChangesAsync(cancellationToken);
        }
    }
}
