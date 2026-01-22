using System.ComponentModel.DataAnnotations;

namespace EkoTrack.Models
{
    public class Organization
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Nazwa organizacji jest wymagana.")]
        [StringLength(100, ErrorMessage = "Nazwa nie może przekraczać 100 znaków.")]
        public string Name { get; set; }

        [StringLength(200)]
        public string Address { get; set; }

        [Required(ErrorMessage = "NIP jest wymagany.")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "NIP musi składać się z 10 cyfr.")]
        
        public string TaxId { get; set; }


        public virtual ICollection<EmissionSource> EmissionSources { get; set; } = new List<EmissionSource>();

        
        public virtual ICollection<AppUser> Users { get; set; } = new List<AppUser>();
    }
}