# Arelymp - Documentation

Welcome to the Arelymp mobile game project documentation. This guide provides an in-depth look into the various components and complex topics associated with the project, including different authentication methods, backend services, database setup, matchmaking, and pipeline configurations.

## Table of Contents

<ol>
<li> <a href="#1-general-architecture"> General Architecture</li>
<li><a href="./Docs/Authentication.md">Authentication</a> </li>
<li><a href="./Docs/Database.md">Database, EntityFramework, and Code First Database</a> </li>
<li> <a href="./Docs/Matchmaker.md">OpenMatch Implementation + Kubernetes Setup</a> </li>
<li> <a href="https://arelymp.com/swagger">Api Documentation</a></li>
<li><a href="./Docs/Pipelines.md"> Other Pipelines</a> </li>
</ol>

## 1. General architecture

![image](./Docs/GameArchitecture.png)

Arelymp is a mobile game built using the Unity engine and the Mirror framework for client-to-game server communication. The game relies on a centralized backend to authorize all external requests, manage, and persist data. Authentication is handled via Google Cloud, employing the OAuth2 PKCE method for secure frontend (Unity) authentication.

For a robust and scalable matchmaking system, Arelymp utilizes the open-source project OpenMatch. OpenMatch, powered by Kubernetes, dynamically deploys game server instances, adding significant complexity to the project but ensuring stability and scalability.

