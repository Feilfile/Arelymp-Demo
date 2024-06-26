#!/bin/bash

kubectl delete -f ./kubernetes.yml

docker build -f Dockerfile.Frontend -t openmatch-frontend:v1 .

docker build -f Dockerfile.Director -t openmatch-director:v1 .

docker build -f Dockerfile.MatchFunction -t openmatch-match-function:v1 .

kubectl apply -f ./kubernetes.yml