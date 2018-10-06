using System.Collections.Generic;
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

        public async Task<IEnumerable<Transfer>> GetTransfers()
        {
            IEnumerable<Transfer> transfers;
            using (var context = _factory.Create())
            {
                var dataTransfers = await context.Transfers.Include(t => t.From).Include(t => t.To).Include(t => t.TransferCategory).ToListAsync();
                transfers = _mapper.Map<IEnumerable<Transfer>>(dataTransfers);
            }
            return transfers;
        }
    }
}