# Temp Back-end

<!---
## 📑 Table of Contents
- [⭐ Introduction](#-introduction)
- [🚧 Development setup](#-development-setup)
- [📚 Project structure](#-project-structure)
-->

## ⭐ Introduction

This repository contains the back-end code for the Temp project.

## 🚧 Development setup

### ℹ️ Prerequisites

Before starting, ensure you have the following installed on your machine:

- DotNet SDK 9.0

### 1. Clone the repository

First, clone the repository and navigate to the project directory:

```bash
git clone <repository_url>
cd <project_directory>
```

### 2. Configure Application Settings

Update the configuration files located at:

- `Temp.API/appsettings.json`
- `Temp.API/appsettings.Development.json`
  Ensure the database connection strings are properly configured.

### 3. Restore Dependencies

If you're using an IDE like Visual Studio or Rider, this step may be automatically handled.

To manually restore the required NuGet packages, run the following command from the root directory:

```bash
dotnet restore
```

### 4. Run the Application

You can run the application using one of the following methods:

#### 👉 Using the IDE:

- Configure `Temp.API` as the startup project.
- Click the "Run" ▶️ button.

#### 👉 Using the cmd:

At the root directory, launch the application with:

```bash
dotnet run --project Temp.API
```

### 🚀 API Endpoint

Once the application is running, it will listen on the following URLs:

- HTTP: **http://localhost:5031**
- HTTPS: **https://localhost:7189**

## 📚 Project structure

This project follows the Domain-Driven Design (DDD) approach, which helps organize the code into distinct layers that focus on different concerns of the application.

### 📔 Temp.API

The `Temp.API` project is an **ASP.NET Core API** and serves as the entry point for the application. It contains the web API layer that handles HTTP requests, controllers, routing, and middlewares. This project is responsible for exposing endpoints to the client.

### 📕 Temp.Application

The `Temp.Application` is a **Class Library** that contains the application's business logic and service layer. It contains the **Application Services, and DTOs**.

### 📗 Temp.Domain

The `Temp.Domain` is a **Class Library** that defines the core business logic of the application. Including the **Domain Entities, Models, Interfaces, and Exceptions, etc**.

### 📘 Temp.EFCore

The `Temp.EFCore` is a **Class Library** contains the **Database Context, Configurations, and Migrations**. It defines how the application interacts with the database using **Entity Framework Core**.

### 📙 Temp.Infrastructure

The `Temp.Infrastructure` is a **Class Library** that provides implementations for external dependencies and services. It includes infrastructure concerns such as **Data Repositories, Redis Caching, Email Services, etc**.
