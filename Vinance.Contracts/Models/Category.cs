using Vinance.Contracts.Enums;

namespace Vinance.Contracts.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Balance { get; set; }
        public CategoryType Type { get; set; }
    }
}