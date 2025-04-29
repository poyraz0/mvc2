namespace ClinicManagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPrimaryKeyToMedicalReport : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MedicalReports",
                c => new
                    {
                        ReportId = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                        ReportPath = c.String(),
                        AnalysisResult = c.String(),
                        UploadDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ReportId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.MedicalReports");
        }
    }
}
