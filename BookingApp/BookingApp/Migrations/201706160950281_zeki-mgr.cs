namespace BookingApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class zekimgr : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.RoomReseravtions");
            AddColumn("dbo.RoomReseravtions", "Id", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.RoomReseravtions", "Id");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.RoomReseravtions");
            DropColumn("dbo.RoomReseravtions", "Id");
            AddPrimaryKey("dbo.RoomReseravtions", new[] { "Room_Id", "AppUser_Id" });
        }
    }
}
