namespace TMM.Application.Commands
{
    public record AddAddressCommand(int CustomerId, AddAddressDTO Address) : IRequest<AddressDTO>;

    public class AddAddressCommandValidator : AbstractValidator<AddAddressCommand>
    {
        public AddAddressCommandValidator()
        {
            RuleFor(c => c.CustomerId).GreaterThan(0);
            RuleFor(c => c.Address).NotNull().SetValidator(new AddressDTOValidator());
        }
    }

    public class AddressDTOValidator : AbstractValidator<AddAddressDTO>
    {
        public AddressDTOValidator()
        {
            RuleFor(a => a.AddressLine1).NotEmpty().MaximumLength(80);
            RuleFor(a => a.AddressLine2).MaximumLength(80);
            RuleFor(a => a.Town).NotEmpty().MaximumLength(50);
            RuleFor(a => a.County).MaximumLength(50);
            RuleFor(a => a.Postcode).NotEmpty().MaximumLength(10);
            RuleFor(a => a.Country).MaximumLength(50);
        }
    }

    public class AddAddressCommandHandler : IRequestHandler<AddAddressCommand, AddressDTO>
    {
        private readonly IRepository<Customer> _customerRepository;

        public AddAddressCommandHandler(IRepository<Customer> customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<AddressDTO> Handle(AddAddressCommand request, CancellationToken cancellationToken)
        {
            var address = request.Address;
            var customer = await _customerRepository.GetByIdAsync(request.CustomerId, cancellationToken);

            if (customer == null)
            {
                throw new CustomerDoesNotExistException(request.CustomerId);
            }

            if (!customer.IsActive)
            {
                throw new CustomerIsInactiveException(request.CustomerId);
            }

            if (customer.GetAddress(address.Country, address.Postcode) != null)
            {
                throw new AddressExistsException(request.CustomerId, address.Country, address.Postcode);
            }

            var newAddress = new Address(address.AddressLine1, address.AddressLine2, address.Town, address.County, address.Postcode, address.Country);
            customer.AddAddress(newAddress);
            await _customerRepository.SaveChangesAsync(cancellationToken);

            return newAddress.Map();
        }
    }
}
