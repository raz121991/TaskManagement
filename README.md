Angular Task Management App

This project is the frontend of a task management application built using Angular 18 in the client and .NET Core 8 in the backend. It provides a user-friendly interface for managing tasks, including features for adding, editing, deleting, and viewing tasks.



## Features

- **Task Management**: Create, edit, delete, and view tasks.
- **Responsive Design**: The application is fully responsive and works on desktops, tablets, and mobile devices.
- **Validation**: Form validation is implemented to ensure data integrity when managing tasks.
- **Integration with .NET Core API**: The frontend communicates with a .NET Core backend via HTTP for task management.
- **Material Design**: The UI is built with Angular Material for a clean and modern look.

## Prerequisites

- **Node.js**: Make sure you have Node.js installed. [Download Node.js](https://nodejs.org/)
- **Angular CLI**: The project requires Angular CLI for development. You can install it globally using npm:

  ```bash
  npm install -g @angular/cli

  NET Core Backend: This frontend project is designed to work with a .NET Core backend. Ensure the backend is set up and running before starting the frontend.

## Backend .NET Core 8
This is the backend for the Task Manager application built using .NET Core. Follow the steps below to set up and run the backend on your local machine.

Clone the Repository:
https://github.com/raz121991/TaskManagement.git
-navigate to the backend folder: cd TaskManagement\backend.

-Restore Dependencies: enter the command in cmd: dotnet restore.

-Configure the Database: go to appsetting.json and make sure to update the Connectionstring under connectionstrings to have your sql server name properly set there.

Apply Database Migrations: in cmd type: dotnet ef database update
Ensure that the Entity Framework Core CLI tools are installed if the command above does not work: dotnet tool install --global dotnet-ef

Run the Application dotnet run


## Angular 18 installation
Navigate to the frontend project folder cd TaskManagement\frontend
Install dependencies: npm install
run the application : ng serve
