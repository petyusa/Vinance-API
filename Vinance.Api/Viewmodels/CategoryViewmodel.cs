
namespace Vinance.Api.Viewmodels
{
    using Contracts.Enums;

    public class CategoryViewmodel
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public int Balance { get; set; }
        public int MonthlyLimit { get; set; }
        public CategoryType Type { get; set; }
    }
}