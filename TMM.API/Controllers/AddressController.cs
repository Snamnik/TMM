namespace TMM.API.Controllers
{
    [ApiController]
    [Route("api/customers")]
    [Authorize(AuthenticationSchemes = AuthenticationScheme.TMMScheme, Roles = Roles.Admin)]
    public class AddressController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AddressController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// add an address of a customer
        /// </summary>
        [HttpPost("{customerId}/addresses")]
        [ProducesResponseType(typeof(AddressDTO), StatusCodes.Status200OK)]
        public Task<AddressDTO> AddAddressAsync([FromRoute] int customerId, [FromBody] AddAddressDTO address, CancellationToken cancellationToken)
        {
            return _mediator.Send(new AddAddressCommand(customerId, address), cancellationToken);
        }

        /// <summary>
        /// delete an address of a customer
        /// </summary>
        [HttpDelete("{customerId}/addresses/{addressId}/")]
        public Task DeleteAddressAsync([FromRoute] int customerId, int addressId, CancellationToken cancellationToken)
        {
            return _mediator.Send(new DeleteAddressCommand(customerId, addressId), cancellationToken);
        }

        /// <summary>
        /// get addresses of a customer
        /// </summary>
        [HttpGet("{customerId}/addresses")]
        [ProducesResponseType(typeof(List<AddressDTO>), StatusCodes.Status200OK)]
        public Task<List<AddressDTO>> GetAddressesAsync([FromRoute] int customerId, CancellationToken cancellationToken)
        {
            return _mediator.Send(new GetAddressesQuery(customerId), cancellationToken);
        }

        /// <summary>
        /// set an address as main address of a customer
        /// </summary>
        [HttpPut("{customerId}/addresses/{addressId}/as-main")]
        public Task SetAsMainAddressAsync([FromRoute] int customerId, int addressId, CancellationToken cancellationToken)
        {
            return _mediator.Send(new SetAsMainAddressCommand(customerId, addressId), cancellationToken);
        }

    }
}