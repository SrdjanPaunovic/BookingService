namespace BookingApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mgrv1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AccommodationTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 30),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Accomodations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 30),
                        Description = c.String(),
                        Address = c.String(maxLength: 50),
                        AverageGrade = c.String(),
                        Latitude = c.String(),
                        Longitude = c.String(),
                        ImageURL = c.String(),
                        Approved = c.String(),
                        AccommodationType_Id = c.Int(nullable: false),
                        AppUser_Id = c.Int(nullable: false),
                        Place_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AccommodationTypes", t => t.AccommodationType_Id, cascadeDelete: true)
                .ForeignKey("dbo.AppUsers", t => t.AppUser_Id, cascadeDelete: false)
                .ForeignKey("dbo.Places", t => t.Place_Id, cascadeDelete: false)
                .Index(t => t.AccommodationType_Id)
                .Index(t => t.AppUser_Id)
                .Index(t => t.Place_Id);
            
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Text = c.String(),
                        Accomodation_Id = c.Int(nullable: false),
                        AppUser_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Accomodations", t => t.Accomodation_Id, cascadeDelete: false)
                .ForeignKey("dbo.AppUsers", t => t.AppUser_Id, cascadeDelete: false)
                .Index(t => t.Accomodation_Id)
                .Index(t => t.AppUser_Id);
            
            CreateTable(
                "dbo.Places",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 30),
                        Region_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Regions", t => t.Region_Id, cascadeDelete: true)
                .Index(t => t.Region_Id);
            
            CreateTable(
                "dbo.Regions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 30),
                        Country_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Countries", t => t.Country_Id, cascadeDelete: true)
                .Index(t => t.Country_Id);
            
            CreateTable(
                "dbo.Countries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 30),
                        Code = c.String(maxLength: 10),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Rooms",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RoomNumber = c.String(),
                        BedCount = c.String(),
                        Description = c.String(),
                        PricePerNight = c.String(),
                        Accomodation_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Accomodations", t => t.Accomodation_Id, cascadeDelete: false)
                .Index(t => t.Accomodation_Id);
            
            CreateTable(
                "dbo.RoomReseravtions",
                c => new
                    {
                        Room_Id = c.Int(nullable: false),
                        AppUser_Id = c.Int(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        EndTime = c.DateTime(nullable: false),
                        Timestamp = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => new { t.Room_Id, t.AppUser_Id })
                .ForeignKey("dbo.AppUsers", t => t.AppUser_Id, cascadeDelete: false)
                .ForeignKey("dbo.Rooms", t => t.Room_Id, cascadeDelete: false)
                .Index(t => t.Room_Id)
                .Index(t => t.AppUser_Id);
            
            AddColumn("dbo.AspNetUsers", "appUserId", c => c.Int(nullable: false));
            AlterColumn("dbo.AppUsers", "FullName", c => c.String(maxLength: 100));
            CreateIndex("dbo.AspNetUsers", "appUserId");
            AddForeignKey("dbo.AspNetUsers", "appUserId", "dbo.AppUsers", "Id", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "appUserId", "dbo.AppUsers");
            DropForeignKey("dbo.RoomReseravtions", "Room_Id", "dbo.Rooms");
            DropForeignKey("dbo.RoomReseravtions", "AppUser_Id", "dbo.AppUsers");
            DropForeignKey("dbo.Rooms", "Accomodation_Id", "dbo.Accomodations");
            DropForeignKey("dbo.Accomodations", "Place_Id", "dbo.Places");
            DropForeignKey("dbo.Places", "Region_Id", "dbo.Regions");
            DropForeignKey("dbo.Regions", "Country_Id", "dbo.Countries");
            DropForeignKey("dbo.Comments", "AppUser_Id", "dbo.AppUsers");
            DropForeignKey("dbo.Comments", "Accomodation_Id", "dbo.Accomodations");
            DropForeignKey("dbo.Accomodations", "AppUser_Id", "dbo.AppUsers");
            DropForeignKey("dbo.Accomodations", "AccommodationType_Id", "dbo.AccommodationTypes");
            DropIndex("dbo.AspNetUsers", new[] { "appUserId" });
            DropIndex("dbo.RoomReseravtions", new[] { "AppUser_Id" });
            DropIndex("dbo.RoomReseravtions", new[] { "Room_Id" });
            DropIndex("dbo.Rooms", new[] { "Accomodation_Id" });
            DropIndex("dbo.Regions", new[] { "Country_Id" });
            DropIndex("dbo.Places", new[] { "Region_Id" });
            DropIndex("dbo.Comments", new[] { "AppUser_Id" });
            DropIndex("dbo.Comments", new[] { "Accomodation_Id" });
            DropIndex("dbo.Accomodations", new[] { "Place_Id" });
            DropIndex("dbo.Accomodations", new[] { "AppUser_Id" });
            DropIndex("dbo.Accomodations", new[] { "AccommodationType_Id" });
            AlterColumn("dbo.AppUsers", "FullName", c => c.Int(nullable: false));
            DropColumn("dbo.AspNetUsers", "appUserId");
            DropTable("dbo.RoomReseravtions");
            DropTable("dbo.Rooms");
            DropTable("dbo.Countries");
            DropTable("dbo.Regions");
            DropTable("dbo.Places");
            DropTable("dbo.Comments");
            DropTable("dbo.Accomodations");
            DropTable("dbo.AccommodationTypes");
        }
    }
}
