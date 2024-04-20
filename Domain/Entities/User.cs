using System.ComponentModel.DataAnnotations;
using Core.Entity;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities
{
    public class User : BaseEntity
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}

