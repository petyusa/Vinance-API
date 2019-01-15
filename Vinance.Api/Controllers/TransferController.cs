using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Vinance.Api.Helpers;

namespace Vinance.Api.Controllers
{
    using Contracts.Enums;
    using Contracts.Extensions;
    using Contracts.Interfaces;
    using Contracts.Models;
    using Logic;
    using Viewmodels.Transfer;

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

        /// <summary>
        /// Gets the transfers of the user in a paginated format.
        /// </summary>
        /// <param name="accountId">If specified, only transfers related to this account will be returned.</param>
        /// <param name="transferType">If specified, only transfers of this type will be returned.</param>
        /// <param name="from">If specified, only transfers from this date will be returned.</param>
        /// <param name="to">If specified, only transfers to this date will be returned.</param>
        /// <param name="pageSize">If specified, the given number of transfers will be returned (if not specified, defaults to 20).</param>
        /// <param name="page">If specified, the given page will be returned (if not, defaults to 1).</param>
        /// <param name="order">If specified, the transfers will be sorted by the given order (default date_desc).</param>
        [SwaggerResponse(200, Type = typeof(VinanceApiResponseExample<PagedList<TransferViewmodel>>))]
        [HttpGet]
        [Route("")]
        public async Task<ActionResult> GetAll(int? accountId, TransferType? transferType, DateTime? from, DateTime? to, int page = 1, int pageSize = 20, string order = null)
        {
            var transfers = await _transferService.GetAll(accountId, transferType, from, to, order);
            var model = _mapper.MapAll<TransferViewmodel>(transfers).ToPagedList(page, pageSize);
            return Ok(model);
        }

        /// <summary>
        /// Creates a new transfer.
        /// </summary>
        /// <param name="transferToCreate">The transfer to be created.</param>
        [SwaggerResponse(201, Type = typeof(VinanceApiResponseExample<TransferViewmodel>))]
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Create(CreateTransferViewmodel transferToCreate)
        {
            var transfer = _mapper.Map<Transfer>(transferToCreate);
            await _authorizationService.HandleCreateUpdateAsync(transfer);
            var createdTransfer = await _transferService.Create(transfer);
            var model = _mapper.Map<TransferViewmodel>(createdTransfer);
            return Created(Request.Path, model);
        }

        /// <summary>
        /// Gets the transfer with the specified id.
        /// </summary>
        /// <param name="transferId">The id of the transfer to be returned.</param>
        [SwaggerResponse(200, Type = typeof(VinanceApiResponseExample<TransferViewmodel>))]
        [SwaggerResponse(403, Description = "User is not he owner of the transfer.")]
        [SwaggerResponse(404, Description = "No transfer found with the given id.")]
        [HttpGet]
        [Route("{transferId}")]
        public async Task<IActionResult> GetById(int transferId)
        {
            await _authorizationService.HandleGetDeleteAsync<Transfer>(transferId);
            var transfer = await _transferService.GetById(transferId);
            var model = _mapper.Map<TransferViewmodel>(transfer);
            return Ok(model);
        }

        /// <summary>
        /// Updates the given transfer.
        /// </summary>
        /// <param name="transferToUpdate">The transfer to be updated.</param>
        [SwaggerResponse(200, Type = typeof(VinanceApiResponseExample<TransferViewmodel>))]
        [SwaggerResponse(403, Description = "User is not he owner of the transfer.")]
        [SwaggerResponse(404, Description = "No transfer found with the given id.")]
        [HttpPut]
        [Route("")]
        public async Task<IActionResult> Update(UpdateTransferViewmodel transferToUpdate)
        {
            var transfer = _mapper.Map<Transfer>(transferToUpdate);
            await _authorizationService.HandleCreateUpdateAsync(transfer);
            var updatedTransfer = await _transferService.Update(transfer);
            var model = _mapper.Map<TransferViewmodel>(updatedTransfer);
            return Ok(model);
        }

        /// <summary>
        /// Deletes the transfer with the specified id.
        /// </summary>
        /// <param name="transferId">The id of the transfer to be deleted.</param>
        [SwaggerResponse(204)]
        [SwaggerResponse(403, Description = "User is not he owner of the transfer.")]
        [SwaggerResponse(404, Description = "No transfer found with the given id.")]
        [HttpDelete]
        [Route("{transferId}")]
        public async Task<IActionResult> Delete(int transferId)
        {
            await _authorizationService.HandleGetDeleteAsync<Transfer>(transferId);
            await _transferService.Delete(transferId);
            return NoContent();
        }

        /// <summary>
        /// Uploads multiple transfers from Excel-file.
        /// </summary>
        /// <param name="file">The excel file containing the transfers.</param>
        [SwaggerResponse(200, Type = typeof(VinanceApiResponseExample<List<TransferViewmodel>>))]
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