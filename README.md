# LSCS
2015 SENG 422 project.

The Land Surveyor Checklist System implemented is a Visual C# 2013 'Solution' that was developed in Microsoft Visual Studio Ultimate 2013. Its structure is dictated primarily by its adherance to the ASP.NET MVC 5 framework. There are four main 'projects', and one 'Tests' folder than holds four corresponding test projects. The projects correspond to the system topology outlined in section 2.0 of our architectural design report.

In order for the project to function within a development environment, both the LSCS.API and LSCS.Web projects must be operational and running. LSCS.Web provides the browser with the necessary view template, as dictated by the controllers, and the ReactJS components poll the LSCS.API for whatever data is needed on the page.

All subsystems are functional and capable of dynamic communication and operations. Users can register new accounts, login, logout, assign managerial or surveyor roles, create checklists, edit checklists, remove checklists, and view checklists with live map and weather feeds for the relevant survey coordinates.

We were unable to meet our goals for API security or full support for account management.


