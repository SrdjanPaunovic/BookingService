namespace BookingApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mgrv3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AppUsers", "UserName", c => c.String(maxLength: 100));
            DropColumn("dbo.AppUsers", "FullName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AppUsers", "FullName", c => c.String(maxLength: 100));
            DropColumn("dbo.AppUsers", "UserName");
        }
    }
}
