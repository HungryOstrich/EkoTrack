using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EkoTrack.Models
{
    public class ActivityLog : IValidatableObject
    {
        public int Id { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Zużycie musi być większe od 0.")]
        public double Quantity { get; set; } 
        public double CalculatedCo2Emission { get; private set; }

        // Powiązanie ze źródłem
        public int EmissionSourceId { get; set; }
        [ForeignKey("EmissionSourceId")]
        public virtual EmissionSource? EmissionSource { get; set; }

        // Powiązanie z użytym współczynnikiem
        public int EmissionFactorId { get; set; }
        [ForeignKey("EmissionFactorId")]
        public virtual EmissionFactor? EmissionFactor { get; set; }

        public string CreatedById { get; set; }
        [Display(Name = "Created by")]
        public virtual AppUser? CreatedBy { get; set; }
        public void CalculateEmission()
        {
            if (EmissionFactor != null)
            {
                this.CalculatedCo2Emission = this.Quantity * this.EmissionFactor.Co2EquivalentPerUnit;
            }
        }

        // --- Logika Walidacji Biznesowej (IValidatableObject) ---
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {

            if (Date > DateTime.Now)
            {
                yield return new ValidationResult(
                    "Data aktywności nie może być z przyszłości.",
                    new[] { nameof(Date) });
            }

            if (Date < DateTime.Now.AddDays(-30))
            {
                yield return new ValidationResult(
                    "Nie można dodawać ani edytować wpisów starszych niż 30 dni (okres zamknięty).",
                    new[] { nameof(Date) });
            }

        }
    }
}