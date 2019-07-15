using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SMIC.Migrations
{
    public partial class Remove_Name : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "AbpUsers");

            migrationBuilder.CreateTable(
                name: "AbpUserExs",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TenantId = table.Column<int>(nullable: true),
                    UserName = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Surname = table.Column<string>(nullable: true),
                    EmailAddress = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    LastLoginTime = table.Column<DateTime>(nullable: true),
                    CreationTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpUserExs", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AbpUserExs");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "AbpUsers",
                maxLength: 64,
                nullable: false,
                defaultValue: "");
        }
    }
}
