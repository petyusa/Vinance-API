using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Vinance.Logic.Services
{
    using Contracts.Exceptions.NotFound;
    using Contracts.Extensions;
    using Contracts.Interfaces;
    using Contracts.Models;
    using Data.Contexts;
    using Identity.Interfaces;

    public class TransferService : ITransferService
    {
        private readonly IFactory<VinanceContext> _factory;
        private readonly Guid _userId;
        private readonly IMapper _mapper;

        public TransferService(IFactory<VinanceContext> factory, IIdentityService identityService, IMapper mapper)
        {
            _factory = factory;
            _mapper = mapper;
            _userId = identityService.GetCurrentUserId();
        }

        public async Task<IEnumerable<Transfer>> GetAll()
        {
            IEnumerable<Transfer> transfers;
            using (var context = _factory.Create())
            {
                var dataTransfers = await context.Transfers
                    .Include(p => p.From)
                    .Include(p => p.To)
                    .Include(t => t.Category)
                    .Where(t => t.UserId == _userId)
                    .ToListAsync();
                transfers = _mapper.Map<IEnumerable<Transfer>>(dataTransfers);
            }
            return transfers;
        }

        public async Task<Transfer> Create(Transfer transfer)
        {
            using (var context = _factory.Create())
            {
                var dataTransfer = _mapper.Map<Data.Entities.Transfer>(transfer);
                dataTransfer.UserId = _userId;
                context.Transfers.Add(dataTransfer);
                await context.SaveChangesAsync();
                return _mapper.Map<Transfer>(dataTransfer);
            }
        }

        public async Task<Transfer> GetById(int transferId)
        {
            using (var context = _factory.Create())
            {
                var dataTransfer = await context.Transfers
                    .Include(p => p.From)
                    .Include(p => p.To)
                    .Include(t => t.Category)
                    .SingleOrDefaultAsync(t => t.Id == transferId && t.UserId == _userId);
                if (dataTransfer == null)
                {
                    throw new TransferNotFoundException($"No transfer found with id: {transferId}");
                }
                return _mapper.Map<Transfer>(dataTransfer);
            }
        }

        public async Task<Transfer> Update(Transfer transfer)
        {
            using (var context = _factory.Create())
            {
                if (!context.Transfers.Any(t => t.Id == transfer.Id))
                {
                    throw new TransferNotFoundException($"No transfer found with id: {transfer.Id}");
                }

                var dataTransfer = _mapper.Map<Data.Entities.Transfer>(transfer);
                context.Entry(dataTransfer).State = EntityState.Modified;
                context.Entry(dataTransfer).Property(t => t.UserId).IsModified = false;
                await context.SaveChangesAsync();
                dataTransfer = context.Transfers.Find(transfer.Id);
                return _mapper.Map<Transfer>(dataTransfer);
            }
        }

        public async Task Delete(int transferId)
        {
            using (var context = _factory.Create())
            {
                var dataTransfer = context.Transfers.Find(transferId);
                if (dataTransfer == null)
                {
                    throw new TransferNotFoundException($"No transfer found with id: {transferId}");
                }

                context.Transfers.Remove(dataTransfer);
                await context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Transfer>> GetByAccountId(int accountId)
        {
            using (var context = _factory.Create())
            {
                var account = await context.Accounts
                    .Include(a => a.TransfersFrom)
                    .Include(a => a.TransfersTo)
                    .SingleOrDefaultAsync(a => a.Id == accountId);
                var transfers = account.TransfersFrom.ToList().Concat(account.TransfersTo.ToList());
                return _mapper.MapAll<Transfer>(transfers);
            }
        }

        public async Task<IEnumerable<Transfer>> GetByCategoryId(int categoryId)
        {
            using (var context = _factory.Create())
            {
                var transfers = await context.Transfers
                    .Where(t => t.CategoryId == categoryId)
                    .ToListAsync();
                return _mapper.MapAll<Transfer>(transfers);
            }
        }
    }
}