using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Vinance.Api.Controllers
{
    using Contracts.Models;
    using Contracts.Interfaces;

    [Route("api/transfers")]
    [ApiController]
    public class TransferController : ControllerBase
    {
        private readonly ITransferService _transferService;

        public TransferController(ITransferService transferService)
        {
            _transferService = transferService;
        }

        [HttpGet]
        [Route("")]
        public async Task<ActionResult> GetAll()
        {
            var transfers = await _transferService.GetAll();
            return Ok(transfers);
        }


        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Create(Transfer transfer)
        {
            var createdTransfer = await _transferService.Create(transfer);
            if (createdTransfer == null)
                return StatusCode((int)HttpStatusCode.InternalServerError, "There was an erro creating the transfer");
            return Created(Request.Path, createdTransfer);
        }

        [HttpGet]
        [Route("{transferId}")]
        public async Task<IActionResult> Get(int transferId)
        {
            var transfer = await _transferService.Get(transferId);
            if (transfer == null)
                return NotFound($"No transfer found with id: {transferId}");
            return Ok(transfer);
        }

        [HttpPut]
        [Route("")]
        public async Task<IActionResult> Update(Transfer transfer)
        {
            var updatedTransfer = await _transferService.Update(transfer);
            if (updatedTransfer == null)
                return StatusCode((int)HttpStatusCode.InternalServerError, "There was an erro updaeting the transfer");
            return Created(Request.Path, updatedTransfer);

        }

        [HttpDelete]
        [Route("{transferId}")]
        public async Task<IActionResult> Delete(int transferId)
        {
            var success = await _transferService.Delete(transferId);
            if (success)
                return NoContent();
            return StatusCode((int)HttpStatusCode.InternalServerError, "There was an erro deleting the transfer");
        }
    }
}