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
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;

        public TransferController(ITransferService transferService, IMapper mapper, IAccountService accountService)
        {
            _transferService = transferService;
            _mapper = mapper;
            _accountService = accountService;
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
            await _accountService.CheckOwner(transfer.FromId, transfer.ToId);
            var createdTransfer = await _transferService.Create(transfer);
            var model = _mapper.Map<TransferViewmodel>(createdTransfer);
            return Created(Request.Path, model);
        }

        [HttpGet]
        [Route("{transferId}")]
        public async Task<IActionResult> GetById(int transferId)
        {
            var transfer = await _transferService.GetById(transferId);
            var model = _mapper.Map<TransferViewmodel>(transfer);
            return Ok(model);
        }

        [HttpPut]
        [Route("")]
        public async Task<IActionResult> Update(Transfer transfer)
        {
            await _accountService.CheckOwner(transfer.FromId, transfer.ToId);
            var updatedTransfer = await _transferService.Update(transfer);
            var model = _mapper.Map<TransferViewmodel>(updatedTransfer);
            return Ok(model);
        }

        [HttpDelete]
        [Route("{transferId}")]
        public async Task<IActionResult> Delete(int transferId)
        {
            await _transferService.Delete(transferId);
            return NoContent();
        }
    }
}