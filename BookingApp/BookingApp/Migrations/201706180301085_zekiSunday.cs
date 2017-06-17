namespace BookingApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class zekiSunday : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Rooms", "BedCount", c => c.Int(nullable: false));
            AlterColumn("dbo.Rooms", "PricePerNight", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Rooms", "PricePerNight", c => c.String());
            AlterColumn("dbo.Rooms", "BedCount", c => c.String());
        }
    }
}
