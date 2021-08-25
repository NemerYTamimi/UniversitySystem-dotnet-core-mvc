using Microsoft.EntityFrameworkCore.Migrations;

namespace UniversitySystem.Migrations
{
    public partial class addfinanialrecordforstudent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.AlterColumn<string>(
                name: "RoomStatus",
                table: "ClassRoomAllocations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Finanials",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Credit = table.Column<double>(type: "float", nullable: false),
                    CreditUsed = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Finanials", x => x.Id);
                });

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_Finanials_FinanialId",
                table: "Students");

            migrationBuilder.DropTable(
                name: "Finanials");

            migrationBuilder.DropIndex(
                name: "IX_Students_FinanialId",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "DinanialId",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "FinanialId",
                table: "Students");

            migrationBuilder.AlterColumn<string>(
                name: "RoomStatus",
                table: "ClassRoomAllocations",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
