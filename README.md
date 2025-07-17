# IKitaplik - Library Management System

[🇹🇷 Türkçe](README_TR.md) \| [🇬🇧 English](README.md)

## Overview

**IKitaplik** is a modular, full-stack Library Management System built with modern .NET technologies. It provides a robust backend REST API, a clean and interactive Blazor UI frontend, and a layered architecture for maintainability and scalability. The system supports book, student, writer, category, deposit (borrowing), donation, and user management, with authentication and authorization.

---

## Table of Contents

- [Architecture](#architecture)
- [Technologies](#technologies)
- [Features](#features)
- [Project Structure](#project-structure)
- [Getting Started](#getting-started)
- [API Endpoints](#api-endpoints)
- [UI Overview](#ui-overview)
- [Entities](#entities)
- [Contributing](#contributing)
- [License](#license)

---

## Architecture

The solution is organized into several projects, each with a clear responsibility:

- **Core**: Shared base entities, interfaces, and utility classes.
- **IKitaplik.Entities**: Entity and DTO definitions for the domain model.
- **IKitaplik.DataAccess**: Data access layer using Entity Framework Core, repository, and unit of work patterns.
- **IKitaplik.Business**: Business logic, service interfaces, and implementations, including validation.
- **IKitaplik.Api**: ASP.NET Core Web API exposing RESTful endpoints.
- **IKitaplik.BlazorUI**: Blazor Server UI for end-users, with authentication and CRUD operations.

---

## Technologies

- **.NET 7+ / .NET 8+**
- **ASP.NET Core Web API**
- **Entity Framework Core**
- **Blazor Server**
- **AutoMapper**
- **FluentValidation**
- **JWT Authentication**
- **MudBlazor** (UI components)
- **Blazored.LocalStorage** (for token storage)
- **Swagger/OpenAPI** (API documentation)

---

## Features

- **Book Management**: Add, update, delete, list, and filter books.
- **Student Management**: Register, update, delete, and list students.
- **Writer & Category Management**: CRUD operations for writers and categories.
- **Deposit (Borrowing) System**: Track which student has borrowed which book, due dates, and returns.
- **Donation Tracking**: Manage book donations from students.
- **User Authentication**: Register, login, JWT-based authentication, and role-based authorization.
- **Movements Log**: Track actions (borrowing, returning, donations, etc.) for auditability.
- **Responsive UI**: Modern, interactive Blazor Server frontend with MudBlazor components.
- **Validation**: Server-side validation using FluentValidation.
- **API Documentation**: Integrated Swagger UI for API exploration.

---

## Project Structure

```
IKitaplik.sln
│
├── Core/                  # Base entities, interfaces, utilities
├── IKitaplik.Entities/    # Domain entities and DTOs
├── IKitaplik.DataAccess/  # EF Core repositories, migrations, unit of work
├── IKitaplik.Business/    # Business logic, services, validation
├── IKitaplik.Api/         # ASP.NET Core Web API
└── IKitaplik.BlazorUI/    # Blazor Server UI
```

---

## Getting Started

### Prerequisites

- [.NET SDK 7+](https://dotnet.microsoft.com/download)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) or compatible database

### Setup

1. **Clone the repository:**
   ```bash
   git clone https://github.com/hamzairmaktr/KutuphaneOtamasyonu
   cd KutuphaneOtamasyonu
   ```

2. **Configure the database:**
   - Update the connection string in `IKitaplik.Api/appsettings.json` and `IKitaplik.BlazorUI/appsettings.json`.

3. **Apply Migrations:**
   ```bash
   dotnet ef database update --project IKitaplik.DataAccess
   ```

4. **Run the API:**
   ```bash
   dotnet run --project IKitaplik.Api
   ```

5. **Run the Blazor UI:**
   ```bash
   dotnet run --project IKitaplik.BlazorUI
   ```

6. **Access the application:**
   - API: `https://localhost:<port>/swagger`
   - UI: `https://localhost:<port>/`

---

## API Endpoints

The API exposes endpoints for all major entities. Example endpoints:

- `POST /api/auth/login` - User login
- `POST /api/auth/register` - User registration
- `GET /api/book/getall` - List all books
- `POST /api/book/add` - Add a new book
- `POST /api/book/update` - Update a book
- `POST /api/book/delete` - Delete a book
- `GET /api/student/getall` - List all students
- ...and more for categories, writers, deposits, donations, and movements.

All endpoints are protected by JWT authentication and role-based authorization.

---

## UI Overview

- **Login/Register:** Secure authentication for users.
- **Navigation:** Sidebar with links to Home, Book List, Add Book, etc.
- **Book List:** Paginated, searchable, and sortable table of books.
- **Book Add:** Form to add new books.
- **Student Management:** List, add, update, and delete students.
- **Responsive Design:** Built with MudBlazor for a modern look and feel.

---

## Entities

### Core Entities

- **Book:** Barcode, Name, Category, Writer, Shelf, Piece, Situation, PageSize, etc.
- **Student:** StudentNumber, Name, Class, Telephone, Email, NumberOfBooksRead, Point, etc.
- **Writer:** Name, BirthDate, DeathDate, Biography.
- **Category:** Name.
- **Deposit:** BookId, StudentId, IssueDate, DeliveryDate, IsDelivered, etc.
- **Donation:** Date, StudentId, BookId, IsItDamaged.
- **User:** Username, FullName, Email, PasswordHash, Role, RefreshToken, etc.
- **Movement:** MovementDate, Title, Note, Type, related entity IDs.

### DTOs

DTOs are used for API communication and validation, ensuring separation between internal models and external contracts.

---

## Contributing

Contributions are welcome! Please open issues or submit pull requests for improvements, bug fixes, or new features.

---

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.

---

**Note:** For detailed API contracts, see the Swagger UI at `/swagger` when the API is running. 
