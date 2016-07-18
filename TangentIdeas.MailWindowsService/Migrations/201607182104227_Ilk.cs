namespace TangentIdeas.MailWindowsService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Ilk : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "System.Mail",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Subject = c.String(),
                        Message = c.String(),
                        Status = c.Int(nullable: false),
                        MailSender = c.Int(nullable: false),
                        SendTime = c.DateTime(nullable: false),
                        Exception = c.String(),
                        Active = c.Boolean(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        CreatedBy = c.Int(nullable: false),
                        UpdatedAt = c.DateTime(),
                        UpdatedBy = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "System.MailTargets",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MailAddres = c.String(),
                        Active = c.Boolean(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        CreatedBy = c.Int(nullable: false),
                        UpdatedAt = c.DateTime(),
                        UpdatedBy = c.Int(),
                        Mail_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("System.Mail", t => t.Mail_Id)
                .Index(t => t.Mail_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("System.MailTargets", "Mail_Id", "System.Mail");
            DropIndex("System.MailTargets", new[] { "Mail_Id" });
            DropTable("System.MailTargets");
            DropTable("System.Mail");
        }
    }
}
