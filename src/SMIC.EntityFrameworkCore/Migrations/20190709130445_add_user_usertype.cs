using Microsoft.EntityFrameworkCore.Migrations;

namespace SMIC.Migrations
{
    public partial class add_user_usertype : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AbpUsers");

            migrationBuilder.AddColumn<short>(
                name: "UserType",
                table: "AbpUsers",
                nullable: false,
                defaultValue: (short)0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserType",
                table: "AbpUsers");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AbpUsers",
                nullable: false,
                defaultValue: "");
        }
    }
}
