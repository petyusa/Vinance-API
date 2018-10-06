using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Vinance.Logic.Services
{
    using Contracts.Interfaces;
    using Contracts.Models;
    using Data.Contexts;

    public class PaymentService : IPaymentService
    {
        private readonly IFactory<VinanceContext> _factory;
        private readonly IMapper _mapper;

        public PaymentService(IFactory<VinanceContext> factory, IMapper mapper)
        {
            _factory = factory;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Payment>> GetPayments()
        {
            IEnumerable<Payment> payments;
            using (var context = _factory.Create())
            {
                var dataPayments = await context.Payments
                    .Include(p => p.From)
                    .Include(p => p.PaymentCategory)
                    .ToListAsync();
                payments = _mapper.Map<IEnumerable<Payment>>(dataPayments);
            }
            return payments;
        }
    }
}