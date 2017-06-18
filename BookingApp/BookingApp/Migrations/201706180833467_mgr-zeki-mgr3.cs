namespace BookingApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mgrzekimgr3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Accomodations", "ImageURLs", c => c.String());
            AlterColumn("dbo.Rooms", "BedCount", c => c.Int(nullable: false));
            AlterColumn("dbo.Rooms", "PricePerNight", c => c.Double(nullable: false));
            DropColumn("dbo.Accomodations", "ImageURL");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Accomodations", "ImageURL", c => c.String());
            AlterColumn("dbo.Rooms", "PricePerNight", c => c.String());
            AlterColumn("dbo.Rooms", "BedCount", c => c.String());
            DropColumn("dbo.Accomodations", "ImageURLs");
        }
    }
}
