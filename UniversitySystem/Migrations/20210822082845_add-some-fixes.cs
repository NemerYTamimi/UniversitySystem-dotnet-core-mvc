using Microsoft.EntityFrameworkCore.Migrations;

namespace UniversitySystem.Migrations
{
    public partial class addsomefixes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TeacherName",
                table: "Teachers",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "TeacherContactNo",
                table: "Teachers",
                newName: "PhoneNumber");

            migrationBuilder.RenameColumn(
                name: "TeacherAddress",
                table: "Teachers",
                newName: "Address");

            migrationBuilder.RenameColumn(
                name: "ContactNo",
                table: "Students",
                newName: "PhoneNumber");

            migrationBuilder.RenameColumn(
                name: "SemesterName",
                table: "Semesters",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "GradeName",
                table: "Grades",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "DesignationName",
                table: "Designations",
                newName: "Name");

            migrationBuilder.AddColumn<string>(
                name: "LoginUserId",
                table: "Teachers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LoginUserId",
                table: "Students",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LoginUserId",
                table: "Teachers");

            migrationBuilder.DropColumn(
                name: "LoginUserId",
                table: "Students");

            migrationBuilder.RenameColumn(
                name: "PhoneNumber",
                table: "Teachers",
                newName: "TeacherContactNo");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Teachers",
                newName: "TeacherName");

            migrationBuilder.RenameColumn(
                name: "Address",
                table: "Teachers",
                newName: "TeacherAddress");

            migrationBuilder.RenameColumn(
                name: "PhoneNumber",
                table: "Students",
                newName: "ContactNo");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Semesters",
                newName: "SemesterName");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Grades",
                newName: "GradeName");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Designations",
                newName: "DesignationName");
        }
    }
}
