namespace BookingApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mg4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Accomodations", "ImageURLs", c => c.String());
            DropColumn("dbo.Accomodations", "ImageURL");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Accomodations", "ImageURL", c => c.String());
            DropColumn("dbo.Accomodations", "ImageURLs");
        }
    }
}
