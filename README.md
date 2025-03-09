This is an e-commerce website built using ASP.NET MVC technology. It allows users to browse products, create product ,create category , and proceed to checkout. The project uses SQL Server as the database and Entity Framework for data access. The application follows the Repository Pattern and uses a Three-Layer Architecture for clean and maintainable code.

Technologies Used :->
ASP.NET MVC: A powerful framework for building web applications using the Model-View-Controller architecture.
SQL Server: A relational database management system to store product, user, and order information.
Entity Framework: An Object-Relational Mapping (ORM) tool to interact with the SQL Server database.
Repository Pattern: A design pattern used to abstract the data access logic from the application logic, making the code more modular and testable.
Three-Layer Architecture: The project follows a 3-tier architecture consisting of:
Presentation Layer: The front-end, consisting of Razor Views and controllers.
Business Logic Layer: Where the application’s core logic resides (Service Layer).
Data Access Layer: Interacts with the database through Entity Framework.


Prerequisites:
Visual Studio (or any IDE that supports ASP.NET MVC)
SQL Server (or SQL Server Express installed on your local machine)
.NET Framework (version compatible with ASP.NET MVC)


Folder Structure
Controllers: Contains the logic to handle HTTP requests and responses.
Models: Represents the data structure and business logic.
Views: Razor Views that display the HTML content.
Data: Contains the DbContext and database interaction logic through Entity Framework.
Repositories: Contains the repository classes that implement the data access logic using the Repository Pattern.


