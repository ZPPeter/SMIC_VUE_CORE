using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SMIC.Migrations.SDIMDb
{
    public partial class add_VW_SJCL_100 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "JBCSS",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BCJDA = table.Column<double>(nullable: false),
                    BCJDB = table.Column<double>(nullable: false),
                    CJJD = table.Column<double>(nullable: false),
                    BCFW = table.Column<double>(nullable: false),
                    Axles = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JBCSS", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "JDRQS",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    jdrq = table.Column<DateTime>(nullable: false),
                    jwrq = table.Column<DateTime>(nullable: false),
                    jdzt = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JDRQS", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RecentSJMX",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    委托单号 = table.Column<string>(nullable: true),
                    送检单位 = table.Column<string>(nullable: true),
                    送检日期 = table.Column<string>(nullable: true),
                    仪器件数 = table.Column<string>(nullable: true),
                    检定状态 = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecentSJMX", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SJMX",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    sjdid = table.Column<string>(nullable: true),
                    qjmc = table.Column<string>(nullable: true),
                    djrq = table.Column<DateTime>(nullable: false),
                    xhggmc = table.Column<string>(nullable: true),
                    xhggbm = table.Column<string>(nullable: true),
                    ccbh = table.Column<string>(nullable: true),
                    jdzt1 = table.Column<string>(nullable: true),
                    jdzt2 = table.Column<string>(nullable: true),
                    zzc = table.Column<string>(nullable: true),
                    yqjcrq = table.Column<DateTime>(nullable: false),
                    wtdw = table.Column<string>(nullable: true),
                    jdrq = table.Column<DateTime>(nullable: false),
                    jwrq = table.Column<DateTime>(nullable: false),
                    jdy = table.Column<string>(nullable: true),
                    bzsm = table.Column<string>(nullable: true),
                    wtdh = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SJMX", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "STATS",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    y = table.Column<int>(nullable: false),
                    m = table.Column<int>(nullable: false),
                    count = table.Column<int>(nullable: false),
                    bm = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_STATS", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VW_SJMX",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    送检单ID = table.Column<double>(nullable: false),
                    送检单号 = table.Column<string>(nullable: true),
                    单位名称 = table.Column<string>(nullable: true),
                    送检日期 = table.Column<DateTime>(nullable: false),
                    器具名称 = table.Column<string>(nullable: true),
                    型号规格 = table.Column<string>(nullable: true),
                    出厂编号 = table.Column<string>(nullable: true),
                    证书编号 = table.Column<string>(nullable: true),
                    检定状态 = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VW_SJMX", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WTD",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    sjdid = table.Column<string>(nullable: true),
                    dwmc = table.Column<string>(nullable: true),
                    sjrq = table.Column<DateTime>(nullable: false),
                    yqjs = table.Column<int>(nullable: false),
                    //q zyjs = table.Column<int>(nullable: false),
                    jdzt = table.Column<string>(nullable: true),
                    //q zyjdzt = table.Column<string>(nullable: true),
                    yqjcrq = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WTD", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JBCSS");

            migrationBuilder.DropTable(
                name: "JDRQS");

            migrationBuilder.DropTable(
                name: "RecentSJMX");

            migrationBuilder.DropTable(
                name: "SJMX");

            migrationBuilder.DropTable(
                name: "STATS");

            migrationBuilder.DropTable(
                name: "VW_SJMX");

            migrationBuilder.DropTable(
                name: "WTD");
        }
    }
}
