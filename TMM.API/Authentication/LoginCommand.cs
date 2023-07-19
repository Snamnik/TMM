using Microsoft.Extensions.Options;

namespace TMM.API.Authentication
{
    public record LoginDTO(string MobileNo, string Password);

    public record LoginCommand(LoginDTO LoginDTO) : IRequest<TokenDTO>;

    public class AddAddressCommandValidator : AbstractValidator<LoginCommand>
    {
        public AddAddressCommandValidator()
        {
            RuleFor(c => c.LoginDTO).NotNull().SetValidator(new LoginDTOValidator());
        }
    }

    public class LoginDTOValidator : AbstractValidator<LoginDTO>
    {
        public LoginDTOValidator()
        {
            RuleFor(c => c.MobileNo).NotEmpty().Matches(ExpressionPattern.MobileNo);
            RuleFor(c => c.Password).NotEmpty().MaximumLength(50);
        }
    }

    public class LoginCommandHandler : IRequestHandler<LoginCommand, TokenDTO>
    {
        private readonly IReadOnlyRepository<Customer> _customerRepository;
        private readonly ITokenService _tokenService;
        private readonly List<Admin> _admins;

        public LoginCommandHandler(IReadOnlyRepository<Customer> customerRepository, ITokenService tokenService, IOptionsMonitor<List<Admin>> admins)
        {
            _customerRepository = customerRepository;
            _tokenService = tokenService;
            _admins = admins.CurrentValue;
        }

        public async Task<TokenDTO> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var admin = _admins.Where(admin => admin.MobileNo == request.LoginDTO.MobileNo && admin.Password.ToLower() == request.LoginDTO.Password.ToLower()).FirstOrDefault();
            if (admin != null)
            {
                return _tokenService.GenerateJWT(admin.UserId, request.LoginDTO.MobileNo, Roles.Admin);
            }

            var customer = await _customerRepository.FirstOrDefaultAsync(new CustomerSpec(request.LoginDTO.MobileNo), cancellationToken);
            if (customer != null && customer.Forename.ToLower() == request.LoginDTO.Password.ToLower())
            {
                return _tokenService.GenerateJWT(customer.Id, request.LoginDTO.MobileNo, Roles.Customer);
            }

            throw new LoginFailedException(request.LoginDTO.MobileNo);
        }
    }
}
