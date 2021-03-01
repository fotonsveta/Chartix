using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Chartix.Infrastructure.Migrations
{
    public partial class UpdateSourceDateAndMetricCreated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastActionDate",
                table: "Source",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsCreated",
                table: "Metric",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastActionDate",
                table: "Source");

            migrationBuilder.DropColumn(
                name: "IsCreated",
                table: "Metric");
        }
    }
}
