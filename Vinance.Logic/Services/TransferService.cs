using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NPOI.XSSF.UserModel;

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

        public async Task<IEnumerable<Transfer>> GetAll(int? categoryId = null, DateTime? @from = null, DateTime? to = null, string order = "date_desc")
        {
            using (var context = _factory.CreateDbContext())
            {
                var dataTransfers = context.Transfers
                    .Where(t => t.UserId == _userId);

                if (from.HasValue && to.HasValue)
                {
                    dataTransfers = dataTransfers.Where(t => t.Date >= from.Value && t.Date <= to.Value);
                }

                if (categoryId.HasValue)
                {
                    dataTransfers = dataTransfers.Where(t => t.CategoryId == categoryId);
                }

                switch (order)
                {
                    case "date":
                        dataTransfers = dataTransfers.OrderBy(t => t.Date);
                        break;
                    case "date_desc":
                        dataTransfers = dataTransfers.OrderByDescending(t => t.Date);
                        break;
                    case "amount":
                        dataTransfers = dataTransfers.OrderBy(t => t.Amount);
                        break;
                    case "amount_desc":
                        dataTransfers = dataTransfers.OrderByDescending(t => t.Amount);
                        break;
                    default:
                        dataTransfers = dataTransfers.OrderByDescending(t => t.Date);
                        break;
                }

                var list = await dataTransfers
                    .Include(t => t.Category)
                    .Include(t => t.From)
                    .Include(t => t.To)
                    .ToListAsync();

                return _mapper.Map<IEnumerable<Transfer>>(list);
            }
        }
    
        public async Task<Transfer> Create(Transfer transfer)
        {
            using (var context = _factory.CreateDbContext())
            {
                var dataTransfer = _mapper.Map<Data.Entities.Transfer>(transfer);
                dataTransfer.UserId = _userId;
                context.Transfers.Add(dataTransfer);
                await context.SaveChangesAsync();
                return _mapper.Map<Transfer>(dataTransfer);
            }
        }

        public async Task<IEnumerable<Transfer>> Upload(StreamReader stream)
        {
            var transfers = new List<Data.Entities.Transfer>();
            using (stream)
            {
                var workbook = new XSSFWorkbook(stream.BaseStream);
                var sheet = workbook.GetSheet("Transfers");
                for (var rownum = 0; rownum <= sheet.LastRowNum; rownum++)
                {
                    if (sheet.GetRow(rownum) == null)
                    {
                        continue;
                    }

                    var row = sheet.GetRow(rownum);
                    var transfer = new Data.Entities.Transfer
                    {
                        Date = row.GetCell(0).DateCellValue,
                        FromId = (int)row.GetCell(1).NumericCellValue,
                        ToId = (int)row.GetCell(2).NumericCellValue,
                        CategoryId = (int)row.GetCell(3).NumericCellValue,
                        Amount = (int)row.GetCell(4).NumericCellValue,
                        Comment = row.GetCell(5)?.StringCellValue,
                        UserId = _userId
                    };

                    transfers.Add(transfer);
                }
            }

            using (var context = _factory.CreateDbContext())
            {
                await context.Transfers.AddRangeAsync(transfers);
                await context.SaveChangesAsync();
            }

            var mappedExpenses = _mapper.MapAll<Transfer>(transfers);

            return mappedExpenses;
        }

        public async Task<Transfer> GetById(int transferId)
        {
            using (var context = _factory.CreateDbContext())
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
            using (var context = _factory.CreateDbContext())
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
            using (var context = _factory.CreateDbContext())
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
            using (var context = _factory.CreateDbContext())
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
            using (var context = _factory.CreateDbContext())
            {
                var transfers = await context.Transfers
                    .Where(t => t.CategoryId == categoryId)
                    .ToListAsync();
                return _mapper.MapAll<Transfer>(transfers);
            }
        }
    }
}