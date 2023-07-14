namespace TMM.API.Controllers
{
    [ApiController]
    [Route("api/customers")]
    [Authorize(AuthenticationSchemes = AuthenticationScheme.TMMScheme)]
    public class CustomerController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CustomerController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// create a customer
        /// </summary>
        [HttpPost("")]
        [ProducesResponseType(typeof(CretaeCustomerDTO), StatusCodes.Status200OK)]
        [Authorize(Roles = Roles.Admin)]
        public Task<CretaeCustomerDTO> CreateCustomerAsync([FromBody] CreateCustomerCommand createCustomerCommand, CancellationToken cancellationToken)
        {
            return _mediator.Send(createCustomerCommand, cancellationToken);
        }

        /// <summary>
        /// delete a customer and all associated addresses
        /// </summary>
        [HttpDelete("{customerId}")]
        [Authorize(Roles = Roles.Admin)]
        public Task DeleteCustomerAsync([FromRoute] int customerId, CancellationToken cancellationToken)
        {
            return _mediator.Send(new DeleteCustomerCommand(customerId), cancellationToken);
        }


        /// <summary>
        /// get customers
        /// </summary>
        [HttpGet("{lastCustomerId}/{size}/{state}")]
        [ProducesResponseType(typeof(List<CustomerDTO>), StatusCodes.Status200OK)]
        [Authorize(Roles = Roles.Admin)]
        public Task<List<CustomerDTO>> GetCustomersAsync([FromRoute] int lastCustomerId, int size, CustomerState state = CustomerState.All, CancellationToken cancellationToken = default)
        {
            return _mediator.Send(new GetCustomersQuery(lastCustomerId, size, state), cancellationToken);
        }

        /// <summary>
        /// deactivate a customer by himself/herself
        /// </summary>
        [HttpPut("deactivate")]
        [Authorize(Roles = Roles.Customer)]
        public Task DeactivateCustomerAsync(CancellationToken cancellationToken)
        {
            return _mediator.Send(new DeactivateCustomerCommand(), cancellationToken);
        }

    }
}