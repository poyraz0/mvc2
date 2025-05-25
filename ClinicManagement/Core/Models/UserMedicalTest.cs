using System;
using System.ComponentModel.DataAnnotations;

namespace ClinicManagement.Core.Models
{
    public class UserMedicalTest
    {
        public int Id { get; set; }

        [Required]
        public string ApplicationUserId { get; set; }

        [Required]
        [StringLength(255)]
        public string FileName { get; set; }

        [Required]
        public string FilePath { get; set; }

        public DateTime UploadDate { get; set; }

        // Navigation property
        public ApplicationUser ApplicationUser { get; set; }
    }
} 