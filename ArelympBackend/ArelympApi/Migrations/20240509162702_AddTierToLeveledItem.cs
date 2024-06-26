using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArelympApi.Migrations
{
    /// <inheritdoc />
    public partial class AddTierToLeveledItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EquippedTier",
                table: "LeveledItem",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EquippedTier",
                table: "LeveledItem");
        }
    }
}
