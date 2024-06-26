using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArelympApi.Migrations
{
    /// <inheritdoc />
    public partial class AssignedItemUserIdNullability2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AssignedItem_UserId_ItemId",
                table: "AssignedItem");

            migrationBuilder.CreateIndex(
                name: "IX_AssignedItem_UserId_ItemId",
                table: "AssignedItem",
                columns: new[] { "UserId", "ItemId" },
                unique: true,
                filter: "\"UserId\" IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AssignedItem_UserId_ItemId",
                table: "AssignedItem");

            migrationBuilder.CreateIndex(
                name: "IX_AssignedItem_UserId_ItemId",
                table: "AssignedItem",
                columns: new[] { "UserId", "ItemId" },
                unique: true);
        }
    }
}
