using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRMS.Migrations
{
    /// <inheritdoc />
    public partial class departments_lookups_relation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "TypeId",
                table: "Departments",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Departments_TypeId",
                table: "Departments",
                column: "TypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Departments_Lookups_TypeId",
                table: "Departments",
                column: "TypeId",
                principalTable: "Lookups",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Departments_Lookups_TypeId",
                table: "Departments");

            migrationBuilder.DropIndex(
                name: "IX_Departments_TypeId",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "TypeId",
                table: "Departments");
        }
    }
}
