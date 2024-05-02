# Webhook System for Flight Information Management

This README provides an overview and instructions for setting up and using the webhook system for maintaining flight information between travel agents and an airline company. The system is built using Visual Studio with .NET 8, SQL Server, RabbitMQ, and Docker.

## Overview

The webhook system facilitates real-time communication between travel agents and the airline company, ensuring efficient management of flight information. It allows travel agents to subscribe to receive updates about flight information.

## Components

- **Visual Studio**: Integrated Development Environment (IDE) for building the application.
- **.NET 8**: Framework for developing web applications and APIs.
- **SQL Server**: Relational database management system for storing flight information and webhook subscriptions.
- **RabbitMQ**: Message broker for handling asynchronous communication between components.
- **Docker**: Containerization platform for deploying and managing application components.

## Prerequisites

Before setting up the webhook system, ensure the following prerequisites are met:

- Visual Studio installed on your development machine.
- .NET 8 SDK installed.
- SQL Server instance available for database operations.
- RabbitMQ server running for message queueing.
- Docker installed for containerization.
- Insomnia or similar tool (Postman) for sending POST requests to register webhooks.

## Setup Instructions

### 1. Clone the Repository

git clone https://github.com/TotalNodal/05aWebhookSystem


### 2. Configure Database

- Update the connection string for "TravelAgentConnection" in `TravelAgentWeb\appsettings.Development.json` to point to your SQL Server instance.

- Update the connection string for "AirlineConnection" in `AirlineWeb\appsettings.Development.json` to point to your SQL Server instance.

- Update the connection string for "AirlineConnection" in `AirlineSendAgent\appsettings.json` to point to your SQL Server instance.

- Run Entity Framework Core migrations to create the database schema: using the following command dotnet ef database update


### 3. Set Up RabbitMQ

- Ensure RabbitMQ server is running.
- Check version compatability in AirlineWeb.csproj and AirlineSendAgent.csproj

### 4. Build and Run the Application

- Open three powershell terminals
- Change working directory of each terminal to all three project folders, TravelAgentWeb, AirlineWeb and AirlineSendAgent
- Run the command "dotnet run" without quoatation marks in the AirlineWeb console
- Run the command "dotnet run" without quoatation marks in the TravelAgentWeb console
- Run the command "dotnet run" without quoatation marks in the AirlineSendAgent console

### 5. Docker Containerization

- Dockerize the application components for deployment:

- docker-compose up --build


### 6. Register Webhooks

To register a webhook subscription, open Insomnia or similar tool and follow these steps:

1. **Create Endpoint for Travel Agent**: Use the following to generate a secret that will be used by a travel agent to validate POST requests. Once all three apps are running copy and past this into a browser https://localhost:5121/index.html and then use this as the URI http://localhost:5113/api/Notifications

2. **Update SubscriptionSecrets in db**: Manually enter the secret from step 1 into the "secret" column in the SubscriptionsSecret table on sql server and enter a publisher name e.g. CardoAir to register that endpoint to listen for all updates. If you right click on the table then click "edit top 200 rows" you will be able to input this information there.   

3. **Create Flight in db**: Use the following endpoint to create a flight record in the db:

- POST Request to https://localhost:5121/api/Flights with payload type JSON, payload example
{
	"FlightCode": "SK932",
	"Price": 1000.00
}

4. **Update Flight in db**: Use the following endpoint to update a flight record in the db:

- PUT request to https://localhost:5121/api/Flights/1 with payload type JSON, payload example
{
	"FlightCode": "SK932",
	"Price": 500.00
}


5. **Get Flight Details**: Use the following endpoint to get a flight record from the db:

- GET request to https://localhost:5121/api/Flights/SK932 





Contributors
[Josh Lewis]
[Les Jackson - Udemy Tutorial - Webhooks with .NET 5]
