using Microsoft.EntityFrameworkCore.Migrations;

namespace KmLog.Server.EF.Migrations
{
    public partial class MoveDistance : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Distance",
                table: "ServiceEntry");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "Distance",
                table: "ServiceEntry",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
