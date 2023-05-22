# Introduction 
Bookish Bits is an application built using technologies provided by the .NET framework. The application itself is built using .NET MAUI, and uses a microservice approach to client-server communication architecture. 

Bookish Bits aims to provide an online platform around a shared interest of literature. Version 1 includes functionality to search and browse for specific books, authors, and users, as well as interact with these elements by adding books and other literary pulications to a user's personal digital library, connecting with other users via social media, comments, and private messaging, as well as providing a marketplace for others to buy and sell their own physical books.

Some of the most significant technologies used during development this project are: 
- C#
- Xaml
- MAUI
- Entity Framework Core
    - SQLServer & SSMS
- ASP.NET
    - REST
    - GRPC
    - Swagger
- Hangfire
- SignalR

# Getting Started
This repository contains source code that must be built and compiled before it can be executed. No artifacts or executable files are located in this repository. 

This project was developed for use with the Windows OS. To get started, clone this repository and then open the repositories' solution in Visual Studio, then:
1.	Select 'Build' from the main menu.
2.	Select 'Build Solution' from the drop-down menu. This will download project dependencies and compile the code so that it can be executed.
3.	Make sure you have the latest version of [SQLServer](https://www.microsoft.com/en-us/sql-server/sql-server-downloads). The free version (Developer Edition) will suffice. Additionally, download [SQL Server Management Studio (SSMS)](https://learn.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms?view=sql-server-ver16) if you wish to preview the databases, or set up the background tasker.
    1. The SQL Server is set up using these values: 
        - Server Type: Database Engine
        - Server Name: localhost
        - Authentication: Windows Authentication
    2. Select 'Tools' from the main menu, then 'NuGet Package Manager', and finally, select 'Package Manager Console'
    3. Use Entity Framework Core to create the required databases, together with required tables and dummy data (seeds):
        1. Navigate to each individual project using the Package Manager Console using the command 'cd <`package file path`>'. If you wish to navigate back to the root of the project, simply type 'cd ..'.
        2. Run the command 'Update-Database' for each of the listed projects:
            - UserAccountService
                - 'Update-Database -Context UserAccountContext'.
            - UserDisplayPictureService
            - MessagingService
            - LibraryCollectionService
            - ReadingGoalService
            - CommentService
            - MarketplaceService
    4. To set up the background tasker, first open up SSMS and login using the details stipulated in 3.1. Create a database named 'background_task' and then run the BackgroundTaskService to populate the database with the required tables.

Alternatively, if you wish to do the above through a terminal window then:
1. Navigate to the root of the project (where the solution file is located), and enter 'dotnet build Hiof.DotNetCourse.V2023.Group14/Hiof.DotNetCourse.V2023.Group14.sln'
2. Install Entity Framework Core by entering 'dotnet tool install --global dotnet-ef'
3. To create the required databases, navigate to each project location listed in 3.3.2, and type 'dotnet ef database update'
    1. For 'UserAccountService', you will need to enter 'dotnet ef database update --context UserAccountContext'

# Launching
1.	Right click on the solution and select 'Configure Startup Projects', then select the 'Multiple Startup Projects' radio button.
    1. Select the dropdown option 'Start Without Debugging' for the following projects:
        - APICommunicatorService
        - BackgroundTaskService
        - CommentService
        - LibraryCollectionService
        - MarketplaceService
        - MessagingService
        - ProxyService
        - ReadingGoalService
        - UserAccountService
        - UserDisplayPictureService
    2. For the 'BookAppMaui' project, either choose 'Start Without Debugging' or 'Start'.
2. Select 'Debug' from the main menu, and then select either 'Start Debugging' or 'Start Without Debugging' (depending on what you selected for the 'BookAppMaui' project).
3. Select the checkbox marked 'Test Account' to automatically fill in the Username and Password forms with a dummy account used for development and preview purposes.
4. Upon clicking log in you should be redirected to the main page. From here you can navigate to all existing pages within the application.

# Tests
1. Tests can be run via Visual Studio by selecting 'Test' from the main menu, and then selecting 'Run All Tests'.
    1. Alternatively, tests can be run through a terminal window by typing 'dotnet test Hiof.DotNetCourse.V2023.Group14/Hiof.DotNetCourse.V2023.Group14.sln'