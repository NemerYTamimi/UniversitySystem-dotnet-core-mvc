using Microsoft.EntityFrameworkCore.Migrations;

namespace UniversitySystem.Migrations
{
    public partial class editfinanialFK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_Finanials_FinanialId",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Students_FinanialId",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "DinanialId",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "FinanialId",
                table: "Students");

            migrationBuilder.AddColumn<int>(
                name: "StudentId",
                table: "Finanials",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Finanials_StudentId",
                table: "Finanials",
                column: "StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Finanials_Students_StudentId",
                table: "Finanials",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Finanials_Students_StudentId",
                table: "Finanials");

            migrationBuilder.DropIndex(
                name: "IX_Finanials_StudentId",
                table: "Finanials");

            migrationBuilder.DropColumn(
                name: "StudentId",
                table: "Finanials");

            migrationBuilder.AddColumn<int>(
                name: "DinanialId",
                table: "Students",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FinanialId",
                table: "Students",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Students_FinanialId",
                table: "Students",
                column: "FinanialId");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Finanials_FinanialId",
                table: "Students",
                column: "FinanialId",
                principalTable: "Finanials",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
