using System.ComponentModel.DataAnnotations;

namespace EkoTrack.Models
{
    public class EmissionFactor
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } 

        [Required]
        public string Unit { get; set; }

        [Required]
        [Range(0.0001, double.MaxValue, ErrorMessage = "Współczynnik musi być dodatni.")]
        public double Co2EquivalentPerUnit { get; set; } 

        public int Year { get; set; }

        public bool IsActive { get; set; } = true;
    }
}