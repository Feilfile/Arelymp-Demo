using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArelympApi.Migrations
{
    /// <inheritdoc />
    public partial class AddItemBindedCharacterAndItemSchemaForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Items_BindedCharacterId",
                table: "Items",
                column: "BindedCharacterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Items_BindedCharacterId",
                table: "Items",
                column: "BindedCharacterId",
                principalTable: "Items",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_Items_BindedCharacterId",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Items_BindedCharacterId",
                table: "Items");
        }
    }
}
