using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArelympApi.Migrations
{
    /// <inheritdoc />
    public partial class AssignedItemUserIdNullability : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssignedItem_Users_UserId",
                table: "AssignedItem");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_AssignedItem_UserId_ItemId",
                table: "AssignedItem");

            migrationBuilder.DropIndex(
                name: "IX_AssignedItem_UserId",
                table: "AssignedItem");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "AssignedItem",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.CreateIndex(
                name: "IX_AssignedItem_UserId_ItemId",
                table: "AssignedItem",
                columns: new[] { "UserId", "ItemId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AssignedItem_Users_UserId",
                table: "AssignedItem",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssignedItem_Users_UserId",
                table: "AssignedItem");

            migrationBuilder.DropIndex(
                name: "IX_AssignedItem_UserId_ItemId",
                table: "AssignedItem");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "AssignedItem",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_AssignedItem_UserId_ItemId",
                table: "AssignedItem",
                columns: new[] { "UserId", "ItemId" });

            migrationBuilder.CreateIndex(
                name: "IX_AssignedItem_UserId",
                table: "AssignedItem",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AssignedItem_Users_UserId",
                table: "AssignedItem",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
