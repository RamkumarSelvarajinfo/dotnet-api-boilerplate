.NET API Boilerplate Template
=============================

A ready-to-use .NET 8 Web API solution template for quickly starting new projects with best practices and a clean architecture.

✨ Features
----------

*   Clean architecture and folder structure
    
*   Pre-configured for RESTful APIs
    
*   Easily customizable solution and project names (\_\_SolutionName\_\_)
    
*   Supports dependency injection, logging, and configuration out of the box
    
*   Integrated **Entity Framework Core 8 (Code-First)** setup
    
*   Auditable entities with CreatedBy, CreatedOn, UpdatedBy, and UpdatedOn
    
*   Ready for **JWT Authentication** integration
    
*   Ready for **Docker** and **CI/CD** deployment
    

🚀 Getting Started
------------------

### 1\. Install the Template

Clone or download this repository, then run the following command in the root directory:

`   dotnet new --install .   `

This will install the template locally on your machine.

### 2\. Create a New API Solution

To create a new solution using this template:

`   dotnet new dotnetapiboilerplate -n YourSolutionName   `

Replace **YourSolutionName** with your desired project name.All placeholders (\_\_SolutionName\_\_) will be automatically replaced.

### 3\. Run Your New API Locally

Navigate to your new solution directory and run:

`   dotnet build  dotnet run --project src/YourSolutionName.Api   `

🗄️ Database Setup (EF Core Code-First)
---------------------------------------

This template includes a **ready-to-use EF Core setup** with:

*   AppDbContext (infrastructure layer)
    
*   Repository & Unit of Work pattern
    
*   Auditable and soft-delete support
    
*   Optional data seeding
    

Follow these steps to generate and apply migrations:

### 1️⃣ Configure Connection String

Edit the appsettings.json file in your **API project** (YourSolutionName.Api) and set your connection string:

`   {    "ConnectionStrings": {      "DefaultConnection": "Server=localhost;Database=YourSolutionNameDb;Trusted_Connection=True;TrustServerCertificate=True;"    }  }   `

### 2️⃣ Add EF Tools (if not installed)

`   dotnet tool install --global dotnet-ef   `

### 3️⃣ Create Initial Migration

From the **solution root**, run:

`   dotnet ef migrations add InitialCreate --project src/YourSolutionName.Infrastructure --startup-project src/YourSolutionName.Api   `

### 4️⃣ Apply Migration to Database

`   dotnet ef database update --project src/YourSolutionName.Infrastructure --startup-project src/YourSolutionName.Api   `

### 5️⃣ Verify Tables

After running the update, open your SQL Server and verify:

*   Your database (e.g., YourSolutionNameDb) is created
    
*   Tables such as Flights and \_\_EFMigrationsHistory exist
    

### 6️⃣ Add New Migrations (after Model Changes)

Whenever you modify domain entities, simply run:

`   dotnet ef migrations add AddNewEntity --project src/YourSolutionName.Infrastructure --startup-project src/YourSolutionName.Api  dotnet ef database update --project src/YourSolutionName.Infrastructure --startup-project src/YourSolutionName.Api   `

### ⚙️ Optional: Run Migrations Automatically on Startup

You can apply migrations automatically when the API starts by adding the following snippet in your Program.cs:

`   using (var scope = app.Services.CreateScope())  {      var db = scope.ServiceProvider.GetRequiredService();      db.Database.Migrate();  }   `

This ensures the database is up-to-date each time you deploy.

🐳 Docker Support
-----------------

### Build and Run with Docker

`   docker build -t your-solution-name-api .  docker run -d -p 8080:80 --name your-solution-name-api your-solution-name-api   `

Your API will be available at [http://localhost:8080](http://localhost:8080).

### Customize Docker

*   Modify the **Dockerfile** to change build or runtime options.
    
*   Use environment variables for configuration (e.g., connection strings, JWT settings).
    

🔧 Customization
----------------

*   Update namespaces, references, and configuration for your project.
    
*   Add custom repositories, services, or modules as needed.
    
*   Extend AppDbContext for new entities and relationships.
    

🤝 Contributing
---------------

Contributions are welcome! Please open issues or submit pull requests for improvements or bug fixes.

📜 License
----------

This project is licensed under the **MIT License**.
