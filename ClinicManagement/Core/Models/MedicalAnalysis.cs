using System;

namespace ClinicManagement.Core.Models
{
    public class MedicalAnalysis
    {
        public int Id { get; set; }
        public int MedicalReportId { get; set; } // Foreign Key
        public DateTime AnalizTarihi { get; set; } = DateTime.Now;
        public string Sonuc { get; set; }
        public string Detaylar { get; set; }

        // Navigation Property
        //public MedicalReport MedicalReport { get; set; }


      
        public string Result { get; set; } // Analiz sonucu
        public DateTime Date { get; set; } // Analiz tarihi
        
        public virtual MedicalReport MedicalReport { get; set; }


    }


}
