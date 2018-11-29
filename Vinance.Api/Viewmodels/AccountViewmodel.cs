using System.ComponentModel.DataAnnotations;

namespace Vinance.Api.Viewmodels
{
    public class AccountViewmodel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public int Balance { get; set; }

        public bool CanBeDeleted { get; set; }
    }
}