INSERT INTO "CharacterLoadout" ("Id", "UserId", "CharacterId", "CharacterSkinId", "WeaponSkinId")
OVERRIDING SYSTEM VALUE
VALUES
    (-1, null, 1, 10001, 20001),
    (-2, null, 2, 10002, 20002)

ON CONFLICT ("Id") DO UPDATE
SET
    "Id" = EXCLUDED."Id",
    "UserId" = EXCLUDED."UserId",
    "CharacterId" = EXCLUDED."CharacterId",
    "CharacterSkinId" = EXCLUDED."CharacterSkinId",
    "WeaponSkinId" = EXCLUDED."WeaponSkinId";