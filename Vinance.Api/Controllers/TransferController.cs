using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Vinance.Api.Controllers
{
    using Contracts.Extensions;
    using Contracts.Interfaces;
    using Contracts.Models;
    using Logic;
    using Viewmodels;

    [Route("transfers")]
    [ApiController]
    public class TransferController : ControllerBase
    {
        private readonly ITransferService _transferService;
        private readonly IAuthorizationService _authorizationService;
        private readonly IMapper _mapper;

        public TransferController(ITransferService transferService, IAuthorizationService authorizationService, IMapper mapper)
        {
            _transferService = transferService;
            _authorizationService = authorizationService;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("")]
        public async Task<ActionResult> GetAll(int? categoryId, DateTime? from, DateTime? to, int page = 1, int pageSize = 20, string order = "date_desc")
        {
            var transfers = await _transferService.GetAll(categoryId, from, to, order);
            var model = _mapper.MapAll<TransferViewmodel>(transfers).ToPagedList(page, pageSize);
            return Ok(model);
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Create(Transfer transfer)
        {
            await _authorizationService.HandleCreateUpdateAsync(transfer);
            var createdTransfer = await _transferService.Create(transfer);
            var model = _mapper.Map<TransferViewmodel>(createdTransfer);
            return Created(Request.Path, model);
        }

        [HttpGet]
        [Route("{transferId}")]
        public async Task<IActionResult> GetById(int transferId)
        {
            await _authorizationService.HandleGetDeleteAsync<Transfer>(transferId);
            var transfer = await _transferService.GetById(transferId);
            var model = _mapper.Map<TransferViewmodel>(transfer);
            return Ok(model);
        }

        [HttpPut]
        [Route("")]
        public async Task<IActionResult> Update(Transfer transfer)
        {
            await _authorizationService.HandleCreateUpdateAsync(transfer);
            var updatedTransfer = await _transferService.Update(transfer);
            var model = _mapper.Map<TransferViewmodel>(updatedTransfer);
            return Ok(model);
        }

        [HttpDelete]
        [Route("{transferId}")]
        public async Task<IActionResult> Delete(int transferId)
        {
            await _authorizationService.HandleGetDeleteAsync<Transfer>(transferId);
            await _transferService.Delete(transferId);
            return NoContent();
        }

        [HttpPost]
        [Route("upload")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            using (var stream = new StreamReader(file.OpenReadStream()))
            {
                var transfers = await _transferService.Upload(stream);
                var viewmodel = _mapper.MapAll<TransferViewmodel>(transfers);
                return Ok(viewmodel);
            }
        }
    }
}