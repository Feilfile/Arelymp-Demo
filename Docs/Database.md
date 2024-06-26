##### <a href="../Documentation.md">< Main Page</a>
# Database

This project utilizes a PostgreSQL database, implemented through a code-first approach using the Entity Framework Core ORM system. This method ensures that the database structure mirrors the code structure. Each database table is defined within an entity, and the table configuration, including settings for indexes and keys, is specified within a type configuration file.

More information can be found in the Entity Framework Core documentation: https://learn.microsoft.com/de-de/ef/core/

![image](./Database.png)

### How to migrate the database

1. In order to migrate the database the dotnet tools need to be installed on the machine
```dotnet tool install --global dotnet-ef```

The following commands need to be executed in the project where the Migrations folder is located:

2. Only if you want to change commits 
```dotnet ef migrations add <commitName>```

3. Use the dotnet ef database update command
```dotnet ef database update```

### Yaml Pipeline for automation

This Pipeline is designed for Azure Devops to automatically execute the database migrations when changes are pushed into Migrations order of the selected branches.

```Yaml
trigger:
  branches:
    include:
      - dev
  paths:
    include:
      - 'ArelympBackend/ArelympApi/Migrations/**'

pool:
  vmImage: 'ubuntu-latest'

steps:
- task: UseDotNet@2
  inputs:
    packageType: 'sdk'
    version: '8.x'
    installationPath: $(Agent.ToolsDirectory)/dotnet

- script: |
    dotnet tool install --global dotnet-ef
    cd ArelympBackend/ArelympApi
    dotnet ef database update --connection "$(DEV-CONNECTIONSTRING)"
  displayName: 'Execute EF Core Migrations'
  ```

### Database Scripts

The following scripts show an example SQL script and a pipeline to manage static items, so basically database entities that needs to be consistent and will be referenced inside the game.

```SQL
PostgreSQL Script:

INSERT INTO "Items" ("Id", "Name", "EquipSlot", "BindedCharacterId", "Cost")
OVERRIDING SYSTEM VALUE
VALUES
    (1, 'Example 1', 0, NULL, 99999),
    (2, 'Example 2', 0, NULL, 99999)
ON CONFLICT ("Id") DO UPDATE
SET
    "Name" = EXCLUDED."Name",
    "EquipSlot" = EXCLUDED."EquipSlot",
    "BindedCharacterId" = EXCLUDED."BindedCharacterId",
    "Cost" = EXCLUDED."Cost";
```

By using the following yaml pipeline the data synchronization can be automated whenever changes are made in the defined branches 

Pipeline.yml:
```YML

trigger:
  branches:
    include:
      - dev
  paths:
    include:
      - 'Setup/SqlScripts/**'

pool:
  vmImage: 'ubuntu-latest'

steps:
- script: |
    cd ./Setup/SqlScripts
    chmod +x ./DatabaseUpdate.sh
    ./DatabaseUpdate.sh $(DB_HOST) $(DB_PASS)
  displayName: 'Run Shell Script'

```

Note that DB_HOST and DB_PASS are secrets arguments saved inside azure devops.

DatabaseUpdate.sh:
```sh

#!/bin/bash

# Function to execute all SQL files in a specified directory
execute_sql_scripts() {
  local script_dir="$1" 
  echo "Executing scripts in directory: $script_dir"

  # Debug: Print list of SQL files in the directory
  echo "SQL files in directory:"
  ls -l "$script_dir"/*.sql

  # Loop through all SQL files in the specified directory
  for sql_file in "$script_dir"/*.sql
  do
    echo "Executing $sql_file"
    PGPASSWORD=$DB_PASS psql -h $DB_HOST -p $DB_PORT -U $DB_USER -d $DB_NAME -f "$sql_file"
    if [ $? -eq 0 ]
    then
      echo "Successfully executed $sql_file"
    else
      echo "Error occurred during execution of $sql_file"
      exit 1
    fi
  done
}

# DB_Host and DB_PASS are the passed arguments
DB_HOST=$1
DB_PASS=$2
DB_NAME="ArelympDb"
DB_USER="sa"
DB_PORT="5432"

# Define directory paths as variables
LEVELING_DIR="./Leveling"
ITEM_DIR="./Items"
OTHERS_DIR="./Others"

echo "Executing the script..."

# Call the function with directory paths as arguments
execute_sql_scripts "$LEVELING_DIR"
execute_sql_scripts "$ITEM_DIR"
execute_sql_scripts "$OTHERS_DIR"

```