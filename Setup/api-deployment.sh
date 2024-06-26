#!/bin/bash

# Stop the current application
systemctl stop kestrel-arelymp-backend

# Replace 'arelymp-backend' and paths as necessary
unzip -o /root/ArelympApi.zip -d /var/www/ArelympApi
chmod -R 755 /var/www/ArelympApi

# Restart the application
systemctl start kestrel-arelymp-backend