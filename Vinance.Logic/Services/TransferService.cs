using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Vinance.Logic.Services
{
    using Contracts.Interfaces;
    using Contracts.Models;
    using Data.Contexts;

    public class TransferService : ITransferService
    {
        private readonly IFactory<VinanceContext> _factory;
        private readonly IMapper _mapper;

        public TransferService(IFactory<VinanceContext> factory, IMapper mapper)
        {
            _factory = factory;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Transfer>> GetAll()
        {
            IEnumerable<Transfer> transfers;
            using (var context = _factory.Create())
            {
                var dataTransfers = await context.Transfers
                    .Include(t => t.From)
                    .Include(t => t.To)
                    .Include(t => t.TransferCategory)
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
                context.Transfers.Add(dataTransfer);
                await context.SaveChangesAsync();
                return _mapper.Map<Transfer>(dataTransfer);
            }
        }

        public async Task<Transfer> Get(int transferId)
        {
            using (var context = _factory.Create())
            {
                var dataTransfer = await context.Transfers
                    .Include(t => t.From)
                    .Include(t => t.To)
                    .Include(t => t.TransferCategory)
                    .SingleOrDefaultAsync(a => a.Id == transferId);
                return _mapper.Map<Transfer>(dataTransfer);
            }
        }

        public async Task<Transfer> Update(Transfer transfer)
        {
            using (var context = _factory.Create())
            {
                if (!context.Transfers.Any(a => a.Id == transfer.Id))
                    return null;

                var dataTransfer = _mapper.Map<Data.Entities.Transfer>(transfer);
                context.Entry(dataTransfer).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return _mapper.Map<Transfer>(dataTransfer);
            }
        }

        public async Task<bool> Delete(int transferId)
        {
            using (var context = _factory.Create())
            {
                var dataTransfer = context.Transfers.Find(transferId);
                if (dataTransfer == null)
                    return false;
                context.Transfers.Remove(dataTransfer);
                return await context.SaveChangesAsync() == 1;
            }
        }
    }
}