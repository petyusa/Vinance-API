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

    public class PaymentService : IPaymentService
    {
        private readonly IFactory<VinanceContext> _factory;
        private readonly IMapper _mapper;

        public PaymentService(IFactory<VinanceContext> factory, IMapper mapper)
        {
            _factory = factory;
            _mapper = mapper;
        }

        public async Task<Payment> Create(Payment payment)
        {
            using (var context = _factory.Create())
            {
                var dataPayment = _mapper.Map<Data.Entities.Payment>(payment);
                await context.Payments.AddAsync(dataPayment);
                return _mapper.Map<Payment>(dataPayment);
            }
        }

        public async Task<IEnumerable<Payment>> GetAll()
        {
            using (var context = _factory.Create())
            {
                var dataPayments = await context.Payments
                    .Include(p => p.From)
                    .Include(p => p.PaymentCategory)
                    .ToListAsync();
                return _mapper.Map<IEnumerable<Payment>>(dataPayments);
            }
        }

        public async Task<Payment> GetById(int paymentId)
        {
            using (var context = _factory.Create())
            {
                var dataPayment = await context.Payments
                    .Include(p => p.From)
                    .Include(p => p.PaymentCategory)
                    .SingleOrDefaultAsync(p=>p.Id == paymentId);
                return _mapper.Map<Payment>(dataPayment);
            }
        }

        public async Task<Payment> Update(Payment payment)
        {
            using (var context = _factory.Create())
            {
                if (!context.Payments.Any(p => p.Id == payment.Id))
                    return null;

                var dataPayment = _mapper.Map<Data.Entities.Payment>(payment);
                context.Entry(dataPayment).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return _mapper.Map<Payment>(dataPayment);
            }
        }

        public async Task<bool> Delete(int paymentId)
        {
            using (var context = _factory.Create())
            {
                var dataPayment = await context.Payments.FindAsync(paymentId);
                if (dataPayment == null)
                    return false;
                context.Payments.Remove(dataPayment);
                return await context.SaveChangesAsync() == 1;
            }
        }
    }
}