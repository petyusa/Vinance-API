using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vinance.Contracts.Models.Helpers;

namespace Vinance.Logic.Services
{
    using Contracts.Exceptions.NotFound;
    using Contracts.Extensions;
    using Contracts.Interfaces;
    using Contracts.Models;
    using Data.Contexts;
    using Identity.Interfaces;

    public class AccountService : IAccountService
    {
        private readonly IFactory<VinanceContext> _factory;
        private readonly Guid _userId;
        private readonly IMapper _mapper;

        public AccountService(IIdentityService identityService, IFactory<VinanceContext> factory, IMapper mapper)
        {
            _factory = factory;
            _mapper = mapper;
            _userId = identityService.GetCurrentUserId();
        }

        public async Task<Account> Create(Account account)
        {
            using (var context = _factory.CreateDbContext())
            {
                var dataAccount = _mapper.Map<Data.Entities.Account>(account);
                dataAccount.UserId = _userId;
                context.Accounts.Add(dataAccount);
                await context.SaveChangesAsync();
                return _mapper.Map<Account>(dataAccount);
            }
        }

        public async Task<Account> Get(int accountId)
        {
            using (var context = _factory.CreateDbContext())
            {
                var dataAccount = await context.Accounts
                    .Include(a => a.Expenses)
                    .Include(a => a.Incomes)
                    .Include(a => a.TransfersFrom)
                    .Include(a => a.TransfersTo)
                    .SingleOrDefaultAsync(a => a.Id == accountId);

                if (dataAccount == null)
                {
                    throw new AccountNotFoundException($"No account found with id: {accountId}");
                }

                var account = _mapper.Map<Account>(dataAccount);
                return account;
            }
        }

        public async Task<IEnumerable<Account>> GetAll()
        {
            using (var context = _factory.CreateDbContext())
            {
                var dataAccounts = await context.Accounts
                    .Where(a => a.UserId == _userId)
                    .Include(a => a.Expenses)
                    .Include(a => a.Incomes)
                    .Include(a => a.TransfersFrom)
                    .Include(a => a.TransfersTo)
                    .ToListAsync();
                return _mapper.MapAll<Account>(dataAccounts.ToList());
            }
        }

        public async Task<Account> Update(Account account)
        {
            using (var context = _factory.CreateDbContext())
            {
                if (!context.Accounts.Any(a => a.Id == account.Id))
                {
                    throw new AccountNotFoundException($"No account found with id: {account.Id}");
                }

                var dataAccount = _mapper.Map<Data.Entities.Account>(account);
                context.Entry(dataAccount).State = EntityState.Modified;
                context.Entry(dataAccount).Property(a => a.UserId).IsModified = false;
                await context.SaveChangesAsync();
                dataAccount = context.Accounts.Find(account.Id);
                return _mapper.Map<Account>(dataAccount);
            }
        }

        public async Task Delete(int accountId)
        {
            using (var context = _factory.CreateDbContext())
            {
                var dataAccount = context.Accounts.Find(accountId);
                if (dataAccount == null || dataAccount.UserId != _userId)
                {
                    throw new AccountNotFoundException($"No account found with id: {accountId}");
                }

                context.Accounts.Remove(dataAccount);
                await context.SaveChangesAsync();
            }
        }

        public List<DailyBalanceList> GetDailyBalances(DateTime? from = null, DateTime? to = null)
        {
            if (!from.HasValue || !to.HasValue)
            {
                var now = DateTime.Now;
                to = new DateTime(now.Year, now.Month, 1).AddMonths(1);
                from = to.Value.AddMonths(-1);
            }
            using (var context = _factory.CreateDbContext())
            {
                var accounts = context.Accounts
                    .Include(a => a.Expenses)
                    .Include(a => a.Incomes)
                    .Include(a => a.TransfersFrom)
                    .Include(a => a.TransfersTo)
                    .Where(a => a.UserId == _userId && a.IsActive && !a.IsSaving && a.Name != "Tartozás" && a.Name != "Követelés");
                var dailyBalances = new List<DailyBalanceList>();
                foreach (var account in accounts)
                {
                    var balances = new Dictionary<DateTime, int>();
                    var opening = account.OpeningBalance;
                    var firstDates = new List<DateTime>();
                    if (account.Incomes != null && account.Incomes.Any())
                    {
                        var firstIncome = account.Incomes.Min(i => i.Date);
                        firstDates.Add(firstIncome);
                    }
                    if (account.Expenses != null && account.Expenses.Any())
                    {
                        var firstExpense = account.Expenses.Min(i => i.Date);
                        firstDates.Add(firstExpense);
                    }
                    if (account.TransfersFrom != null && account.TransfersFrom.Any())
                    {
                        var firstTransferFrom = account.TransfersFrom.Min(i => i.Date);
                        firstDates.Add(firstTransferFrom);
                    }
                    if (account.TransfersTo != null && account.TransfersTo.Any())
                    {
                        var firstTransferTo = account.TransfersTo.Min(i => i.Date);
                        firstDates.Add(firstTransferTo);
                    }

                    if (!firstDates.Any())
                        continue;

                    var firstDate = firstDates.Min(d => d);
                    balances[firstDate.AddDays(-1)] = opening;
                    for (var i = firstDate; i <= to.Value; i = i.AddDays(1))
                    {
                        var date = i.Date;
                        var balance = balances[date.AddDays(-1)];
                        if (account.Incomes != null)
                        {
                            var incomes = account.Incomes.Where(income => income.Date.Date == date);
                            balance += incomes.Sum(income => income.Amount);
                        }

                        if (account.Expenses != null)
                        {
                            var expenses = account.Expenses.Where(income => income.Date.Date == date);
                            balance -= expenses.Sum(e => e.Amount);
                        }

                        if (account.TransfersFrom != null)
                        {
                            var transfersFrom = account.TransfersFrom.Where(income => income.Date.Date == date);
                            balance -= transfersFrom.Sum(t => t.Amount);
                        }

                        if (account.TransfersTo != null)
                        {
                            var transfersTo = account.TransfersTo.Where(income => income.Date.Date == date);
                            balance += transfersTo.Sum(t => t.Amount);
                        }

                        if (balances.ContainsKey(date))
                        {
                            balances[date] += balance;
                        }
                        else
                        {
                            balances.Add(date, balance);
                        }
                    }
                    var dailyBalancesForAccount = new DailyBalanceList{ AccountName = account.Name, DailyBalances = balances};
                    dailyBalances.Add(dailyBalancesForAccount);
                }

                return dailyBalances;
            }
        }
    }
}
