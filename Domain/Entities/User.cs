using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Apposite.Domain.Entities
{
    public class User : IdentityUser<Guid>
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        public string FullName => $"{Name} {Surname}";
        public bool IsDeleted { get; set; } 
    }
}

