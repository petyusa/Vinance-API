using System.ComponentModel.DataAnnotations;
using Vinance.Contracts.Enums;

namespace Vinance.Api.Viewmodels.Account
{
    /// <summary>
    /// The basic model of an account to be created.
    /// </summary>
    public class CreateAccountViewmodel
    {
        /// <summary>
        /// Name of the account.
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Type of the account.
        /// </summary>
        [Required]
        public AccountType AccountType { get; set; }

        /// <summary>
        /// Shows if the account is active or not (if not active, no transactions can be made to/from the account)
        /// </summary>
        [Required]
        public bool IsActive { get; set; }

        /// <summary>
        /// Opening balance of the account.
        /// </summary>
        [Required]
        public int OpeningBalance { get; set; }
    }
}