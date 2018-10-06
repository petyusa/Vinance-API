using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Vinance.Api.Controllers
{
    using Contracts.Interfaces;

    [Route("api/transfer")]
    [ApiController]
    public class TransferController : ControllerBase
    {
        private readonly ITransferService _transferService;

        public TransferController(ITransferService transferService)
        {
            _transferService = transferService;
        }

        [HttpGet]
        [Route("transfers")]
        public async Task<ActionResult> GetTransfers()
        {
            var transfers = await _transferService.GetTransfers();
            return Ok(transfers);
        }
    }
}