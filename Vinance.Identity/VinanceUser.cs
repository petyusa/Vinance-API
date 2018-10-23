using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Vinance.Identity
{
    public class VinanceUser : IdentityUser
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
    }
}