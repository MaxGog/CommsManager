using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CommsManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddConfigurations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ArtistProfiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: false),
                    ArtistPicture = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    ArtistBanner = table.Column<byte[]>(type: "varbinary(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtistProfiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Communication = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CustomerPicture = table.Column<byte[]>(type: "varbinary(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ArtistProfiles_Emails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmailAdress = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    TypeEmail = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ArtistProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtistProfiles_Emails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArtistProfiles_Emails_ArtistProfiles",
                        column: x => x.ArtistProfileId,
                        principalTable: "ArtistProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ArtistProfiles_Phones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypePhone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    NumberPhone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RegionNumber = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ArtistProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtistProfiles_Phones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArtistProfiles_Phones_ArtistProfiles",
                        column: x => x.ArtistProfileId,
                        principalTable: "ArtistProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ArtistProfiles_SocialLinks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Link = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    TypeLink = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsVisible = table.Column<bool>(type: "bit", nullable: false),
                    ArtistProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtistProfiles_SocialLinks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArtistProfiles_SocialLinks_ArtistProfiles",
                        column: x => x.ArtistProfileId,
                        principalTable: "ArtistProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Commissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ViewAttachment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TypeCommission = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Price = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ArtistProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Commissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Commissions_ArtistProfiles",
                        column: x => x.ArtistProfileId,
                        principalTable: "ArtistProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Customers_Emails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmailAdress = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    TypeEmail = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers_Emails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Customers_Emails_Customers",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Customers_Phones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypePhone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    NumberPhone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RegionNumber = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers_Phones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Customers_Phones_Customers",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Customers_SocialLinks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Link = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    TypeLink = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsVisible = table.Column<bool>(type: "bit", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers_SocialLinks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Customers_SocialLinks_Customers",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Price = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: false),
                    Deadline = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ArtistId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_Customers",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrderAttachments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Attachment = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    TypeAttachment = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    OrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderAttachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderAttachments_Orders",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArtistProfiles_CreatedDate",
                table: "ArtistProfiles",
                column: "CreatedDate");

            migrationBuilder.CreateIndex(
                name: "IX_ArtistProfiles_Name",
                table: "ArtistProfiles",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_ArtistProfiles_Emails_ArtistProfileId",
                table: "ArtistProfiles_Emails",
                column: "ArtistProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_ArtistProfiles_Emails_EmailAdress",
                table: "ArtistProfiles_Emails",
                column: "EmailAdress");

            migrationBuilder.CreateIndex(
                name: "IX_ArtistProfiles_Phones_ArtistProfileId",
                table: "ArtistProfiles_Phones",
                column: "ArtistProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_ArtistProfiles_Phones_NumberPhone",
                table: "ArtistProfiles_Phones",
                column: "NumberPhone");

            migrationBuilder.CreateIndex(
                name: "IX_ArtistProfiles_SocialLinks_ArtistProfileId",
                table: "ArtistProfiles_SocialLinks",
                column: "ArtistProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_ArtistProfiles_SocialLinks_IsActive",
                table: "ArtistProfiles_SocialLinks",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_ArtistProfiles_SocialLinks_IsVisible",
                table: "ArtistProfiles_SocialLinks",
                column: "IsVisible");

            migrationBuilder.CreateIndex(
                name: "IX_ArtistProfiles_SocialLinks_TypeLink",
                table: "ArtistProfiles_SocialLinks",
                column: "TypeLink");

            migrationBuilder.CreateIndex(
                name: "IX_Commissions_ArtistProfileId",
                table: "Commissions",
                column: "ArtistProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_Commissions_Name",
                table: "Commissions",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Commissions_TypeCommission",
                table: "Commissions",
                column: "TypeCommission");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_CreatedDate",
                table: "Customers",
                column: "CreatedDate");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_IsActive",
                table: "Customers",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_IsActive_Name",
                table: "Customers",
                columns: new[] { "IsActive", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_Customers_Name",
                table: "Customers",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_Emails_CustomerId",
                table: "Customers_Emails",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_Emails_EmailAdress",
                table: "Customers_Emails",
                column: "EmailAdress");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_Phones_CustomerId",
                table: "Customers_Phones",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_Phones_NumberPhone",
                table: "Customers_Phones",
                column: "NumberPhone");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_SocialLinks_CustomerId",
                table: "Customers_SocialLinks",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_SocialLinks_IsActive",
                table: "Customers_SocialLinks",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_SocialLinks_IsVisible",
                table: "Customers_SocialLinks",
                column: "IsVisible");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_SocialLinks_TypeLink",
                table: "Customers_SocialLinks",
                column: "TypeLink");

            migrationBuilder.CreateIndex(
                name: "IX_OrderAttachments_OrderId",
                table: "OrderAttachments",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ArtistId",
                table: "Orders",
                column: "ArtistId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CustomerId",
                table: "Orders",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_Deadline",
                table: "Orders",
                column: "Deadline");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_IsActive",
                table: "Orders",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_Status",
                table: "Orders",
                column: "Status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArtistProfiles_Emails");

            migrationBuilder.DropTable(
                name: "ArtistProfiles_Phones");

            migrationBuilder.DropTable(
                name: "ArtistProfiles_SocialLinks");

            migrationBuilder.DropTable(
                name: "Commissions");

            migrationBuilder.DropTable(
                name: "Customers_Emails");

            migrationBuilder.DropTable(
                name: "Customers_Phones");

            migrationBuilder.DropTable(
                name: "Customers_SocialLinks");

            migrationBuilder.DropTable(
                name: "OrderAttachments");

            migrationBuilder.DropTable(
                name: "ArtistProfiles");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Customers");
        }
    }
}
