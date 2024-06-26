#!/bin/bash

echo "Starting setup..."

DIRECTORY="/root/Setup"

echo "Step 1: Removing potentially conflicting packages..."
for pkg in docker.io docker-doc docker-compose docker-compose-v2 podman-docker containerd runc; do 
    echo "Removing $pkg..."
    sudo apt-get -y remove "$pkg"
done

echo "Step 2: Adding Docker's official GPG key..."
sudo apt-get update
sudo apt-get install -y ca-certificates curl gnupg lsb-release
sudo mkdir -p /etc/apt/keyrings
sudo curl -fsSL https://download.docker.com/linux/ubuntu/gpg -o /etc/apt/keyrings/docker.gpg
sudo chmod a+r /etc/apt/keyrings/docker.gpg

echo "Step 3: Adding the Docker repository to Apt sources..."
echo \
  "deb [arch=$(dpkg --print-architecture) signed-by=/etc/apt/keyrings/docker.gpg] https://download.docker.com/linux/ubuntu \
  $(lsb_release -cs) stable" | \
  sudo tee /etc/apt/sources.list.d/docker.list > /dev/null
sudo apt-get update

echo "Step 4: Installing Docker Engine, Docker CLI, Containerd, and Docker Compose plugin..."
sudo apt-get install -y docker-ce docker-ce-cli containerd.io docker-compose-plugin

echo "Making Docker Compose executable..."
sudo ln -sf /usr/bin/docker-compose /usr/local/bin/docker-compose

echo "Step 5: Installing Certbot for SSL certificate generation..."
sudo snap install core; sudo snap refresh core
sudo snap install --classic certbot
sudo ln -sf /snap/bin/certbot /usr/bin/certbot

echo "Optional: Generating SSL certificate using Certbot..."
sudo certbot certonly --standalone -d arelymp.com 

echo "Opening necessary ports for NGINX and MSSQL (Development environment)..."
sudo ufw allow 21
sudo ufw allow 80
sudo ufw allow 443
sudo ufw allow 5432

echo "Step 6: Starting Docker services using Compose..."

sudo docker-compose -f $DIRECTORY/docker-compose.yml up -d

echo "Step 7: Installing .NET runtime..."
wget https://packages.microsoft.com/config/ubuntu/22.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
sudo dpkg -i packages-microsoft-prod.deb
rm packages-microsoft-prod.deb

sudo apt-get update
sudo apt-get install -y apt-transport-https
sudo apt-get update
sudo apt-get install -y dotnet-runtime-8.0
sudo apt-get install -y aspnetcore-runtime-8.0

echo "Step 8: Setting up deployment directory and copying deployment scripts..."
mkdir /root/deployment
cp ./api-deployment.sh /root/deployment
sudo cp ./kestrel-arelymp-backend.service /etc/systemd/system

echo "Step 9: Reloading systemd and enabling the service..."
sudo systemctl daemon-reload
sudo systemctl enable kestrel-arelymp-backend.service

echo "Step 10: Installing missing programms"
sudo apt install unzip

echo "Setup completed."
