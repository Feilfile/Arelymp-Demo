#!/bin/bash

DIRECTORY="/root/Setup"

# Step 1: Remove potentially conflicting packages
for pkg in docker.io docker-doc docker-compose docker-compose-v2 podman-docker containerd runc; do 
    sudo apt-get -y remove "$pkg"
done

# Step 2: Add Docker's official GPG key
sudo apt-get update
sudo apt-get install -y ca-certificates curl gnupg lsb-release
sudo mkdir -p /etc/apt/keyrings
sudo curl -fsSL https://download.docker.com/linux/ubuntu/gpg -o /etc/apt/keyrings/docker.gpg
sudo chmod a+r /etc/apt/keyrings/docker.gpg

# Step 3: Add the Docker repository to Apt sources
echo \
  "deb [arch=$(dpkg --print-architecture) signed-by=/etc/apt/keyrings/docker.gpg] https://download.docker.com/linux/ubuntu \
  $(lsb_release -cs) stable" | \
  sudo tee /etc/apt/sources.list.d/docker.list > /dev/null
sudo apt-get update

# Step 4: Install Docker Engine, Docker CLI, Containerd, and Docker Compose plugin
sudo apt-get install -y docker-ce docker-ce-cli containerd.io docker-compose-plugin

# Ensure Docker Compose is executable (Docker version 19.03.0+ uses the Docker Compose plugin)
sudo ln -sf /usr/bin/docker-compose /usr/local/bin/docker-compose

# Step 5: Install Certbot for SSL certificate generation
sudo snap install core; sudo snap refresh core
sudo snap install --classic certbot
sudo ln -sf /snap/bin/certbot /usr/bin/certbot

# Optional: Generate SSL certificate using Certbot (uncomment after configuring DNS)
# Make sure to replace "your_domain.com" and "www.your_domain.com" with your actual domain names
sudo certbot certonly --standalone -d arelymp.com #-d www.your_domain.com

# Open necessary ports for NGINX and MSSQL (Development environment)
sudo ufw allow 80
sudo ufw allow 443
sudo ufw allow 1433

# Step 6: Start Docker services using Compose
sudo docker-compose -f $DIRECTORY/docker-compose.yml up -d

# Step 7: Install .NET runtime
wget https://packages.microsoft.com/config/ubuntu/22.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
sudo dpkg -i packages-microsoft-prod.deb
rm packages-microsoft-prod.deb

sudo apt-get update
sudo apt-get install -y apt-transport-https
sudo apt-get update
sudo apt-get install -y dotnet-runtime-8.0

# Step 8: Set up deployment directory and copy deployment scripts
mkdir /root/deployment
cp ./api-deployment.sh /root/deployment
sudo cp ./kestrel-arelymp-backend.service /etc/systemd/system

# Step 9: Reload systemd and enable the service
sudo systemctl daemon-reload
sudo systemctl enable kestrel-arelymp-backend.service
