using System.ComponentModel.DataAnnotations;
using EkoTrack.Models;

namespace EkoTrack.DTOs
{
    public class ActivityLogDTO
    {
        public int Id { get; set; }
        public string DateFormatted { get; set; } 
        public double Quantity { get; set; }
        public double CalculatedCo2 { get; set; }
        public int EmissionSourceId { get; set; }
        public string SourceName { get; set; }
        public string CreatedById { get; set; }
        public virtual AppUser? CreatedBy { get; set; }

        // Informacje o czynniku (paliwie)
        public int EmissionFactorId { get; set; }
        public string FactorName { get; set; }
        public string Unit { get; set; }

        public ActivityLogDTO() { }

        public ActivityLogDTO(ActivityLog log)
        {
            Id = log.Id;
            DateFormatted = log.Date.ToShortDateString();
            Quantity = log.Quantity;
            CalculatedCo2 = log.CalculatedCo2Emission;
            CreatedById = log.CreatedById;
            CreatedBy = log.CreatedBy;

            EmissionSourceId = log.EmissionSourceId;
            EmissionFactorId = log.EmissionFactorId;

            // Płaskie mapowanie nazw (bezpieczne dla nulli)
            SourceName = log.EmissionSource?.Name ?? "Nieznane źródło";
            FactorName = log.EmissionFactor?.Name ?? "Nieznany czynnik";
            Unit = log.EmissionFactor?.Unit ?? "-";
        }
    }
}