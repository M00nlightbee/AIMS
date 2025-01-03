# AIMS: Inventory Management System

## Overview
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

---

## Project Structure
AIMS/ ├── Controllers/ │ ├── HomeController.cs │ ├── InventoryController.cs │ ├── OrderController.cs │ ├── ProductController.cs │ └── UserController.cs ├── Data/ │ ├── DataAccess.cs │ ├── ProductData.cs │ └── UserData.cs ├── Models/ │ ├── Product.cs │ ├── Users.cs │ └── ErrorViewModel.cs ├── Views/ │ ├── Home/ │ ├── Inventory/ │ ├── User/ │ └── Shared/ ├── wwwroot/ │ ├── css/ │ │ └── site.css ├── appsettings.json ├── Program.cs ├── launchSettings.json


---

## Getting Started

### Prerequisites
- Install [.NET 6 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)
- Install [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
- Install [Visual Studio 2022](https://visualstudio.microsoft.com/) with ASP.NET and web development workload.

### Installation
1. Clone the repository:
   ```bash
   git clone https://github.com/M00nlightbee/AIMS.git
   cd aims
Set up the database:
Configure the connection string in appsettings.json.
Run the application:
dotnet run
Access the application in your browser:
http://localhost:5213
Usage

Branch Managers: Monitor and manage branch-specific inventory.
Administrators: Oversee all users, roles, and analytics.
Sales Assistants: View inventory details and assist with sales operations.
Contributing



This project is licensed under the MIT License. See the LICENSE file for details.

Contact

For queries or feedback:

Email: Aim@gmail.com
