using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ArelympApi.Migrations
{
    /// <inheritdoc />
    public partial class AssignedItemUserIdNullability3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssignedItem_Users_UserId",
                table: "AssignedItem");

            migrationBuilder.DropForeignKey(
                name: "FK_CharacterLoadout_Users_UserId",
                table: "CharacterLoadout");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CharacterLoadout",
                table: "CharacterLoadout");

            migrationBuilder.DropIndex(
                name: "IX_AssignedItem_UserId_ItemId",
                table: "AssignedItem");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "AssignedItem",
                newName: "AssignedItemId");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "CharacterLoadout",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "CharacterLoadout",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "AssignedItem",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CharacterLoadout",
                table: "CharacterLoadout",
                column: "Id");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_AssignedItem_UserId_ItemId",
                table: "AssignedItem",
                columns: new[] { "UserId", "ItemId" });

            migrationBuilder.CreateIndex(
                name: "IX_CharacterLoadout_UserId_CharacterId",
                table: "CharacterLoadout",
                columns: new[] { "UserId", "CharacterId" },
                unique: true);

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

            migrationBuilder.AddForeignKey(
                name: "FK_CharacterLoadout_Users_UserId",
                table: "CharacterLoadout",
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

            migrationBuilder.DropForeignKey(
                name: "FK_CharacterLoadout_Users_UserId",
                table: "CharacterLoadout");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CharacterLoadout",
                table: "CharacterLoadout");

            migrationBuilder.DropIndex(
                name: "IX_CharacterLoadout_UserId_CharacterId",
                table: "CharacterLoadout");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_AssignedItem_UserId_ItemId",
                table: "AssignedItem");

            migrationBuilder.DropIndex(
                name: "IX_AssignedItem_UserId",
                table: "AssignedItem");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "CharacterLoadout");

            migrationBuilder.RenameColumn(
                name: "AssignedItemId",
                table: "AssignedItem",
                newName: "Id");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "CharacterLoadout",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "AssignedItem",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CharacterLoadout",
                table: "CharacterLoadout",
                columns: new[] { "UserId", "CharacterId" });

            migrationBuilder.CreateIndex(
                name: "IX_AssignedItem_UserId_ItemId",
                table: "AssignedItem",
                columns: new[] { "UserId", "ItemId" },
                unique: true,
                filter: "\"UserId\" IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_AssignedItem_Users_UserId",
                table: "AssignedItem",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CharacterLoadout_Users_UserId",
                table: "CharacterLoadout",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
