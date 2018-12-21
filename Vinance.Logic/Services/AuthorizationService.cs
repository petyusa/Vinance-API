using System;
using System.Threading.Tasks;

namespace Vinance.Logic.Services
{
    using Contracts.Enums;
    using Contracts.Exceptions;
    using Contracts.Interfaces;
    using Contracts.Models;
    using Contracts.Models.BaseModels;
    using Identity.Interfaces;

    public class AuthorizationService : IAuthorizationService
    {
        private readonly IAccountService _accountService;
        private readonly IIncomeService _incomeService;
        private readonly IExpenseService _expenseService;
        private readonly ITransferService _transferService;
        private readonly ICategoryService _categoryService;
        private readonly Guid _userId;

        public AuthorizationService(
            IAccountService accountService,
            IIncomeService incomeService,
            IExpenseService expenseService,
            ITransferService transferService,
            ICategoryService categoryService,
            IIdentityService identityService)
        {
            _accountService = accountService;
            _incomeService = incomeService;
            _expenseService = expenseService;
            _transferService = transferService;
            _categoryService = categoryService;
            _userId = identityService.GetCurrentUserId();
        }

        public async Task HandleCreateUpdateAsync(BaseModel resource)
        {
            switch (resource)
            {
                case Account account:
                    await AuthorizeAccount(account);
                    break;
                case Category category:
                    await AuthorizeCategory(category);
                    break;
                case Income income:
                    await AuthorizeIncome(income);
                    break;
                case Expense expense:
                    await AuthorizeExpense(expense);
                    break;
                case Transfer transfer:
                    await AuthorizeTransfer(transfer);
                    break;
                default:
                    throw new ArgumentException($"Not existing type for {resource}");
            }
        }

        public async Task HandleGetDeleteAsync<T>(int entityId) where T : BaseModel
        {
            BaseModel resource;
            if (typeof(T) == typeof(Account))
            {
                resource = await _accountService.Get(entityId);
            }
            else if (typeof(T) == typeof(Category))
            {
                resource = await _categoryService.Get(entityId);
            }
            else if (typeof(T) == typeof(Income))
            {
                resource = await _incomeService.GetById(entityId);
            }
            else if (typeof(T) == typeof(Expense))
            {
                resource = await _expenseService.GetById(entityId);
            }
            else
            {
                resource = await _transferService.GetById(entityId);
            }
            AuthorizeOwner(resource);
        }

        private async Task AuthorizeAccount(Account account)
        {
            if (account.Id != 0)
            {
                var existingAccount = await _accountService.Get(account.Id);
                AuthorizeOwner(existingAccount);
            }
        }

        private async Task AuthorizeCategory(Category category)
        {
            if (category.Id != 0)
            {
                var existingCategory = await _categoryService.Get(category.Id);
                AuthorizeOwner(existingCategory);
            }
        }


        private async Task AuthorizeIncome(Income income)
        {
            if (income.Id != 0)
            {
                var existingIncome = await _incomeService.GetById(income.Id);
                AuthorizeOwner(existingIncome);
            }
            var account = await _accountService.Get(income.ToId);
            var category = await _categoryService.Get(income.CategoryId);
            AuthorizeOwner(account, category);
            await AuthorizeCategory(income, CategoryType.Income);
        }

        private async Task AuthorizeExpense(Expense expense)
        {
            if (expense.Id != 0)
            {
                var existingExpense = await _expenseService.GetById(expense.Id);
                AuthorizeOwner(existingExpense);
            }
            var account = await _accountService.Get(expense.FromId);
            var category = await _categoryService.Get(expense.CategoryId);
            AuthorizeOwner(account, category);
            await AuthorizeCategory(expense, CategoryType.Expense);
        }

        private async Task AuthorizeTransfer(Transfer transfer)
        {
            if (transfer.Id != 0)
            {
                var existingTransfer = await _transferService.GetById(transfer.Id);
                AuthorizeOwner(existingTransfer);
            }
            var accountFrom = await _accountService.Get(transfer.FromId);
            var accountTo = await _accountService.Get(transfer.ToId);
            AuthorizeOwner(accountFrom, accountTo);
        }

        private void AuthorizeOwner(params BaseModel[] resources)
        {
            foreach (var resource in resources)
            {
                AuthorizeOwner(resource);
            }
        }

        private void AuthorizeOwner(BaseModel resource)
        {
            if (resource.UserId != _userId)
            {
                throw new NotOwnerException("The user does not own the selected resource");
            }
        }

        private async Task AuthorizeCategory(Transaction resource, CategoryType type)
        {
            var category = resource.Category ?? await _categoryService.Get(resource.CategoryId);
            if (category.Type != type)
            {
                throw new NotCorrectCategoryException($"The type of the category was not correct, expected {type}, but was {category.Type}");
            }
        }
    }
}