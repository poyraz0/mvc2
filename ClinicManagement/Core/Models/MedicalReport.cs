using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;



namespace ClinicManagement.Core.Models
{
    public class MedicalReport
    {
        [Key]
        public int ReportId { get; set; } // Entity Framework bunu otomatik olarak Primary Key kabul eder.

        public string UserId { get; set; }
        public string ReportPath { get; set; }
        public string AnalysisResult { get; set; }
        public DateTime? UploadDate { get; set; }

        // Navigation Property
        public ICollection<MedicalAnalysis> MedicalAnalyses { get; set; }
    }


}
