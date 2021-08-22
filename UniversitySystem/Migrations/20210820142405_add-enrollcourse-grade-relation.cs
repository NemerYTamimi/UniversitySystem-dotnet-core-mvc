using Microsoft.EntityFrameworkCore.Migrations;

namespace UniversitySystem.Migrations
{
    public partial class addenrollcoursegraderelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CourseGrade",
                table: "EnrollCourses");

            migrationBuilder.AddColumn<int>(
                name: "CourseGradeId",
                table: "EnrollCourses",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EnrollCourses_CourseGradeId",
                table: "EnrollCourses",
                column: "CourseGradeId");

            migrationBuilder.AddForeignKey(
                name: "FK_EnrollCourses_Grades_CourseGradeId",
                table: "EnrollCourses",
                column: "CourseGradeId",
                principalTable: "Grades",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EnrollCourses_Grades_CourseGradeId",
                table: "EnrollCourses");

            migrationBuilder.DropIndex(
                name: "IX_EnrollCourses_CourseGradeId",
                table: "EnrollCourses");

            migrationBuilder.DropColumn(
                name: "CourseGradeId",
                table: "EnrollCourses");

            migrationBuilder.AddColumn<string>(
                name: "CourseGrade",
                table: "EnrollCourses",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
