# LSCS
####2015 SENG 422 project.

The Land Surveyor Checklist System implemented is a Visual C# 2013 'Solution' that was developed in Microsoft Visual Studio Ultimate 2013. Its structure is dictated primarily by its adherance to the ASP.NET MVC 5 framework. There are four main 'projects', and one 'Tests' folder than holds four corresponding test projects. The projects correspond to the system topology outlined in section 2.0 of our architectural design report.

In order for the project to function within a development environment, both the LSCS.API and LSCS.Web projects must be operational and running. LSCS.Web provides the browser with the necessary view template, as dictated by the controllers, and the ReactJS components poll the LSCS.API for whatever data is needed on the page.

All subsystems are functional and capable of dynamic communication and operations. Users can register new accounts, login, logout, assign managerial or surveyor roles, create checklists, edit checklists, remove checklists, and view checklists with live map and weather feeds for the relevant survey coordinates.

We were unable to meet our goals for API security or full support for account management.

# Components

As stated above there are 4 main projects within the LSCS solution:
- LSCS.Api
- LSCS.Web
- LSCS.Repository
- LSCS.Models

Each of these components is discussed in more detail below.

## LSCS.Api

As the name indicates, this project contains the API. The API is responsible for handling all data transfers with regard to checklists. It provides endpoints to:
- get all checklists in the system
- get a single checklist specified by an ID
- create a checklist
- update a checklist
- delete a checklist

## LSCS.Web

The Web project contains the main web app. This project is where all the UI for the LSCS system is built. As discussed in our report, much of the front end uses React.JS to dynamically load data into the page as it is modified. The React components can be found in the `scripts/templates` directory in two `.jsx` files.

The Web project also manages user accounts. It is supported by a SQL Server database instead of MongoDB, like the API. It provides the mechanisms to create new accounts, log in, log out, etc.

## LSCS.Models

The models project contains all the data models that are used by both the API and the Web project to transfer data.

## LSCS.Repository

The repository project contains code for configuring and handling communication with the database.
