namespace BookingApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mgrv2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AppUsers", "IsForbidden", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Accomodations", "Latitude", c => c.Double(nullable: false));
            AlterColumn("dbo.Accomodations", "Longitude", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Accomodations", "Longitude", c => c.String());
            AlterColumn("dbo.Accomodations", "Latitude", c => c.String());
            DropColumn("dbo.AppUsers", "IsForbidden");
        }
    }
}
