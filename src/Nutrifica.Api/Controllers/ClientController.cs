using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nutrifica.Api.Contracts.Clients;

namespace Nutrifica.Api.Controllers
{
    [Authorize]
    [Route("api/clients")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly ISender _mediatr;

        public ClientController()
        {
            // _mediatr = mediatr;
        }
        
        /// <summary>
        /// Get Clients
        /// </summary>
        /// <returns></returns>
        [HttpPost("logout")]
        public async Task<ActionResult<ICollection<ClientDTO>>> Get(CancellationToken ct)
        {
            return Ok(Array.Empty<ClientDTO>());
        }
    }
}
