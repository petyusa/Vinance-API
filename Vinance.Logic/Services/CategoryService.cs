using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Vinance.Logic.Services
{
    using Contracts.Extensions;
    using Contracts.Interfaces;
    using Contracts.Models.BaseModels;
    using Contracts.Models.Categories;
    using Data.Contexts;
    using Identity;

    public class CategoryService : ICategoryService
    {
        private readonly IFactory<VinanceContext> _factory;
        private readonly Guid _userId;
        private readonly IMapper _mapper;

        public CategoryService(IFactory<VinanceContext> factory, IIdentityService identityService, IMapper mapper)
        {
            _factory = factory;
            _mapper = mapper;
            _userId = identityService.GetCurrentUserId();
        }

        public async Task<IEnumerable<Category>> GetAll<T>() where T : Category
        {
            using (var context = _factory.Create())
            {
                IEnumerable<Data.Entities.Base.Category> categories;
                if (typeof(T) == typeof(ExpenseCategory))
                {
                    categories = await context.ExpenseCategories
                        .Include(ec => ec.Expenses)
                        .Where(ec => ec.UserId == _userId)
                        .ToListAsync();
                }
                else if (typeof(T) == typeof(IncomeCategory))
                {
                    categories = await context.IncomeCategories
                        .Include(ic => ic.Incomes)
                        .Where(ic => ic.UserId == _userId)
                        .ToListAsync();
                }
                else
                {
                    categories = await context.TransferCategories
                        .Include(tc => tc.Transfers)
                        .Where(ic => ic.UserId == _userId)
                        .ToListAsync();
                }
                return _mapper.MapAll<T>(categories);
            }
        }

        public async Task<Category> Create(Category category)
        {
            using (var context = _factory.Create())
            {
                switch (category)
                {
                    case IncomeCategory incomeCategory:
                        var dataIncomeCategory = _mapper.Map<Data.Entities.Categories.IncomeCategory>(incomeCategory);
                        dataIncomeCategory.UserId = _userId;
                        context.IncomeCategories.Add(dataIncomeCategory);
                        await context.SaveChangesAsync();
                        dataIncomeCategory = context.IncomeCategories.Find(category.Id);
                        return _mapper.Map<IncomeCategory>(dataIncomeCategory);
                    case ExpenseCategory expenseCategory:
                        var dataExpenseCategory = _mapper.Map<Data.Entities.Categories.ExpenseCategory>(expenseCategory);
                        dataExpenseCategory.UserId = _userId;
                        context.ExpenseCategories.Add(dataExpenseCategory);
                        await context.SaveChangesAsync();
                        dataExpenseCategory = context.ExpenseCategories.Find(category.Id);
                        return _mapper.Map<IncomeCategory>(dataExpenseCategory);
                    case TransferCategory transferCategory:
                        var dataTransferCategory =
                            _mapper.Map<Data.Entities.Categories.TransferCategory>(transferCategory);
                        dataTransferCategory.UserId = _userId;
                        context.TransferCategories.Add(dataTransferCategory);
                        await context.SaveChangesAsync();
                        dataTransferCategory = context.TransferCategories.Find(category.Id);
                        return _mapper.Map<IncomeCategory>(dataTransferCategory);
                    default:
                        throw new InvalidOperationException($"{nameof(category)} is not of type Category");
                }
            }
        }
    }
}