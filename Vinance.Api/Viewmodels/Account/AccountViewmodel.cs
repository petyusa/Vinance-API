namespace Vinance.Api.Viewmodels.Account
{
    using Contracts.Enums;

    /// <summary>
    /// The view model of an account.
    /// </summary>
    public class AccountViewmodel
    {
        /// <summary>
        /// The id of the account.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The name of the account.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The type of the account.
        /// </summary>
        public AccountType AccountType { get; set; }

        /// <summary>
        /// The calculated balance of the account.
        /// </summary>
        public int Balance { get; set; }

        /// <summary>
        /// Shows if the account is active (if not, no transactions can be made from/to the account).
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Shows if the account can be deleted (are there any transactions to/from this account).
        /// </summary>
        public bool CanBeDeleted { get; set; }
    }
}