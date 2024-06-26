using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArelympApi.Migrations
{
    /// <inheritdoc />
    public partial class CharacterLoadoutUserForeignKeyUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CharacterLoadout_UserId",
                table: "CharacterLoadout");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_CharacterLoadout_UserId",
                table: "CharacterLoadout",
                column: "UserId",
                unique: true);
        }
    }
}
