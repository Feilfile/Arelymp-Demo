using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ArelympApi.Migrations
{
    /// <inheritdoc />
    public partial class DatabaseInit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AssignedItem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    ItemId = table.Column<int>(type: "integer", nullable: false),
                    EquipSlot = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssignedItem", x => x.Id);
                    table.UniqueConstraint("AK_AssignedItem_UserId_ItemId", x => new { x.UserId, x.ItemId });
                });

            migrationBuilder.CreateTable(
                name: "LeveledItem",
                columns: table => new
                {
                    AssignedItemId = table.Column<int>(type: "integer", nullable: false),
                    ItemLevel = table.Column<int>(type: "integer", nullable: false),
                    ItemExperience = table.Column<int>(type: "integer", nullable: false),
                    IsMaxed = table.Column<bool>(type: "boolean", nullable: false),
                    ItemSchemaId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeveledItem", x => x.AssignedItemId);
                    table.ForeignKey(
                        name: "FK_LeveledItem_AssignedItem_AssignedItemId",
                        column: x => x.AssignedItemId,
                        principalTable: "AssignedItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ItemSchema",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LeveledItemAssignedItemId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemSchema", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemSchema_LeveledItem_LeveledItemAssignedItemId",
                        column: x => x.LeveledItemAssignedItemId,
                        principalTable: "LeveledItem",
                        principalColumn: "AssignedItemId");
                });

            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    EquipSlot = table.Column<int>(type: "integer", nullable: false),
                    BindedCharacterId = table.Column<int>(type: "integer", nullable: true),
                    Cost = table.Column<int>(type: "integer", nullable: false),
                    ItemSchemaId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Items_ItemSchema_ItemSchemaId",
                        column: x => x.ItemSchemaId,
                        principalTable: "ItemSchema",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LevelUpSchema",
                columns: table => new
                {
                    ItemSchemaId = table.Column<int>(type: "integer", nullable: false),
                    Level = table.Column<int>(type: "integer", nullable: false),
                    ExpRequired = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LevelUpSchema", x => new { x.ItemSchemaId, x.Level });
                    table.ForeignKey(
                        name: "FK_LevelUpSchema_ItemSchema_ItemSchemaId",
                        column: x => x.ItemSchemaId,
                        principalTable: "ItemSchema",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Equip",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    CharacterId = table.Column<int>(type: "integer", nullable: false),
                    CharacterSkinId = table.Column<int>(type: "integer", nullable: false),
                    WeaponSkinId = table.Column<int>(type: "integer", nullable: false),
                    WeaponEffectId = table.Column<int>(type: "integer", nullable: true),
                    AbilityOneSkinId = table.Column<int>(type: "integer", nullable: true),
                    AbilityTwoSkinId = table.Column<int>(type: "integer", nullable: true),
                    AbilityThreeSkinId = table.Column<int>(type: "integer", nullable: true),
                    AbilityFourSkinId = table.Column<int>(type: "integer", nullable: true),
                    ArmorEffectId = table.Column<int>(type: "integer", nullable: true),
                    VictoryPoseId = table.Column<int>(type: "integer", nullable: true),
                    TitleId = table.Column<int>(type: "integer", nullable: true),
                    BannerId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Equip", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_Equip_Items_AbilityFourSkinId",
                        column: x => x.AbilityFourSkinId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Equip_Items_AbilityOneSkinId",
                        column: x => x.AbilityOneSkinId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Equip_Items_AbilityThreeSkinId",
                        column: x => x.AbilityThreeSkinId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Equip_Items_AbilityTwoSkinId",
                        column: x => x.AbilityTwoSkinId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Equip_Items_ArmorEffectId",
                        column: x => x.ArmorEffectId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Equip_Items_BannerId",
                        column: x => x.BannerId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Equip_Items_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Equip_Items_CharacterSkinId",
                        column: x => x.CharacterSkinId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Equip_Items_TitleId",
                        column: x => x.TitleId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Equip_Items_VictoryPoseId",
                        column: x => x.VictoryPoseId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Equip_Items_WeaponEffectId",
                        column: x => x.WeaponEffectId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Equip_Items_WeaponSkinId",
                        column: x => x.WeaponSkinId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Elo = table.Column<int>(type: "integer", nullable: false),
                    Level = table.Column<int>(type: "integer", nullable: false),
                    Experience = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Equip_Id",
                        column: x => x.Id,
                        principalTable: "Equip",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CharacterLoadout",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    CharacterId = table.Column<int>(type: "integer", nullable: false),
                    CharacterSkinId = table.Column<int>(type: "integer", nullable: false),
                    WeaponSkinId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterLoadout", x => new { x.UserId, x.CharacterId });
                    table.ForeignKey(
                        name: "FK_CharacterLoadout_Items_CharacterSkinId",
                        column: x => x.CharacterSkinId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CharacterLoadout_Items_WeaponSkinId",
                        column: x => x.WeaponSkinId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CharacterLoadout_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AssignedItem_ItemId",
                table: "AssignedItem",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_AssignedItem_UserId",
                table: "AssignedItem",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CharacterLoadout_CharacterSkinId",
                table: "CharacterLoadout",
                column: "CharacterSkinId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CharacterLoadout_UserId",
                table: "CharacterLoadout",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CharacterLoadout_WeaponSkinId",
                table: "CharacterLoadout",
                column: "WeaponSkinId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Equip_AbilityFourSkinId",
                table: "Equip",
                column: "AbilityFourSkinId");

            migrationBuilder.CreateIndex(
                name: "IX_Equip_AbilityOneSkinId",
                table: "Equip",
                column: "AbilityOneSkinId");

            migrationBuilder.CreateIndex(
                name: "IX_Equip_AbilityThreeSkinId",
                table: "Equip",
                column: "AbilityThreeSkinId");

            migrationBuilder.CreateIndex(
                name: "IX_Equip_AbilityTwoSkinId",
                table: "Equip",
                column: "AbilityTwoSkinId");

            migrationBuilder.CreateIndex(
                name: "IX_Equip_ArmorEffectId",
                table: "Equip",
                column: "ArmorEffectId");

            migrationBuilder.CreateIndex(
                name: "IX_Equip_BannerId",
                table: "Equip",
                column: "BannerId");

            migrationBuilder.CreateIndex(
                name: "IX_Equip_CharacterId",
                table: "Equip",
                column: "CharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_Equip_CharacterSkinId",
                table: "Equip",
                column: "CharacterSkinId");

            migrationBuilder.CreateIndex(
                name: "IX_Equip_TitleId",
                table: "Equip",
                column: "TitleId");

            migrationBuilder.CreateIndex(
                name: "IX_Equip_VictoryPoseId",
                table: "Equip",
                column: "VictoryPoseId");

            migrationBuilder.CreateIndex(
                name: "IX_Equip_WeaponEffectId",
                table: "Equip",
                column: "WeaponEffectId");

            migrationBuilder.CreateIndex(
                name: "IX_Equip_WeaponSkinId",
                table: "Equip",
                column: "WeaponSkinId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_ItemSchemaId",
                table: "Items",
                column: "ItemSchemaId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemSchema_LeveledItemAssignedItemId",
                table: "ItemSchema",
                column: "LeveledItemAssignedItemId");

            migrationBuilder.CreateIndex(
                name: "IX_LevelUpSchema_ItemSchemaId",
                table: "LevelUpSchema",
                column: "ItemSchemaId");

            migrationBuilder.AddForeignKey(
                name: "FK_AssignedItem_Items_ItemId",
                table: "AssignedItem",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AssignedItem_Users_UserId",
                table: "AssignedItem",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssignedItem_Items_ItemId",
                table: "AssignedItem");

            migrationBuilder.DropForeignKey(
                name: "FK_Equip_Items_AbilityFourSkinId",
                table: "Equip");

            migrationBuilder.DropForeignKey(
                name: "FK_Equip_Items_AbilityOneSkinId",
                table: "Equip");

            migrationBuilder.DropForeignKey(
                name: "FK_Equip_Items_AbilityThreeSkinId",
                table: "Equip");

            migrationBuilder.DropForeignKey(
                name: "FK_Equip_Items_AbilityTwoSkinId",
                table: "Equip");

            migrationBuilder.DropForeignKey(
                name: "FK_Equip_Items_ArmorEffectId",
                table: "Equip");

            migrationBuilder.DropForeignKey(
                name: "FK_Equip_Items_BannerId",
                table: "Equip");

            migrationBuilder.DropForeignKey(
                name: "FK_Equip_Items_CharacterId",
                table: "Equip");

            migrationBuilder.DropForeignKey(
                name: "FK_Equip_Items_CharacterSkinId",
                table: "Equip");

            migrationBuilder.DropForeignKey(
                name: "FK_Equip_Items_TitleId",
                table: "Equip");

            migrationBuilder.DropForeignKey(
                name: "FK_Equip_Items_VictoryPoseId",
                table: "Equip");

            migrationBuilder.DropForeignKey(
                name: "FK_Equip_Items_WeaponEffectId",
                table: "Equip");

            migrationBuilder.DropForeignKey(
                name: "FK_Equip_Items_WeaponSkinId",
                table: "Equip");

            migrationBuilder.DropTable(
                name: "CharacterLoadout");

            migrationBuilder.DropTable(
                name: "LevelUpSchema");

            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "ItemSchema");

            migrationBuilder.DropTable(
                name: "LeveledItem");

            migrationBuilder.DropTable(
                name: "AssignedItem");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Equip");
        }
    }
}
