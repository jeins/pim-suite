namespace PIMSuite.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Departments",
                c => new
                    {
                        Name = c.String(nullable: false, maxLength: 128),
                        Creation = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Name);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserId = c.Guid(nullable: false),
                        FirstName = c.String(nullable: false),
                        Lastname = c.String(nullable: false),
                        Username = c.String(nullable: false, maxLength: 20),
                        Email = c.String(nullable: false),
                        DepartamentName = c.String(nullable: false, maxLength: 128),
                        PhoneNumber = c.String(nullable: false),
                        LocationName = c.String(nullable: false, maxLength: 128),
                        Password = c.String(nullable: false),
                        Creation = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.Departments", t => t.DepartamentName, cascadeDelete: true)
                .ForeignKey("dbo.Locations", t => t.LocationName, cascadeDelete: true)
                .Index(t => t.DepartamentName)
                .Index(t => t.LocationName);
            
            CreateTable(
                "dbo.Locations",
                c => new
                    {
                        Name = c.String(nullable: false, maxLength: 128),
                        Creation = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Name);
            
            CreateTable(
                "dbo.Leaderships",
                c => new
                    {
                        LeadershipId = c.Guid(nullable: false),
                        UserId = c.Guid(nullable: false),
                        DepartmentName = c.String(),
                        Chief = c.Boolean(nullable: false),
                        Creation = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.LeadershipId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Users", "LocationName", "dbo.Locations");
            DropForeignKey("dbo.Users", "DepartamentName", "dbo.Departments");
            DropIndex("dbo.Users", new[] { "LocationName" });
            DropIndex("dbo.Users", new[] { "DepartamentName" });
            DropTable("dbo.Leaderships");
            DropTable("dbo.Locations");
            DropTable("dbo.Users");
            DropTable("dbo.Departments");
        }
    }
}
