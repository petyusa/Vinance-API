using System.ComponentModel.DataAnnotations;
using Vinance.Contracts.Enums;

namespace Vinance.Api.Viewmodels
{
    public class AccountViewmodel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public AccountType AccountType { get; set; }

        public int Balance { get; set; }

        public bool IsActive { get; set; }

        public bool CanBeDeleted { get; set; }
    }
}