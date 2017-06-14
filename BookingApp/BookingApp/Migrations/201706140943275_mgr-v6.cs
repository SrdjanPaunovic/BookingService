namespace BookingApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mgrv6 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Countries", "Name", c => c.String(nullable: false, maxLength: 30));
            CreateIndex("dbo.Places", "Name", unique: true);
            CreateIndex("dbo.Regions", "Name", unique: true);
            CreateIndex("dbo.Countries", "Name", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.Countries", new[] { "Name" });
            DropIndex("dbo.Regions", new[] { "Name" });
            DropIndex("dbo.Places", new[] { "Name" });
            AlterColumn("dbo.Countries", "Name", c => c.String(maxLength: 30));
        }
    }
}
