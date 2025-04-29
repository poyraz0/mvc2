namespace ClinicManagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialDb1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MedicalAnalysis",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MedicalReportId = c.Int(nullable: false),
                        AnalizTarihi = c.DateTime(nullable: false),
                        Sonuc = c.String(),
                        Detaylar = c.String(),
                        MedicalReport_ReportId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MedicalReports", t => t.MedicalReport_ReportId)
                .ForeignKey("dbo.MedicalReports", t => t.MedicalReportId)
                .Index(t => t.MedicalReportId)
                .Index(t => t.MedicalReport_ReportId);
            
            AlterColumn("dbo.MedicalReports", "UploadDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MedicalAnalysis", "MedicalReportId", "dbo.MedicalReports");
            DropForeignKey("dbo.MedicalAnalysis", "MedicalReport_ReportId", "dbo.MedicalReports");
            DropIndex("dbo.MedicalAnalysis", new[] { "MedicalReport_ReportId" });
            DropIndex("dbo.MedicalAnalysis", new[] { "MedicalReportId" });
            AlterColumn("dbo.MedicalReports", "UploadDate", c => c.DateTime(nullable: false));
            DropTable("dbo.MedicalAnalysis");
        }
    }
}
