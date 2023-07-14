namespace TMM.Application.Commands
{
    public record DeactivateCustomerCommand() : IRequest;

    public class DeactivateCustomerCommandHandler : IRequestHandler<DeactivateCustomerCommand>
    {
        private readonly IRepository<Customer> _customerRepository;
        private readonly ICurrentUser _currentUser;

        public DeactivateCustomerCommandHandler(IRepository<Customer> customerRepository, ICurrentUser currentUser)
        {
            _customerRepository = customerRepository;
            _currentUser = currentUser;
        }

        public async Task Handle(DeactivateCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = await _customerRepository.GetByIdAsync(_currentUser.UserId, cancellationToken);
            if (customer == null)
            {
                throw new CustomerDoesNotExistException(_currentUser.UserId);
            }
            if (!customer.IsActive)
            {
                throw new CustomerIsInactiveException(_currentUser.UserId);
            }

            customer.Deactivate();
            await _customerRepository.SaveChangesAsync(cancellationToken);
        }
    }
}
