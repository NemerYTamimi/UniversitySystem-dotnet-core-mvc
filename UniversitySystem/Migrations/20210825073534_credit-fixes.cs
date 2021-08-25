using Microsoft.EntityFrameworkCore.Migrations;

namespace UniversitySystem.Migrations
{
    public partial class creditfixes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreditLeft",
                table: "Teachers");

            migrationBuilder.DropColumn(
                name: "CreditTaken",
                table: "Teachers");

            migrationBuilder.DropColumn(
                name: "CreditLeft",
                table: "CourseAssigns");

            migrationBuilder.DropColumn(
                name: "CreditTaken",
                table: "CourseAssigns");

            migrationBuilder.AddColumn<double>(
                name: "Capacity",
                table: "Courses",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Capacity",
                table: "Courses");

            migrationBuilder.AddColumn<double>(
                name: "CreditLeft",
                table: "Teachers",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "CreditTaken",
                table: "Teachers",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "CreditLeft",
                table: "CourseAssigns",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "CreditTaken",
                table: "CourseAssigns",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
