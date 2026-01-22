using System.ComponentModel.DataAnnotations;
using EkoTrack.Models;

namespace EkoTrack.DTOs
{
    public class OrganizationDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Nazwa organizacji jest wymagana.")]
        [StringLength(100, ErrorMessage = "Nazwa nie może przekraczać 100 znaków.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "NIP jest wymagany.")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "NIP musi składać się z 10 cyfr.")]
        public string TaxId { get; set; }

        [StringLength(200)]
        public string Address { get; set; }

        // Read-only property for view display
        public int UsersCount { get; set; }

        public OrganizationDTO() { }

        // Constructor to map Entity -> DTO
        public OrganizationDTO(Organization org)
        {
            Id = org.Id;
            Name = org.Name;
            TaxId = org.TaxId;
            Address = org.Address;
            UsersCount = org.Users?.Count ?? 0;
        }
    }
}