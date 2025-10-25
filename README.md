.NET API Boilerplate Template
=============================

A ready-to-use .NET API solution template for quickly starting new projects with best practices and a clean architecture.

Features
--------

*   Clean architecture and folder structure
    
*   Pre-configured for RESTful APIs
    
*   Easily customizable solution and project names (**\_\_SolutionName\_\_**)
    
*   Supports dependency injection, logging, and configuration out of the box
    
*   Ready for Docker and CI/CD integration
    

Getting Started
---------------

### 1\. Install the Template

Clone or download this repository, then run the following command in the root directory:

`   dotnet new --install .   `

This will install the template locally on your machine.

### 2\. Create a New API Solution

To create a new solution using this template, run:

`   dotnet new dotnetapiboilerplate -n YourSolutionName   `

Replace **YourSolutionName** with your desired solution name. All instances of **\_\_SolutionName\_\_** in the template will be replaced automatically.

### 3\. Run Your New API Locally

Navigate to your new solution directory and run:

`   dotnet build  dotnet run --project src/YourSolutionName.Api   `

Docker Support
--------------

### Build and Run with Docker

1.  docker build -t your-solution-name-api .
    
2.  docker run -d -p 8080:80 --name your-solution-name-api your-solution-name-apiThe API will be available at [http://localhost:8080](http://localhost:8080/).
    

### Customizing Docker

*   Edit the **Dockerfile** to change build or runtime options as needed.
    
*   Use environment variables for configuration (see **appsettings.json**).
    

Customization
-------------

*   Update namespaces, project references, and configuration as needed for your project.
    
*   Add or remove features according to your requirements.
    

Contributing
------------

Contributions are welcome! Please open issues or submit pull requests for improvements or bug fixes.

License
-------

This project is licensed under the MIT License.
