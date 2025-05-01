# AIMS: Inventory Management System

## About this project
AIMS is a versatile ASP.NET Core MVC application tailored for efficient inventory management and user administration. It provides a streamlined interface for branch managers, administrators, and sales assistants to manage products, track inventory, and gain insights through analytics.

---

## Features
### Inventory Management
- Add, edit, delete, and view inventory items.
- Track product quantities, categories, and branch allocations.
- Generate alerts for low-stock items.

### User Management
- Create and manage user accounts for different roles:
  - **Branch Managers**
  - **Administrators**
  - **Sales Assistants**
- Assign users to specific branches.

### Analytics Dashboard
- Visualise inventory and user statistics.
- Analyse inventory value and low-stock alerts.
- Role-based insights for effective decision-making.

### Responsive Design
- Fully styled with **CSS** and responsive layouts.
- Interactive features powered by **JavaScript**.

---

## Technologies Used
- **Backend:** ASP.NET Core MVC (C#)
- **Frontend:** HTML, CSS, JavaScript
- **Database:** SQL Server
- **Styling:** Bootstrap, custom CSS
- **Charts:** Chart.js for interactive data visualizations
- **Dependency Injection:** Built-in ASP.NET Core DI
- **Environment:** Visual Studio 2022 / .NET 6+
- **Version Control:** Git
* ![ASP.NET Core](https://img.shields.io/badge/-ASP.NET%20Core-5C2D91?style=for-the-badge&logo=.net&logoColor=white)
* ![C#](https://img.shields.io/badge/-C%23-239120?style=for-the-badge&logo=csharp&logoColor=white)
* ![SQL Server](https://img.shields.io/badge/-SQL%20Server-CC2927?style=for-the-badge&logo=microsoft-sql-server&logoColor=white)
* ![Bootstrap](https://img.shields.io/badge/-Bootstrap-563D7C?style=for-the-badge&logo=bootstrap&logoColor=white)
* ![Chart.js](https://img.shields.io/badge/-Chart.js-FF6384?style=for-the-badge&logo=chartdotjs&logoColor=white)
---

## Project Structure
AIMS/ 
├── Controllers/ │ 
  ├── HomeController.cs │ 
  ├── InventoryController.cs │ 
  ├── OrderController.cs │ 
  ├── ProductController.cs │ 
  └── UserController.cs 
├── Data/ │ 
  ├── DataAccess.cs │ 
  ├── ProductData.cs │ 
  ├── UserData.cs 
├── Models/ │ 
  ├── Product.cs │ 
  ├── Users.cs │ 
  └── ErrorViewModel.cs 
├── Views/ │ 
  ├── Home/ │ 
  ├── Inventory/ │ 
  ├── User/ │ 
└── Shared/ 
  ├── wwwroot/ │ 
  ├── css/ │ 
  │ └── site.css 
  ├── appsettings.json 
  ├── Program.cs 
  ├── launchSettings.json

---

## Getting Started

### Prerequisites
- Install [.NET 6 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)
- Install [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
- Install [Visual Studio 2022](https://visualstudio.microsoft.com/) with ASP.NET and web development workload.

### Installation

1. Open SQL Server Object Explorer
Launch Visual Studio and navigate to View > SQL Server Object Explorer.

2. Connect to the Database
In SQL Server Object Explorer, click Add SQL Server. Select localdb\MSSQLLocalDB and click Connect.

3. Create the Database
Right-click Databases in the connected server, select Add New Database, name it AIMS, and click OK.

4. Create Tables
Expand the AIMS database node, right-click Tables, and select Add New Table. Use the following structure:

```bash
CREATE TABLE [dbo].[Users] (
    [UserId]       INT           IDENTITY (1, 1) NOT NULL,
    [FirstName]    NVARCHAR (50) NULL,
    [LastName]     NVARCHAR (50) NULL,
    [Branch]       NVARCHAR (50) NULL,
    [Role]         NVARCHAR (50) NULL,
    [Email]        NVARCHAR (50) NULL,
    [UserPassword] NVARCHAR (50) NULL,
    [CreatedDate]  DATE          DEFAULT (getdate()) NOT NULL,
    [UpdatedDate]  DATE          DEFAULT (getdate()) NOT NULL,
    PRIMARY KEY CLUSTERED ([UserId] ASC)
);
```

```bash
CREATE TABLE [dbo].[Product] (
    [ProductId]          INT            IDENTITY (1, 1) NOT NULL,
    [ProductName]        NVARCHAR (50)  NOT NULL,
    [ProductDescription] NVARCHAR (MAX) NOT NULL,
    [ProductSize]        NVARCHAR (50)  NOT NULL,
    [Quantity]           INT            NOT NULL,
    [Price]              MONEY          NOT NULL,
    [Branch]             NVARCHAR (50)  NOT NULL,
    [Category]           NVARCHAR (50)  NOT NULL,
    [CreatedDate]        DATE           DEFAULT (getdate()) NOT NULL,
    [UpdatedDate]        DATE           DEFAULT (getdate()) NOT NULL,
    PRIMARY KEY CLUSTERED ([ProductId] ASC)
);
```

```bash
 CREATE TABLE [dbo].[Orders] (
    [OrderId]       INT  IDENTITY (1, 1) NOT NULL,
    [OrderQuantity] INT  NOT NULL,
    [CreatedAt]     DATE DEFAULT (getdate()) NOT NULL,
    [ProductId]     INT  NULL,
    PRIMARY KEY CLUSTERED ([OrderId] ASC),
    FOREIGN KEY ([ProductId]) REFERENCES [dbo].[Product] ([ProductId])
);
```

```bash
CREATE TABLE [dbo].[ArchivedOrderDetails] (
    [ArchivedOrderId] INT  IDENTITY (1, 1) NOT NULL,
    [OrderId]         INT  NOT NULL,
    [OrderNumber]     INT  NOT NULL,
    [OrderQuantity]   INT  NOT NULL,
    [CreatedAt]       DATE DEFAULT (getdate()) NOT NULL,
    [ProductId]       INT  NULL,
    PRIMARY KEY CLUSTERED ([ArchivedOrderId] ASC),
    FOREIGN KEY ([ProductId]) REFERENCES [dbo].[Product] ([ProductId])
);
```

5. Insert Dummy Data Open a new query in SQL Server Object Explorer and execute the following scripts:

```bash
INSERT INTO [dbo].[Users] ([FirstName], [LastName], [Branch], [Role], [Email], [UserPassword], [CreatedDate], [UpdatedDate])
VALUES 
('Alice', 'Admin', 'Sheffield', 'Admin', 'alice.admin@example.com', 'password123', '2025-01-01 19:08:08.790', '2025-01-01 19:08:08.790'),
('Bob', 'Manager', 'London', 'Branch Manager', 'bob.manager@example.com', 'password123', '2025-01-01 19:08:08.790', '2025-01-01 19:08:08.790'),
('Charlie', 'Sales', 'Derby', 'Sales Associate', 'charlie.sales@example.com', 'password123', '2025-01-01 19:08:08.790', '2025-01-01 19:08:08.790');
```
Configure the connection string in appsettings.json.
Run the application:
dotnet run
Access the application in your browser:
http://localhost:5213
---
Usage:
Branch Managers: Monitor and manage branch-specific inventory.
Administrators: Oversee all users, roles, and analytics.
Sales Assistants: View inventory details and assist with sales operations.
Contributing


This project is licensed under the MIT License. See the LICENSE file for details.

Contact for queries or feedback:
Email: Aim@gmail.com
