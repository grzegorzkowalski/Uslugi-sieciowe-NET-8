using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WeatherWorkerService.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Weather",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WeatherDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Temp = table.Column<double>(type: "float", nullable: false),
                    FeelsLike = table.Column<double>(type: "float", nullable: false),
                    TempMin = table.Column<double>(type: "float", nullable: false),
                    TempMax = table.Column<double>(type: "float", nullable: false),
                    Pressure = table.Column<int>(type: "int", nullable: false),
                    Humidity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Weather", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Weather");
        }
    }
}
