namespace TMM.API.Authentication
{
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
        private readonly ITokenService _tokenService;

        public LoginCommandHandler(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        public Task<TokenDTO> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            return _tokenService.LoginAsync(request.LoginDTO, cancellationToken);
        }
    }
}
