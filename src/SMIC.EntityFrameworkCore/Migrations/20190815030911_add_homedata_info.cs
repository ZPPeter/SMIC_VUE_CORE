using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SMIC.Migrations
{
    public partial class add_homedata_info : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "AbpUserExs");

            migrationBuilder.CreateTable(
                name: "HomeInfos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    User = table.Column<string>(maxLength: 16, nullable: true),
                    Title = table.Column<string>(maxLength: 32, nullable: true),
                    Description = table.Column<string>(maxLength: 256, nullable: true),
                    CreationTime = table.Column<DateTime>(maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HomeInfos", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HomeInfos");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "AbpUserExs",
                nullable: true);
        }
    }
}
