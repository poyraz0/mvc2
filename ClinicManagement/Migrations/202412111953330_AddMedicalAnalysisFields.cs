namespace ClinicManagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMedicalAnalysisFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MedicalAnalysis", "Result", c => c.String());
            AddColumn("dbo.MedicalAnalysis", "Date", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.MedicalAnalysis", "Date");
            DropColumn("dbo.MedicalAnalysis", "Result");
        }
    }
}
