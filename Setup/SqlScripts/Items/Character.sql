INSERT INTO "Items" ("Id", "Name", "EquipSlot", "BindedCharacterId", "Cost")
OVERRIDING SYSTEM VALUE
VALUES
    (-1, 'SystemOnly', -1, NULL, 0),
    (1, 'Phantor', 0, NULL, 99999),
    (2, 'Sigma', 0, NULL, 99999)
ON CONFLICT ("Id") DO UPDATE
SET
    "Name" = EXCLUDED."Name",
    "EquipSlot" = EXCLUDED."EquipSlot",
    "BindedCharacterId" = EXCLUDED."BindedCharacterId",
    "Cost" = EXCLUDED."Cost";
