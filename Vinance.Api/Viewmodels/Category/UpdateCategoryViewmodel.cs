using Vinance.Contracts.Enums;

namespace Vinance.Api.Viewmodels.Category
{
    /// <summary>
    /// The basic model for creating a category.
    /// </summary>
    public class UpdateCategoryViewmodel
    {
        /// <summary>
        /// The id of the category.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The name of the category.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The monthly-limit for the category.
        /// </summary>
        public int MonthlyLimit { get; set; }

        /// <summary>
        /// The type of the category.
        /// </summary>
        public CategoryType Type { get; set; }
    }
}