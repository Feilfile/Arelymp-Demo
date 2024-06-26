#!/bin/bash

# Function to execute all SQL files in a specified directory
execute_sql_scripts() {
  local script_dir="$1"  # Directory containing SQL scripts
  echo "Executing scripts in directory: $script_dir"

  # Debug: Print list of SQL files in the directory
  echo "SQL files in directory:"
  ls -l "$script_dir"/*.sql

  # Loop through all SQL files in the specified directory
  for sql_file in "$script_dir"/*.sql
  do
    echo "Executing $sql_file"
    # Execute SQL script using psql command
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

DB_HOST=$1
DB_PASS=$2
DB_NAME="ArelympDb"
DB_USER="sa"
DB_PORT="5432"

# Define directory paths as variables
LEVELING_DIR="./Leveling"
ITEM_DIR="./Items"
OTHERS_DIR="./Others"
USER_DIR="./Users"

echo "Executing the script..."

# Call the function with directory paths as arguments
execute_sql_scripts "$LEVELING_DIR"
execute_sql_scripts "$ITEM_DIR"
execute_sql_scripts "$OTHERS_DIR"