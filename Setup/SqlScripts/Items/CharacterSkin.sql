INSERT INTO "Items" ("Id", "Name", "EquipSlot", "BindedCharacterId", "Cost", "ItemSchemaId")
OVERRIDING SYSTEM VALUE
VALUES

    (10001, 'Default', 1, 1, 99999, 1),
    (10002, 'Default', 1, 2, 99999, 1)

ON CONFLICT ("Id") DO UPDATE
SET
    "Name" = EXCLUDED."Name",
    "EquipSlot" = EXCLUDED."EquipSlot",
    "BindedCharacterId" = EXCLUDED."BindedCharacterId",
    "Cost" = EXCLUDED."Cost",
    "ItemSchemaId" = EXCLUDED."ItemSchemaId";
