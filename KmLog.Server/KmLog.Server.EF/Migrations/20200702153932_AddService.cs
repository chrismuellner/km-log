using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KmLog.Server.EF.Migrations
{
    public partial class AddService : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ServiceEntry",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    Cost = table.Column<double>(nullable: false),
                    Distance = table.Column<long>(nullable: false),
                    TotalDistance = table.Column<long>(nullable: false),
                    Notes = table.Column<string>(nullable: true),
                    CarId = table.Column<Guid>(nullable: false),
                    ServiceType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceEntry", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServiceEntry_Car_CarId",
                        column: x => x.CarId,
                        principalTable: "Car",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ServiceEntry_CarId",
                table: "ServiceEntry",
                column: "CarId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ServiceEntry");
        }
    }
}
