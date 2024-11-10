
# VFX challenge - Exchange rate API

## Overview

This application provides an API to fetch and manage currency exchange rates. It supports retrieving rates from a local database and, when not available, querying an external API for the latest rate. This README guides you through setting up the application locally, publishing it to Azure and IIS, and outlines some limitations and potential improvements.

---

## Getting started

### Prerequisites
- **Visual Studio 2022** or later (with .NET Core SDK installed);
- **SQL Server** for database setup (optional for in-memory testing);
- **Azure** (for Azure deployment);
- **IIS** for local or on-premise server deployment.

### Local development in Visual Studio

1. **Clone the repository**:
   ```bash
   git clone https://github.com/IvoCalisto-DigInov/VFX_Challenge.git
   cd VFX_Challenge
   ```

2. **Open the solution**:
   - Launch Visual Studio and open `VFX_Challenge.sln`;

3. **Configure the database**:
   - If using a SQL Server instance:
     - Open `appsettings.json` and configure the connection string under `ConnectionStrings`;
   - If using an in-memory database (for testing only):
     - Update the `Program.cs` file to use an in-memory database instead of SQL Server;

4. **Run the application**:
   - Press `F5` or select **Start Debugging** in Visual Studio to launch the API. It should run at `https://localhost:5001` or a similar local port;

5. **Test the API**:
   - Use tools like **Postman** or a web browser to test the API endpoints, such as:
     ```
     GET https://localhost:5001/api/exchangerate/USD/EUR
     ```

---

## Deployment to Azure

### Publishing the application to Azure App Service

1. **Create an app service**:
   - Log into the [Azure Portal](https://portal.azure.com);
   - Navigate to **App Services** and select **Create**;
   - Set up your application by configuring the resource group, instance name, runtime stack (e.g., .NET Core 8), and region;

2. **Publish from Visual Studio**:
   - In Visual Studio, right-click on the project and select **Publish**;
   - Choose **Azure** as the target and select **Azure App Service (Windows)** or **App Service (Linux)** based on your configuration;
   - Click **Next**, select your app service and click **Finish**;
   - Visual Studio will deploy the application to Azure and you can monitor the process in the Output window;

3. **Configure app settings on Azure**:
   - Go to the Azure App Service resource;
   - Under **Settings**, select **Configuration**;
   - Add or modify settings such as `ConnectionStrings` if you are using an Azure SQL Database or other external resources;

4. **Access the live application**:
   - Once published, your API will be accessible at `https://<your-app-name>.azurewebsites.net`.

---

## Deployment to IIS

### Steps for IIS deployment

1. **Publish the project**:
   - In Visual Studio, right-click on the project and select **Publish**;
   - Choose **Folder** as the target and specify a local folder for the published output;
   - Visual Studio will publish the project files to the specified folder;

2. **Configure IIS**:
   - Open **IIS manager**;
   - Right-click on **Sites** and select **Add website**;
   - Enter the **Site name**, **Physical path** (pointing to your published folder), and **Port** (e.g., 8080);
   - Click **OK** to create the site;

3. **Update application pool settings**:
   - Ensure the application pool is set to use **No managed code** if running as a self-contained .NET app, or set it to **.NET CLR** if required;

4. **Set up permissions**:
   - Give the `IIS_IUSRS` group read and execute permissions for the folder where your application is published;

5. **Access the application**:
   - Visit `http://localhost:8080` (or your specified port) to access the API on your local IIS server.

---

## Limitations and potential improvements

### Limitations:
- **API rate limits**: The external exchange rate API may have rate limits, impacting frequent requests;
- **Database dependency**: The solution relies on a SQL Server Database; in-memory or other database options can be explored for flexibility;
- **Error handling**: Currently, error handling logs errors and returns a generic message to clients. Custom error handling could improve user feedback;

### Potential improvements:
1. **Caching layer**: Implement caching for frequent exchange rate requests to minimize database and external API calls;
2. **Improved API documentation**: Use Swagger/OpenAPI to provide better documentation and testing capabilities;
3. **Retry mechanism for external API**: Add retries with exponential backoff when calls to the external API fail;
4. **Enhanced security**: Implement API key or OAuth for securing access to endpoints, especially in production;
5. **Logging enhancements**: Use a centralized logging service (like Azure Monitor or AWS CloudWatch) for monitoring across environments.

---
