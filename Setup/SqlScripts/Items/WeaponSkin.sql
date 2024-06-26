INSERT INTO "Items" ("Id", "Name", "EquipSlot", "BindedCharacterId", "Cost", "ItemSchemaId")
OVERRIDING SYSTEM VALUE
VALUES

    (20001, 'Default', 10, 1, 99999, 1),
    (20002, 'Default', 10, 2, 99999, 1)

ON CONFLICT ("Id") DO UPDATE
SET
    "Name" = EXCLUDED."Name",
    "EquipSlot" = EXCLUDED."EquipSlot",
    "BindedCharacterId" = EXCLUDED."BindedCharacterId",
    "Cost" = EXCLUDED."Cost",
    "ItemSchemaId" = EXCLUDED."ItemSchemaId";
