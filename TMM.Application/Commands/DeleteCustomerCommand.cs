namespace TMM.Application.Commands
{
    public record DeleteCustomerCommand(int CustomerId) : IRequest;

    public class DeleteCustomerCommandValidator : AbstractValidator<DeleteCustomerCommand>
    {
        public DeleteCustomerCommandValidator()
        {
            RuleFor(c => c.CustomerId).GreaterThan(0);
        }
    }

    public class DeleteCustomerCommandHandler : IRequestHandler<DeleteCustomerCommand>
    {
        private readonly IRepository<Customer> _customerRepository;

        public DeleteCustomerCommandHandler(IRepository<Customer> customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = await _customerRepository.GetByIdAsync(request.CustomerId, cancellationToken);

            if (customer == null)
            {
                throw new CustomerDoesNotExistException(request.CustomerId);
            }

            await _customerRepository.DeleteAsync(customer);
            await _customerRepository.SaveChangesAsync(cancellationToken);
        }
    }
}
