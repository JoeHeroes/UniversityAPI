using Microsoft.EntityFrameworkCore.Migrations;

namespace UniAPI.Migrations
{
    public partial class UniUserIdAdd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CreateById",
                table: "Universities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CreatedById",
                table: "Universities",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Universities_CreatedById",
                table: "Universities",
                column: "CreatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Universities_Users_CreatedById",
                table: "Universities",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Universities_Users_CreatedById",
                table: "Universities");

            migrationBuilder.DropIndex(
                name: "IX_Universities_CreatedById",
                table: "Universities");

            migrationBuilder.DropColumn(
                name: "CreateById",
                table: "Universities");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Universities");
        }
    }
}
