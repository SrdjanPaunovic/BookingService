namespace BookingApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mgr_v1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AccommodationTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Accomodations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        Address = c.String(),
                        AverageGrade = c.String(),
                        Latitude = c.String(),
                        Longitude = c.String(),
                        ImageURL = c.String(),
                        Approved = c.String(),
                        AccommodationTypeId = c.Int(nullable: false),
                        User_Id = c.Int(nullable: false),
                        Place_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AccommodationTypes", t => t.AccommodationTypeId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.User_Id, cascadeDelete: true)
                .ForeignKey("dbo.Places", t => t.Place_Id, cascadeDelete: true)
                .Index(t => t.AccommodationTypeId)
                .Index(t => t.User_Id)
                .Index(t => t.Place_Id);
            
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Text = c.String(),
                        Accomodation_Id = c.Int(),
                        User_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Accomodations", t => t.Accomodation_Id)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .Index(t => t.Accomodation_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Username = c.String(),
                        Email = c.String(),
                        Password = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.RoomReseravtions",
                c => new
                    {
                        Room_Id = c.Int(nullable: false),
                        User_Id = c.Int(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        EndTime = c.DateTime(nullable: false),
                        Timestamp = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => new { t.Room_Id, t.User_Id })
                .ForeignKey("dbo.Rooms", t => t.Room_Id, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.User_Id, cascadeDelete: true)
                .Index(t => t.Room_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.Rooms",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RoomNumber = c.String(),
                        BedCount = c.String(),
                        Description = c.String(),
                        PricePerNight = c.String(),
                        Accomodation_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Accomodations", t => t.Accomodation_Id)
                .Index(t => t.Accomodation_Id);
            
            CreateTable(
                "dbo.Places",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Region_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Regions", t => t.Region_Id)
                .Index(t => t.Region_Id);
            
            CreateTable(
                "dbo.Regions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Country_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Countries", t => t.Country_Id)
                .Index(t => t.Country_Id);
            
            CreateTable(
                "dbo.Countries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Code = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Places", "Region_Id", "dbo.Regions");
            DropForeignKey("dbo.Regions", "Country_Id", "dbo.Countries");
            DropForeignKey("dbo.Accomodations", "Place_Id", "dbo.Places");
            DropForeignKey("dbo.RoomReseravtions", "User_Id", "dbo.Users");
            DropForeignKey("dbo.RoomReseravtions", "Room_Id", "dbo.Rooms");
            DropForeignKey("dbo.Rooms", "Accomodation_Id", "dbo.Accomodations");
            DropForeignKey("dbo.Comments", "User_Id", "dbo.Users");
            DropForeignKey("dbo.Accomodations", "User_Id", "dbo.Users");
            DropForeignKey("dbo.Comments", "Accomodation_Id", "dbo.Accomodations");
            DropForeignKey("dbo.Accomodations", "AccommodationTypeId", "dbo.AccommodationTypes");
            DropIndex("dbo.Regions", new[] { "Country_Id" });
            DropIndex("dbo.Places", new[] { "Region_Id" });
            DropIndex("dbo.Rooms", new[] { "Accomodation_Id" });
            DropIndex("dbo.RoomReseravtions", new[] { "User_Id" });
            DropIndex("dbo.RoomReseravtions", new[] { "Room_Id" });
            DropIndex("dbo.Comments", new[] { "User_Id" });
            DropIndex("dbo.Comments", new[] { "Accomodation_Id" });
            DropIndex("dbo.Accomodations", new[] { "Place_Id" });
            DropIndex("dbo.Accomodations", new[] { "User_Id" });
            DropIndex("dbo.Accomodations", new[] { "AccommodationTypeId" });
            DropTable("dbo.Countries");
            DropTable("dbo.Regions");
            DropTable("dbo.Places");
            DropTable("dbo.Rooms");
            DropTable("dbo.RoomReseravtions");
            DropTable("dbo.Users");
            DropTable("dbo.Comments");
            DropTable("dbo.Accomodations");
            DropTable("dbo.AccommodationTypes");
        }
    }
}
