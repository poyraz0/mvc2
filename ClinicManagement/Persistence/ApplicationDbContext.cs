using ClinicManagement.Core.Models;
using ClinicManagement.Persistence.EntityConfigurations;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;


namespace ClinicManagement.Persistence
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Specialization> Specializations { get; set; }
        public DbSet<City> Cities { get; set; }
        //public DbSet<PatientStatus> PatientStatus { get; set; }

        public DbSet<MedicalReport> MedicalReports { get; set; }
      
        public DbSet<MedicalAnalysis> MedicalAnalysis { get; set; }





        public ApplicationDbContext()
            : base("ClinicDB", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new PatientConfiguration());
            modelBuilder.Configurations.Add(new AppointmentConfiguration());
            modelBuilder.Configurations.Add(new DoctorConfiguration());
            modelBuilder.Configurations.Add(new AttendanceConfiguration());
            modelBuilder.Configurations.Add(new SpecializationConfiguration());
            modelBuilder.Configurations.Add(new CityConfiguration());

            modelBuilder.Entity<MedicalAnalysis>()
               .HasRequired(ma => ma.MedicalReport)
               .WithMany()
               .HasForeignKey(ma => ma.MedicalReportId)
               .WillCascadeOnDelete(false); // İsteğe bağlı

            //modelBuilder.Configurations.Add(new PatientStatusConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}