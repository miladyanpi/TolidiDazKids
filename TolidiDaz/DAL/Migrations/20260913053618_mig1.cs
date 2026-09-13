using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class mig1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "About",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShortDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LongDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JsonPicture = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdentityCode = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RegisterDate = table.Column<int>(type: "int", nullable: false),
                    RegisterTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EditDate = table.Column<int>(type: "int", nullable: false),
                    EditTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    JsonLableTexts = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Visible = table.Column<bool>(type: "bit", nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_About", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Advertisement",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SiteName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SiteUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JsonPicture = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdentityCode = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RegisterDate = table.Column<int>(type: "int", nullable: false),
                    RegisterTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EditDate = table.Column<int>(type: "int", nullable: false),
                    EditTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    JsonLableTexts = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Visible = table.Column<bool>(type: "bit", nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Advertisement", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "AdvertisementSingle",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SiteName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SiteUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JsonPicture = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdentityCode = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RegisterDate = table.Column<int>(type: "int", nullable: false),
                    RegisterTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EditDate = table.Column<int>(type: "int", nullable: false),
                    EditTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    JsonLableTexts = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Visible = table.Column<bool>(type: "bit", nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdvertisementSingle", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParentID = table.Column<int>(type: "int", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JsonPicture = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Order = table.Column<int>(type: "int", nullable: false),
                    IdentityCode = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RegisterDate = table.Column<int>(type: "int", nullable: false),
                    RegisterTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EditDate = table.Column<int>(type: "int", nullable: false),
                    EditTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    JsonLableTexts = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Visible = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Category_Category_ParentID",
                        column: x => x.ParentID,
                        principalTable: "Category",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ContactUs",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Mobile = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IpAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    IdentityCode = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RegisterDate = table.Column<int>(type: "int", nullable: false),
                    RegisterTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EditDate = table.Column<int>(type: "int", nullable: false),
                    EditTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    JsonLableTexts = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Visible = table.Column<bool>(type: "bit", nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactUs", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Mcode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gender = table.Column<int>(type: "int", nullable: true),
                    BirthDate = table.Column<int>(type: "int", nullable: true),
                    Mobile = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JsonPicture = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PostalCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdentityCode = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RegisterDate = table.Column<int>(type: "int", nullable: false),
                    RegisterTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EditDate = table.Column<int>(type: "int", nullable: false),
                    EditTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    JsonLableTexts = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Visible = table.Column<bool>(type: "bit", nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Department",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdentityCode = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RegisterDate = table.Column<int>(type: "int", nullable: false),
                    RegisterTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EditDate = table.Column<int>(type: "int", nullable: false),
                    EditTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    JsonLableTexts = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Visible = table.Column<bool>(type: "bit", nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Department", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Faq",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Mobile = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IpAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    IdentityCode = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RegisterDate = table.Column<int>(type: "int", nullable: false),
                    RegisterTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EditDate = table.Column<int>(type: "int", nullable: false),
                    EditTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    JsonLableTexts = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Visible = table.Column<bool>(type: "bit", nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Faq", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "GroupBlog",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdentityCode = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RegisterDate = table.Column<int>(type: "int", nullable: false),
                    RegisterTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EditDate = table.Column<int>(type: "int", nullable: false),
                    EditTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    JsonLableTexts = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Visible = table.Column<bool>(type: "bit", nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupBlog", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "GroupQuestion",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdentityCode = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RegisterDate = table.Column<int>(type: "int", nullable: false),
                    RegisterTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EditDate = table.Column<int>(type: "int", nullable: false),
                    EditTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    JsonLableTexts = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Visible = table.Column<bool>(type: "bit", nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupQuestion", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "OrderPaymentTemp",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ResNum = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Amount = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdentityCode = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RegisterDate = table.Column<int>(type: "int", nullable: false),
                    RegisterTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EditDate = table.Column<int>(type: "int", nullable: false),
                    EditTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    JsonLableTexts = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Visible = table.Column<bool>(type: "bit", nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderPaymentTemp", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Position",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CostType = table.Column<int>(type: "int", nullable: true),
                    Price = table.Column<long>(type: "bigint", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdentityCode = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RegisterDate = table.Column<int>(type: "int", nullable: false),
                    RegisterTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EditDate = table.Column<int>(type: "int", nullable: false),
                    EditTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    JsonLableTexts = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Visible = table.Column<bool>(type: "bit", nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Position", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Province",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdentityCode = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RegisterDate = table.Column<int>(type: "int", nullable: false),
                    RegisterTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EditDate = table.Column<int>(type: "int", nullable: false),
                    EditTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    JsonLableTexts = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Visible = table.Column<bool>(type: "bit", nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Province", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "RawProduct",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdentityCode = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RegisterDate = table.Column<int>(type: "int", nullable: false),
                    RegisterTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EditDate = table.Column<int>(type: "int", nullable: false),
                    EditTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    JsonLableTexts = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Visible = table.Column<bool>(type: "bit", nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RawProduct", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Reminder",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<int>(type: "int", nullable: false),
                    Time = table.Column<TimeSpan>(type: "time", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SendSms = table.Column<bool>(type: "bit", nullable: false),
                    IdentityCode = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RegisterDate = table.Column<int>(type: "int", nullable: false),
                    RegisterTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EditDate = table.Column<int>(type: "int", nullable: false),
                    EditTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    JsonLableTexts = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Visible = table.Column<bool>(type: "bit", nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reminder", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ReminderEvent",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SendSms = table.Column<bool>(type: "bit", nullable: false),
                    Time = table.Column<TimeSpan>(type: "time", nullable: false),
                    ReminderType = table.Column<int>(type: "int", nullable: false),
                    IdentityCode = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RegisterDate = table.Column<int>(type: "int", nullable: false),
                    RegisterTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EditDate = table.Column<int>(type: "int", nullable: false),
                    EditTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    JsonLableTexts = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Visible = table.Column<bool>(type: "bit", nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReminderEvent", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SendProductMethod",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdentityCode = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RegisterDate = table.Column<int>(type: "int", nullable: false),
                    RegisterTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EditDate = table.Column<int>(type: "int", nullable: false),
                    EditTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    JsonLableTexts = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Visible = table.Column<bool>(type: "bit", nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SendProductMethod", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Setting",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CountShowRecord = table.Column<int>(type: "int", nullable: false),
                    JsonPicture = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneSender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CurrencyUnit = table.Column<int>(type: "int", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JsonTel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JsonMobile = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdentityCode = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RegisterDate = table.Column<int>(type: "int", nullable: false),
                    RegisterTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EditDate = table.Column<int>(type: "int", nullable: false),
                    EditTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    JsonLableTexts = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Visible = table.Column<bool>(type: "bit", nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Setting", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Slider",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JsonPicture = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JsonVideo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdentityCode = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RegisterDate = table.Column<int>(type: "int", nullable: false),
                    RegisterTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EditDate = table.Column<int>(type: "int", nullable: false),
                    EditTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    JsonLableTexts = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Visible = table.Column<bool>(type: "bit", nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Slider", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SmsOtpCode",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateExpired = table.Column<int>(type: "int", nullable: false),
                    TimeExpired = table.Column<TimeSpan>(type: "time", nullable: false),
                    IdentityCode = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RegisterDate = table.Column<int>(type: "int", nullable: false),
                    RegisterTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EditDate = table.Column<int>(type: "int", nullable: false),
                    EditTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    JsonLableTexts = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Visible = table.Column<bool>(type: "bit", nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SmsOtpCode", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Story",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JsonPicture = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JsonVideo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdentityCode = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RegisterDate = table.Column<int>(type: "int", nullable: false),
                    RegisterTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EditDate = table.Column<int>(type: "int", nullable: false),
                    EditTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    JsonLableTexts = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Visible = table.Column<bool>(type: "bit", nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Story", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Team",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JsonPictures = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShowInAbout = table.Column<bool>(type: "bit", nullable: false),
                    IdentityCode = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RegisterDate = table.Column<int>(type: "int", nullable: false),
                    RegisterTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EditDate = table.Column<int>(type: "int", nullable: false),
                    EditTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    JsonLableTexts = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Visible = table.Column<bool>(type: "bit", nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Team", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryID = table.Column<int>(type: "int", nullable: true),
                    ProductCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Brand = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SkuCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<long>(type: "bigint", nullable: false),
                    Discount = table.Column<long>(type: "bigint", nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false),
                    ProductExistStatus = table.Column<int>(type: "int", nullable: true),
                    JsonPicture = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShortDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ViewCount = table.Column<int>(type: "int", nullable: false),
                    IdentityCode = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RegisterDate = table.Column<int>(type: "int", nullable: false),
                    RegisterTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EditDate = table.Column<int>(type: "int", nullable: false),
                    EditTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    JsonLableTexts = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Visible = table.Column<bool>(type: "bit", nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Product_Category_CategoryID",
                        column: x => x.CategoryID,
                        principalTable: "Category",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductFeature",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryID = table.Column<int>(type: "int", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdentityCode = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RegisterDate = table.Column<int>(type: "int", nullable: false),
                    RegisterTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EditDate = table.Column<int>(type: "int", nullable: false),
                    EditTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    JsonLableTexts = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Visible = table.Column<bool>(type: "bit", nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductFeature", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ProductFeature_Category_CategoryID",
                        column: x => x.CategoryID,
                        principalTable: "Category",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CustomerID = table.Column<int>(type: "int", nullable: true),
                    CustomerID1 = table.Column<int>(type: "int", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Customer_CustomerID",
                        column: x => x.CustomerID,
                        principalTable: "Customer",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Customer_CustomerID1",
                        column: x => x.CustomerID1,
                        principalTable: "Customer",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "SmsLog",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerID = table.Column<int>(type: "int", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Mobile = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StatusText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SendDate = table.Column<int>(type: "int", nullable: true),
                    Time = table.Column<TimeSpan>(type: "time", nullable: true),
                    IdentityCode = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RegisterDate = table.Column<int>(type: "int", nullable: false),
                    RegisterTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EditDate = table.Column<int>(type: "int", nullable: false),
                    EditTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    JsonLableTexts = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Visible = table.Column<bool>(type: "bit", nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SmsLog", x => x.ID);
                    table.ForeignKey(
                        name: "FK_SmsLog_Customer_CustomerID",
                        column: x => x.CustomerID,
                        principalTable: "Customer",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Wallet",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerID = table.Column<int>(type: "int", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Balance = table.Column<long>(type: "bigint", nullable: false),
                    GiftCredit = table.Column<long>(type: "bigint", nullable: false),
                    IdentityCode = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RegisterDate = table.Column<int>(type: "int", nullable: false),
                    RegisterTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EditDate = table.Column<int>(type: "int", nullable: false),
                    EditTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    JsonLableTexts = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Visible = table.Column<bool>(type: "bit", nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wallet", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Wallet_Customer_CustomerID",
                        column: x => x.CustomerID,
                        principalTable: "Customer",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Ticket",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DepartmentID = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TicketPriority = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JsonTicketFile = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdentityCode = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RegisterDate = table.Column<int>(type: "int", nullable: false),
                    RegisterTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EditDate = table.Column<int>(type: "int", nullable: false),
                    EditTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    JsonLableTexts = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Visible = table.Column<bool>(type: "bit", nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ticket", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Ticket_Department_DepartmentID",
                        column: x => x.DepartmentID,
                        principalTable: "Department",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Question",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GroupQuestionID = table.Column<int>(type: "int", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdentityCode = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RegisterDate = table.Column<int>(type: "int", nullable: false),
                    RegisterTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EditDate = table.Column<int>(type: "int", nullable: false),
                    EditTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    JsonLableTexts = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Visible = table.Column<bool>(type: "bit", nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Question", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Question_GroupQuestion_GroupQuestionID",
                        column: x => x.GroupQuestionID,
                        principalTable: "GroupQuestion",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Personel",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PositionID = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gender = table.Column<int>(type: "int", nullable: true),
                    BirthDate = table.Column<int>(type: "int", nullable: true),
                    Mobile = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JsonPicture = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdentityCode = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RegisterDate = table.Column<int>(type: "int", nullable: false),
                    RegisterTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EditDate = table.Column<int>(type: "int", nullable: false),
                    EditTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    JsonLableTexts = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Visible = table.Column<bool>(type: "bit", nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Personel", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Personel_Position_PositionID",
                        column: x => x.PositionID,
                        principalTable: "Position",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "City",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProvinceID = table.Column<int>(type: "int", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdentityCode = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RegisterDate = table.Column<int>(type: "int", nullable: false),
                    RegisterTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EditDate = table.Column<int>(type: "int", nullable: false),
                    EditTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    JsonLableTexts = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Visible = table.Column<bool>(type: "bit", nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_City", x => x.ID);
                    table.ForeignKey(
                        name: "FK_City_Province_ProvinceID",
                        column: x => x.ProvinceID,
                        principalTable: "Province",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RawProductStore",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RawProductID = table.Column<int>(type: "int", nullable: true),
                    MessurmentType = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<long>(type: "bigint", nullable: false),
                    BuyDate = table.Column<int>(type: "int", nullable: false),
                    IdentityCode = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RegisterDate = table.Column<int>(type: "int", nullable: false),
                    RegisterTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EditDate = table.Column<int>(type: "int", nullable: false),
                    EditTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    JsonLableTexts = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Visible = table.Column<bool>(type: "bit", nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RawProductStore", x => x.ID);
                    table.ForeignKey(
                        name: "FK_RawProductStore_RawProduct_RawProductID",
                        column: x => x.RawProductID,
                        principalTable: "RawProduct",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Cart",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerID = table.Column<int>(type: "int", nullable: true),
                    SendProductMethodID = table.Column<int>(type: "int", nullable: true),
                    CartStatus = table.Column<int>(type: "int", nullable: false),
                    IdentityCode = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RegisterDate = table.Column<int>(type: "int", nullable: false),
                    RegisterTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EditDate = table.Column<int>(type: "int", nullable: false),
                    EditTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    JsonLableTexts = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Visible = table.Column<bool>(type: "bit", nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cart", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Cart_Customer_CustomerID",
                        column: x => x.CustomerID,
                        principalTable: "Customer",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Cart_SendProductMethod_SendProductMethodID",
                        column: x => x.SendProductMethodID,
                        principalTable: "SendProductMethod",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerID = table.Column<int>(type: "int", nullable: true),
                    SendProductMethodID = table.Column<int>(type: "int", nullable: true),
                    OrderCode = table.Column<long>(type: "bigint", nullable: false),
                    OrderStatus = table.Column<int>(type: "int", nullable: false),
                    PaymentStatus = table.Column<int>(type: "int", nullable: false),
                    TotalAmount = table.Column<long>(type: "bigint", nullable: false),
                    Discount = table.Column<long>(type: "bigint", nullable: false),
                    FinalAmount = table.Column<long>(type: "bigint", nullable: false),
                    CardPen = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RefId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResNum = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JsonAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExpireAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StatusDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdentityCode = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RegisterDate = table.Column<int>(type: "int", nullable: false),
                    RegisterTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EditDate = table.Column<int>(type: "int", nullable: false),
                    EditTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    JsonLableTexts = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Visible = table.Column<bool>(type: "bit", nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Order_Customer_CustomerID",
                        column: x => x.CustomerID,
                        principalTable: "Customer",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Order_SendProductMethod_SendProductMethodID",
                        column: x => x.SendProductMethodID,
                        principalTable: "SendProductMethod",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Blog",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GroupBlogID = table.Column<int>(type: "int", nullable: true),
                    TeamID = table.Column<int>(type: "int", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StudyDuration = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumberOfVisits = table.Column<int>(type: "int", nullable: false),
                    JsonPicture = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdentityCode = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RegisterDate = table.Column<int>(type: "int", nullable: false),
                    RegisterTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EditDate = table.Column<int>(type: "int", nullable: false),
                    EditTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    JsonLableTexts = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Visible = table.Column<bool>(type: "bit", nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blog", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Blog_GroupBlog_GroupBlogID",
                        column: x => x.GroupBlogID,
                        principalTable: "GroupBlog",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Blog_Team_TeamID",
                        column: x => x.TeamID,
                        principalTable: "Team",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FavoritUserProduct",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerID = table.Column<int>(type: "int", nullable: true),
                    ProductID = table.Column<int>(type: "int", nullable: true),
                    IdentityCode = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RegisterDate = table.Column<int>(type: "int", nullable: false),
                    RegisterTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EditDate = table.Column<int>(type: "int", nullable: false),
                    EditTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    JsonLableTexts = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Visible = table.Column<bool>(type: "bit", nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavoritUserProduct", x => x.ID);
                    table.ForeignKey(
                        name: "FK_FavoritUserProduct_Customer_CustomerID",
                        column: x => x.CustomerID,
                        principalTable: "Customer",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FavoritUserProduct_Product_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Product",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PricingRule",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductID = table.Column<int>(type: "int", nullable: true),
                    RoleID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RuleType = table.Column<int>(type: "int", nullable: true),
                    MinQuantity = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<long>(type: "bigint", nullable: false),
                    FromDate = table.Column<int>(type: "int", nullable: true),
                    ToDate = table.Column<int>(type: "int", nullable: true),
                    IdentityCode = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RegisterDate = table.Column<int>(type: "int", nullable: false),
                    RegisterTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EditDate = table.Column<int>(type: "int", nullable: false),
                    EditTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    JsonLableTexts = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Visible = table.Column<bool>(type: "bit", nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PricingRule", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PricingRule_Product_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Product",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Product_CountAction_CostType",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PositionID = table.Column<int>(type: "int", nullable: true),
                    ProductID = table.Column<int>(type: "int", nullable: true),
                    CountAction = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<long>(type: "bigint", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdentityCode = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RegisterDate = table.Column<int>(type: "int", nullable: false),
                    RegisterTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EditDate = table.Column<int>(type: "int", nullable: false),
                    EditTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    JsonLableTexts = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Visible = table.Column<bool>(type: "bit", nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product_CountAction_CostType", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Product_CountAction_CostType_Position_PositionID",
                        column: x => x.PositionID,
                        principalTable: "Position",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Product_CountAction_CostType_Product_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Product",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductComment",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductID = table.Column<int>(type: "int", nullable: true),
                    CustomerID = table.Column<int>(type: "int", nullable: true),
                    ParentID = table.Column<int>(type: "int", nullable: true),
                    AuthorName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Body = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Rating = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    IsVerifiedBuyer = table.Column<bool>(type: "bit", nullable: false),
                    LikeCount = table.Column<int>(type: "int", nullable: false),
                    DislikeCount = table.Column<int>(type: "int", nullable: false),
                    IdentityCode = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RegisterDate = table.Column<int>(type: "int", nullable: false),
                    RegisterTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EditDate = table.Column<int>(type: "int", nullable: false),
                    EditTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    JsonLableTexts = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Visible = table.Column<bool>(type: "bit", nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductComment", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ProductComment_Customer_CustomerID",
                        column: x => x.CustomerID,
                        principalTable: "Customer",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductComment_Product_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Product",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductFeatureValue",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductFeatureID = table.Column<int>(type: "int", nullable: true),
                    ProductID = table.Column<int>(type: "int", nullable: true),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdentityCode = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RegisterDate = table.Column<int>(type: "int", nullable: false),
                    RegisterTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EditDate = table.Column<int>(type: "int", nullable: false),
                    EditTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    JsonLableTexts = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Visible = table.Column<bool>(type: "bit", nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductFeatureValue", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ProductFeatureValue_ProductFeature_ProductFeatureID",
                        column: x => x.ProductFeatureID,
                        principalTable: "ProductFeature",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductFeatureValue_Product_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Product",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RefreshTokenEntity",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DeviceId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsRevoked = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReplacedByToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RemoteIpAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdentityCode = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RegisterDate = table.Column<int>(type: "int", nullable: false),
                    RegisterTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EditDate = table.Column<int>(type: "int", nullable: false),
                    EditTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    JsonLableTexts = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Visible = table.Column<bool>(type: "bit", nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokenEntity", x => x.ID);
                    table.ForeignKey(
                        name: "FK_RefreshTokenEntity_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WalletTransaction",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WalletID = table.Column<int>(type: "int", nullable: true),
                    Kind = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<long>(type: "bigint", nullable: false),
                    AfterBalance = table.Column<long>(type: "bigint", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RefID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CardPen = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExternalReference = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdentityCode = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RegisterDate = table.Column<int>(type: "int", nullable: false),
                    RegisterTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EditDate = table.Column<int>(type: "int", nullable: false),
                    EditTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    JsonLableTexts = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Visible = table.Column<bool>(type: "bit", nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WalletTransaction", x => x.ID);
                    table.ForeignKey(
                        name: "FK_WalletTransaction_Wallet_WalletID",
                        column: x => x.WalletID,
                        principalTable: "Wallet",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CustomerAddress",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CityID = table.Column<int>(type: "int", nullable: true),
                    CustomerID = table.Column<int>(type: "int", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Plaque = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BuildingUnit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PostalCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Default = table.Column<bool>(type: "bit", nullable: false),
                    IdentityCode = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RegisterDate = table.Column<int>(type: "int", nullable: false),
                    RegisterTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EditDate = table.Column<int>(type: "int", nullable: false),
                    EditTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    JsonLableTexts = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Visible = table.Column<bool>(type: "bit", nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerAddress", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CustomerAddress_City_CityID",
                        column: x => x.CityID,
                        principalTable: "City",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CustomerAddress_Customer_CustomerID",
                        column: x => x.CustomerID,
                        principalTable: "Customer",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RawProductStore_Product",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RawProductStoreID = table.Column<int>(type: "int", nullable: true),
                    ProductID = table.Column<int>(type: "int", nullable: true),
                    Count = table.Column<int>(type: "int", nullable: true),
                    IdentityCode = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RegisterDate = table.Column<int>(type: "int", nullable: false),
                    RegisterTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EditDate = table.Column<int>(type: "int", nullable: false),
                    EditTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    JsonLableTexts = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Visible = table.Column<bool>(type: "bit", nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RawProductStore_Product", x => x.ID);
                    table.ForeignKey(
                        name: "FK_RawProductStore_Product_Product_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Product",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_RawProductStore_Product_RawProductStore_RawProductStoreID",
                        column: x => x.RawProductStoreID,
                        principalTable: "RawProductStore",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "CartItem",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CartID = table.Column<int>(type: "int", nullable: false),
                    ProductID = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    IdentityCode = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RegisterDate = table.Column<int>(type: "int", nullable: false),
                    RegisterTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EditDate = table.Column<int>(type: "int", nullable: false),
                    EditTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    JsonLableTexts = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Visible = table.Column<bool>(type: "bit", nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartItem", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CartItem_Cart_CartID",
                        column: x => x.CartID,
                        principalTable: "Cart",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CartItem_Product_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Product",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrderItem",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderID = table.Column<int>(type: "int", nullable: true),
                    ProductID = table.Column<int>(type: "int", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    PriceAtOrder = table.Column<long>(type: "bigint", nullable: false),
                    IdentityCode = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RegisterDate = table.Column<int>(type: "int", nullable: false),
                    RegisterTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EditDate = table.Column<int>(type: "int", nullable: false),
                    EditTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    JsonLableTexts = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Visible = table.Column<bool>(type: "bit", nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItem", x => x.ID);
                    table.ForeignKey(
                        name: "FK_OrderItem_Order_OrderID",
                        column: x => x.OrderID,
                        principalTable: "Order",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderItem_Product_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Product",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BlogComment",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BlogID = table.Column<int>(type: "int", nullable: true),
                    CustomerID = table.Column<int>(type: "int", nullable: true),
                    ParentID = table.Column<int>(type: "int", nullable: true),
                    AuthorName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Body = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Rating = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    IsVerifiedBuyer = table.Column<bool>(type: "bit", nullable: false),
                    LikeCount = table.Column<int>(type: "int", nullable: false),
                    DislikeCount = table.Column<int>(type: "int", nullable: false),
                    IdentityCode = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RegisterDate = table.Column<int>(type: "int", nullable: false),
                    RegisterTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EditDate = table.Column<int>(type: "int", nullable: false),
                    EditTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    JsonLableTexts = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Visible = table.Column<bool>(type: "bit", nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogComment", x => x.ID);
                    table.ForeignKey(
                        name: "FK_BlogComment_Blog_BlogID",
                        column: x => x.BlogID,
                        principalTable: "Blog",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BlogComment_Customer_CustomerID",
                        column: x => x.CustomerID,
                        principalTable: "Customer",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RegisterCostRawProductStore",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RawProductStore_ProductID = table.Column<int>(type: "int", nullable: true),
                    PersonelID = table.Column<int>(type: "int", nullable: true),
                    Count = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<long>(type: "bigint", nullable: false),
                    IdentityCode = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RegisterDate = table.Column<int>(type: "int", nullable: false),
                    RegisterTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EditDate = table.Column<int>(type: "int", nullable: false),
                    EditTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    JsonLableTexts = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Visible = table.Column<bool>(type: "bit", nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegisterCostRawProductStore", x => x.ID);
                    table.ForeignKey(
                        name: "FK_RegisterCostRawProductStore_Personel_PersonelID",
                        column: x => x.PersonelID,
                        principalTable: "Personel",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RegisterCostRawProductStore_RawProductStore_Product_RawProductStore_ProductID",
                        column: x => x.RawProductStore_ProductID,
                        principalTable: "RawProductStore_Product",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_CustomerID",
                table: "AspNetUsers",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_CustomerID1",
                table: "AspNetUsers",
                column: "CustomerID1",
                unique: true,
                filter: "[CustomerID1] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Blog_GroupBlogID",
                table: "Blog",
                column: "GroupBlogID");

            migrationBuilder.CreateIndex(
                name: "IX_Blog_TeamID",
                table: "Blog",
                column: "TeamID");

            migrationBuilder.CreateIndex(
                name: "IX_BlogComment_BlogID",
                table: "BlogComment",
                column: "BlogID");

            migrationBuilder.CreateIndex(
                name: "IX_BlogComment_CustomerID",
                table: "BlogComment",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_Cart_CustomerID",
                table: "Cart",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_Cart_SendProductMethodID",
                table: "Cart",
                column: "SendProductMethodID");

            migrationBuilder.CreateIndex(
                name: "IX_CartItem_CartID",
                table: "CartItem",
                column: "CartID");

            migrationBuilder.CreateIndex(
                name: "IX_CartItem_ProductID",
                table: "CartItem",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_Category_ParentID",
                table: "Category",
                column: "ParentID");

            migrationBuilder.CreateIndex(
                name: "IX_City_ProvinceID",
                table: "City",
                column: "ProvinceID");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerAddress_CityID",
                table: "CustomerAddress",
                column: "CityID");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerAddress_CustomerID",
                table: "CustomerAddress",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_FavoritUserProduct_CustomerID",
                table: "FavoritUserProduct",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_FavoritUserProduct_ProductID",
                table: "FavoritUserProduct",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_Order_CustomerID",
                table: "Order",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_Order_SendProductMethodID",
                table: "Order",
                column: "SendProductMethodID");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItem_OrderID",
                table: "OrderItem",
                column: "OrderID");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItem_ProductID",
                table: "OrderItem",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_Personel_PositionID",
                table: "Personel",
                column: "PositionID");

            migrationBuilder.CreateIndex(
                name: "IX_PricingRule_ProductID",
                table: "PricingRule",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_Product_CategoryID",
                table: "Product",
                column: "CategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_Product_CountAction_CostType_PositionID",
                table: "Product_CountAction_CostType",
                column: "PositionID");

            migrationBuilder.CreateIndex(
                name: "IX_Product_CountAction_CostType_ProductID",
                table: "Product_CountAction_CostType",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductComment_CustomerID",
                table: "ProductComment",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductComment_ProductID",
                table: "ProductComment",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductFeature_CategoryID",
                table: "ProductFeature",
                column: "CategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductFeatureValue_ProductFeatureID",
                table: "ProductFeatureValue",
                column: "ProductFeatureID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductFeatureValue_ProductID",
                table: "ProductFeatureValue",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_Question_GroupQuestionID",
                table: "Question",
                column: "GroupQuestionID");

            migrationBuilder.CreateIndex(
                name: "IX_RawProductStore_RawProductID",
                table: "RawProductStore",
                column: "RawProductID");

            migrationBuilder.CreateIndex(
                name: "IX_RawProductStore_Product_ProductID",
                table: "RawProductStore_Product",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_RawProductStore_Product_RawProductStoreID",
                table: "RawProductStore_Product",
                column: "RawProductStoreID");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokenEntity_UserId",
                table: "RefreshTokenEntity",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_RegisterCostRawProductStore_PersonelID",
                table: "RegisterCostRawProductStore",
                column: "PersonelID");

            migrationBuilder.CreateIndex(
                name: "IX_RegisterCostRawProductStore_RawProductStore_ProductID",
                table: "RegisterCostRawProductStore",
                column: "RawProductStore_ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_SmsLog_CustomerID",
                table: "SmsLog",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_Ticket_DepartmentID",
                table: "Ticket",
                column: "DepartmentID");

            migrationBuilder.CreateIndex(
                name: "IX_Wallet_CustomerID",
                table: "Wallet",
                column: "CustomerID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WalletTransaction_WalletID",
                table: "WalletTransaction",
                column: "WalletID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "About");

            migrationBuilder.DropTable(
                name: "Advertisement");

            migrationBuilder.DropTable(
                name: "AdvertisementSingle");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "BlogComment");

            migrationBuilder.DropTable(
                name: "CartItem");

            migrationBuilder.DropTable(
                name: "ContactUs");

            migrationBuilder.DropTable(
                name: "CustomerAddress");

            migrationBuilder.DropTable(
                name: "Faq");

            migrationBuilder.DropTable(
                name: "FavoritUserProduct");

            migrationBuilder.DropTable(
                name: "OrderItem");

            migrationBuilder.DropTable(
                name: "OrderPaymentTemp");

            migrationBuilder.DropTable(
                name: "PricingRule");

            migrationBuilder.DropTable(
                name: "Product_CountAction_CostType");

            migrationBuilder.DropTable(
                name: "ProductComment");

            migrationBuilder.DropTable(
                name: "ProductFeatureValue");

            migrationBuilder.DropTable(
                name: "Question");

            migrationBuilder.DropTable(
                name: "RefreshTokenEntity");

            migrationBuilder.DropTable(
                name: "RegisterCostRawProductStore");

            migrationBuilder.DropTable(
                name: "Reminder");

            migrationBuilder.DropTable(
                name: "ReminderEvent");

            migrationBuilder.DropTable(
                name: "Setting");

            migrationBuilder.DropTable(
                name: "Slider");

            migrationBuilder.DropTable(
                name: "SmsLog");

            migrationBuilder.DropTable(
                name: "SmsOtpCode");

            migrationBuilder.DropTable(
                name: "Story");

            migrationBuilder.DropTable(
                name: "Ticket");

            migrationBuilder.DropTable(
                name: "WalletTransaction");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Blog");

            migrationBuilder.DropTable(
                name: "Cart");

            migrationBuilder.DropTable(
                name: "City");

            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropTable(
                name: "ProductFeature");

            migrationBuilder.DropTable(
                name: "GroupQuestion");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Personel");

            migrationBuilder.DropTable(
                name: "RawProductStore_Product");

            migrationBuilder.DropTable(
                name: "Department");

            migrationBuilder.DropTable(
                name: "Wallet");

            migrationBuilder.DropTable(
                name: "GroupBlog");

            migrationBuilder.DropTable(
                name: "Team");

            migrationBuilder.DropTable(
                name: "Province");

            migrationBuilder.DropTable(
                name: "SendProductMethod");

            migrationBuilder.DropTable(
                name: "Position");

            migrationBuilder.DropTable(
                name: "Product");

            migrationBuilder.DropTable(
                name: "RawProductStore");

            migrationBuilder.DropTable(
                name: "Customer");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropTable(
                name: "RawProduct");
        }
    }
}
