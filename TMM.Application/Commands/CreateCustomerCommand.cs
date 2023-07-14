namespace TMM.Application.Commands
{
    public record CreateCustomerCommand(string Title, string Forename, string Surname, string EmailAddress, string MobileNo, AddAddressDTO Address)
        : IRequest<CretaeCustomerDTO>;

    public class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
    {
        public CreateCustomerCommandValidator()
        {
            RuleFor(c => c.Title).NotEmpty().MaximumLength(20);
            RuleFor(c => c.Forename).NotEmpty().MaximumLength(50);
            RuleFor(c => c.Surname).NotEmpty().MaximumLength(50);
            RuleFor(c => c.EmailAddress).NotEmpty().MaximumLength(75).EmailAddress();
            RuleFor(c => c.MobileNo).NotEmpty().Matches(ExpressionPattern.MobileNo);
            RuleFor(c => c.Address).NotNull().SetValidator(new AddressDTOValidator());
        }
    }

    public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, CretaeCustomerDTO>
    {
        private readonly IRepository<Customer> _customerRepository;

        public CreateCustomerCommandHandler(IRepository<Customer> customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<CretaeCustomerDTO> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = await _customerRepository.FirstOrDefaultAsync(new CustomerSpec(request.MobileNo, request.EmailAddress), cancellationToken);
            if (customer != null)
            {
                if (customer.MobileNo == request.MobileNo)
                    throw new CustomerMobileNumberExistsException(request.MobileNo);

                else if (customer.EmailAddress == request.EmailAddress)
                    throw new CustomerEmailAddressExistsException(request.EmailAddress);
            }

            var newCustomer = new Customer(request.Title, request.Forename, request.Surname, request.EmailAddress, request.MobileNo, request.Address.Map());

            await _customerRepository.AddAsync(newCustomer, cancellationToken);
            await _customerRepository.SaveChangesAsync(cancellationToken);

            return newCustomer.Map();
        }
    }
}
