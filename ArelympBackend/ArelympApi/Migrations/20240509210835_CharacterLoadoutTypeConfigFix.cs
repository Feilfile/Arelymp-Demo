using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArelympApi.Migrations
{
    /// <inheritdoc />
    public partial class CharacterLoadoutTypeConfigFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CharacterLoadout_CharacterSkinId",
                table: "CharacterLoadout");

            migrationBuilder.DropIndex(
                name: "IX_CharacterLoadout_WeaponSkinId",
                table: "CharacterLoadout");

            migrationBuilder.CreateIndex(
                name: "IX_CharacterLoadout_CharacterSkinId",
                table: "CharacterLoadout",
                column: "CharacterSkinId");

            migrationBuilder.CreateIndex(
                name: "IX_CharacterLoadout_WeaponSkinId",
                table: "CharacterLoadout",
                column: "WeaponSkinId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CharacterLoadout_CharacterSkinId",
                table: "CharacterLoadout");

            migrationBuilder.DropIndex(
                name: "IX_CharacterLoadout_WeaponSkinId",
                table: "CharacterLoadout");

            migrationBuilder.CreateIndex(
                name: "IX_CharacterLoadout_CharacterSkinId",
                table: "CharacterLoadout",
                column: "CharacterSkinId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CharacterLoadout_WeaponSkinId",
                table: "CharacterLoadout",
                column: "WeaponSkinId",
                unique: true);
        }
    }
}
