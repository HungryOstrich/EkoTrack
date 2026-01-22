using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace EkoTrack.Models
{
    public class AppUser : IdentityUser
    {
       // [Required]
        //public string FullName { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public virtual ICollection<Organization> Organizations { get; set; } = new List<Organization>();
    }
}
