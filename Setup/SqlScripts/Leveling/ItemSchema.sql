INSERT INTO "ItemSchema" ("Id")
OVERRIDING SYSTEM VALUE
VALUES

    (1)

ON CONFLICT ("Id") DO UPDATE
SET
    "Id" = EXCLUDED."Id";

INSERT INTO "LevelUpSchema" ("ItemSchemaId", "Level", "ExpRequired")
OVERRIDING SYSTEM VALUE
VALUES

    (1, 1, 5000),
    (1, 2, 15000),
    (1, 3, 50000)

ON CONFLICT ("ItemSchemaId", "Level") DO UPDATE
SET
    "ItemSchemaId" = EXCLUDED."ItemSchemaId",
    "Level" = EXCLUDED."Level",
    "ExpRequired" = EXCLUDED."ExpRequired";
