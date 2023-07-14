namespace TMM.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        ///login to the system
        /// </summary>
        [HttpPost("login")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(TokenDTO), StatusCodes.Status200OK)]
        public Task<TokenDTO> AddAddressAsync([FromBody] LoginDTO loginDTO, CancellationToken cancellationToken)
        {
            return _mediator.Send(new LoginCommand(loginDTO), cancellationToken);
        }

    }
}