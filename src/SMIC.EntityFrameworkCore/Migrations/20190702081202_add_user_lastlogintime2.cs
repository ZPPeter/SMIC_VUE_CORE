using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SMIC.Migrations
{
    public partial class add_user_lastlogintime2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastLoginTime2",
                table: "AbpUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastLoginTime2",
                table: "AbpUsers");
        }
    }
}
