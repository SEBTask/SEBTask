# How to run/use the application
* Clone the repository
* Have SQL server Express installed on Your machine (SQL Server 2017 Express edition was used for development) and SQL Server with name localhost\SQLEXPRESS running
* In Visual Studio's Packet Manager Console execute command `update-database' to apply pending migrations
* Run the application's API project via IIS Express in Visual Studio
* Interact with the API via Swagger UI (accesed via url localhost:url/swagger/index.html) or via some other interface (browser, Postman, etc.)

# Explanation of choices
## Framework - ASP.NET Core
* A great framework that i enjoy working with
##

## Database/data access - SQL Server with EF Core
* SQL Server is the default database choice when working with ASP.NET Core
* EF Core is the default ORM choice when working with ASP.NET Core
##

## Architecture - "Onion" based 
* This architecture was chosen, because it's the architecture where everything else is built around the domain logic
##

## Unit test libraries - NUnit and Moq  
* They were chosen because i've used them previously and they did the job
##
