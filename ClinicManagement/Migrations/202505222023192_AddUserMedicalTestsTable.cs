namespace ClinicManagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserMedicalTestsTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserMedicalTests",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ApplicationUserId = c.String(nullable: false, maxLength: 128),
                        FileName = c.String(nullable: false, maxLength: 255),
                        FilePath = c.String(nullable: false),
                        UploadDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserId, cascadeDelete: true)
                .Index(t => t.ApplicationUserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserMedicalTests", "ApplicationUserId", "dbo.AspNetUsers");
            DropIndex("dbo.UserMedicalTests", new[] { "ApplicationUserId" });
            DropTable("dbo.UserMedicalTests");
        }
    }
}
