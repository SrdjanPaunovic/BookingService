namespace BookingApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mgrv5 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Accomodations", "Approved", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Accomodations", "Approved", c => c.String());
        }
    }
}
