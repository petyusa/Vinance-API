using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Vinance.Api.Controllers
{
    using Contracts.Extensions;
    using Contracts.Interfaces;
    using Contracts.Models;
    using Viewmodels;

    [Route("api/transfers")]
    [ApiController]
    public class TransferController : ControllerBase
    {
        private readonly ITransferService _transferService;
        private readonly IMapper _mapper;

        public TransferController(ITransferService transferService, IMapper mapper)
        {
            _transferService = transferService;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("")]
        public async Task<ActionResult> GetAll()
        {
            var transfers = await _transferService.GetAll();

            var model = _mapper.MapAll<TransferViewmodel>(transfers);
            return Ok(model);
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Create(Transfer transfer)
        {
            var createdTransfer = await _transferService.Create(transfer);
            if (createdTransfer == null)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "There was an erro creating the transfer");
            }

            var model = _mapper.Map<TransferViewmodel>(createdTransfer);
            return Ok(model);
        }

        [HttpGet]
        [Route("{transferId}")]
        public async Task<IActionResult> Get(int transferId)
        {
            var transfer = await _transferService.Get(transferId);
            if (transfer == null)
            {
                return NotFound($"No transfer found with id: {transferId}");
            }

            var model = _mapper.Map<TransferViewmodel>(transfer);
            return Ok(model);
        }

        [HttpPut]
        [Route("")]
        public async Task<IActionResult> Update(Transfer transfer)
        {
            var updatedTransfer = await _transferService.Update(transfer);
            if (updatedTransfer == null)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "There was an erro updaeting the transfer");
            }

            var model = _mapper.Map<TransferViewmodel>(updatedTransfer);
            return Ok(model);
        }

        [HttpDelete]
        [Route("{transferId}")]
        public async Task<IActionResult> Delete(int transferId)
        {
            var success = await _transferService.Delete(transferId);
            if (success)
            {
                return NoContent();
            }

            return StatusCode((int)HttpStatusCode.InternalServerError, "There was an erro deleting the transfer");
        }
    }
}