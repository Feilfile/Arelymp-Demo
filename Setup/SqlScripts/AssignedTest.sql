INSERT INTO "Users" ("Id", "Email", "Name", "Elo", "Level", "Experience")
OVERRIDING SYSTEM VALUE
VALUES
    ('Google@a_2293302950509756942', 'PicoPeach@Gmail.com', 'PicoPeach', 1, 1, 1);

INSERT INTO "AssignedItem" ("UserId", "ItemId", "EquipSlot")
OVERRIDING SYSTEM VALUE
VALUES
    ('Google@a_2293302950509756942', 1, 0),
    ('Google@a_2293302950509756942', 10001, 1),
    ('Google@a_2293302950509756942', 10002, 1),
    ('Google@a_2293302950509756942', 20001, 2),
    ('Google@a_2293302950509756942', 20002, 2)

ON CONFLICT ("UserId", "ItemId") DO UPDATE
SET
    "UserId" = EXCLUDED."UserId",
    "ItemId" = EXCLUDED."ItemId",
    "EquipSlot" = EXCLUDED."EquipSlot";

INSERT INTO "Equip" ("UserId", "CharacterId", "CharacterSkinId", "WeaponSkinId");

OVERRIDING SYSTEM VALUE
VALUES
    ('Google@a_2293302950509756942', 1, 10001, 20001);

INSERT INTO "LeveledItem" ("AssignedItemId", "ItemLevel", "ItemExperience", "IsMaxed", "EquippedTier", "ItemSchemaId")
OVERRIDING SYSTEM VALUE
VALUES
    (7, 2, 0, 'FALSE', 1, 1),
    (8, 2, 0, 'FALSE', 1, 1),
    (9, 2, 0, 'FALSE', 1, 1),
    (10, 2, 0, 'FALSE', 1, 1);