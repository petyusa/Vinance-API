using System.Collections.Generic;
using System.Linq;

namespace Vinance.Contracts.Models
{
    public class Account
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Balance { get; set; }

        public int UserId { get; set; }
    }
}