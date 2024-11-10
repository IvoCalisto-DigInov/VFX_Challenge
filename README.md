
VFX Challenge - Exchange Rate API
Overview
This application provides an API to fetch and manage currency exchange rates. It supports retrieving rates from a local database and, when not available, querying an external API for the latest rate. This README guides you through setting up the application locally, publishing it to Azure and IIS, and outlines some limitations and potential improvements.

Getting Started
Prerequisites
Visual Studio 2022 or later (with .NET Core SDK installed)
SQL Server for database setup (optional for in-memory testing)
Azure Account (for Azure deployment)
IIS for local or on-premise server deployment
Local Development in Visual Studio
Clone the Repository

bash
Copiar código
git clone <repository-url>
cd VFX_Challenge
Open the Solution

Launch Visual Studio and open VFX_Challenge.sln.
Configure the Database

If using a SQL Server instance:
Open appsettings.json and configure the connection string under ConnectionStrings.
If using an in-memory database (for testing only):
Update the Startup.cs file to use an in-memory database instead of SQL Server.
Run Database Migrations

In Visual Studio’s Package Manager Console, navigate to the project directory and run:
bash
Copiar código
Update-Database
Run the Application

Press F5 or select Start Debugging in Visual Studio to launch the API. It should run at https://localhost:5001 or a similar local port.
Test the API

Use tools like Postman or a web browser to test the API endpoints, such as:
bash
Copiar código
GET https://localhost:5001/api/exchangerate/USD/EUR
Deployment to Azure
Publishing the Application to Azure App Service
Create an App Service

Log into the Azure Portal.
Navigate to App Services and select Create.
Set up your application by configuring the resource group, instance name, runtime stack (e.g., .NET Core 8), and region.
Publish from Visual Studio

In Visual Studio, right-click on the project and select Publish.
Choose Azure as the target and select Azure App Service (Windows) or App Service (Linux) based on your configuration.
Click Next, select your App Service, and click Finish.
Visual Studio will deploy the application to Azure, and you can monitor the process in the Output window.
Configure App Settings on Azure

Go to the Azure App Service resource.
Under Settings, select Configuration.
Add or modify settings such as ConnectionStrings if you are using an Azure SQL Database or other external resources.
Access the Live Application

Once published, your API will be accessible at https://<your-app-name>.azurewebsites.net.
Deployment to IIS
Steps for IIS Deployment
Publish the Project

In Visual Studio, right-click on the project and select Publish.
Choose Folder as the target and specify a local folder for the published output.
Visual Studio will publish the project files to the specified folder.
Configure IIS

Open IIS Manager.
Right-click on Sites and select Add Website.
Enter the Site Name, Physical Path (pointing to your published folder), and Port (e.g., 8080).
Click OK to create the site.
Update Application Pool Settings

Ensure the Application Pool is set to use No Managed Code if running as a self-contained .NET app, or set it to .NET CLR if required.
Set Up Permissions

Give the IIS_IUSRS group read and execute permissions for the folder where your application is published.
Access the Application

Visit http://localhost:8080 (or your specified port) to access the API on your local IIS server.
Limitations and Potential Improvements
Limitations
API Rate Limits: The external exchange rate API may have rate limits, impacting frequent requests.
Database Dependency: The solution relies on a SQL Server database; in-memory or other database options can be explored for flexibility.
Error Handling: Currently, error handling logs errors and returns a generic message to clients. Custom error handling could improve user feedback.
Potential Improvements
Caching Layer: Implement caching for frequent exchange rate requests to minimize database and external API calls.
Improved API Documentation: Use Swagger/OpenAPI to provide better documentation and testing capabilities.
Retry Mechanism for External API: Add retries with exponential backoff when calls to the external API fail.
Enhanced Security: Implement API key or OAuth for securing access to endpoints, especially in production.
Logging Enhancements: Use a centralized logging service (like Azure Monitor or AWS CloudWatch) for monitoring across environments.
 
