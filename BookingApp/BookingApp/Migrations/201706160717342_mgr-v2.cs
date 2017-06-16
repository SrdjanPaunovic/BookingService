namespace BookingApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mgrv2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AppUsers", "UserName", c => c.String(maxLength: 100));
            AddColumn("dbo.AppUsers", "IsForbidden", c => c.Boolean(nullable: false));
            AddColumn("dbo.Comments", "Rate", c => c.Int(nullable: false));
            AlterColumn("dbo.Accomodations", "AverageGrade", c => c.Double(nullable: false));
            AlterColumn("dbo.Accomodations", "Latitude", c => c.Double(nullable: false));
            AlterColumn("dbo.Accomodations", "Longitude", c => c.Double(nullable: false));
            AlterColumn("dbo.Accomodations", "Approved", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Countries", "Name", c => c.String(nullable: false, maxLength: 30));
            CreateIndex("dbo.Places", "Name", unique: true);
            CreateIndex("dbo.Regions", "Name", unique: true);
            CreateIndex("dbo.Countries", "Name", unique: true);
            DropColumn("dbo.AppUsers", "FullName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AppUsers", "FullName", c => c.String(maxLength: 100));
            DropIndex("dbo.Countries", new[] { "Name" });
            DropIndex("dbo.Regions", new[] { "Name" });
            DropIndex("dbo.Places", new[] { "Name" });
            AlterColumn("dbo.Countries", "Name", c => c.String(maxLength: 30));
            AlterColumn("dbo.Accomodations", "Approved", c => c.String());
            AlterColumn("dbo.Accomodations", "Longitude", c => c.String());
            AlterColumn("dbo.Accomodations", "Latitude", c => c.String());
            AlterColumn("dbo.Accomodations", "AverageGrade", c => c.String());
            DropColumn("dbo.Comments", "Rate");
            DropColumn("dbo.AppUsers", "IsForbidden");
            DropColumn("dbo.AppUsers", "UserName");
        }
    }
}
