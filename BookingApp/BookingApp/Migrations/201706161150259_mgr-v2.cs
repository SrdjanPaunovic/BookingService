namespace BookingApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mgrv2 : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.RoomReseravtions");
            AddColumn("dbo.AppUsers", "UserName", c => c.String(maxLength: 100));
            AddColumn("dbo.AppUsers", "IsForbidden", c => c.Boolean(nullable: false));
            AddColumn("dbo.Comments", "Rate", c => c.Int(nullable: false));
            AddColumn("dbo.RoomReseravtions", "Id", c => c.Int(nullable: false, identity: true));
            AddColumn("dbo.RoomReseravtions", "StartTime", c => c.DateTime());
            AlterColumn("dbo.Accomodations", "AverageGrade", c => c.Double(nullable: false));
            AlterColumn("dbo.Accomodations", "Latitude", c => c.Double(nullable: false));
            AlterColumn("dbo.Accomodations", "Longitude", c => c.Double(nullable: false));
            AlterColumn("dbo.Accomodations", "Approved", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Countries", "Name", c => c.String(nullable: false, maxLength: 30));
            AlterColumn("dbo.RoomReseravtions", "EndTime", c => c.DateTime());
            AlterColumn("dbo.RoomReseravtions", "Timestamp", c => c.DateTime());
            AddPrimaryKey("dbo.RoomReseravtions", "Id");
            CreateIndex("dbo.Places", "Name", unique: true);
            CreateIndex("dbo.Regions", "Name", unique: true);
            CreateIndex("dbo.Countries", "Name", unique: true);
            DropColumn("dbo.AppUsers", "FullName");
            DropColumn("dbo.RoomReseravtions", "StartDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.RoomReseravtions", "StartDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.AppUsers", "FullName", c => c.String(maxLength: 100));
            DropIndex("dbo.Countries", new[] { "Name" });
            DropIndex("dbo.Regions", new[] { "Name" });
            DropIndex("dbo.Places", new[] { "Name" });
            DropPrimaryKey("dbo.RoomReseravtions");
            AlterColumn("dbo.RoomReseravtions", "Timestamp", c => c.DateTime(nullable: false));
            AlterColumn("dbo.RoomReseravtions", "EndTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Countries", "Name", c => c.String(maxLength: 30));
            AlterColumn("dbo.Accomodations", "Approved", c => c.String());
            AlterColumn("dbo.Accomodations", "Longitude", c => c.String());
            AlterColumn("dbo.Accomodations", "Latitude", c => c.String());
            AlterColumn("dbo.Accomodations", "AverageGrade", c => c.String());
            DropColumn("dbo.RoomReseravtions", "StartTime");
            DropColumn("dbo.RoomReseravtions", "Id");
            DropColumn("dbo.Comments", "Rate");
            DropColumn("dbo.AppUsers", "IsForbidden");
            DropColumn("dbo.AppUsers", "UserName");
            AddPrimaryKey("dbo.RoomReseravtions", new[] { "Room_Id", "AppUser_Id" });
        }
    }
}
