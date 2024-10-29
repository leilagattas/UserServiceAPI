# UserServiceAPI

## Summary

UserServiceAPI is a RESTful web service that provides user management functionalities, including user creation, authentication, and role-based access control using JWT (JSON Web Tokens).

## Functionalities

- User creation: under the endpoint POST /api/v1/Users, it allows the creation of a new user with the "Active" status set to true.
- Update user state: under the endpoint PATCH /api/v1/Users/{user_id}, it toggles the "Active" status of an existing user.
- Delete user: under the endpoint DELETE /api/v1/Users/{user_id}, it removes the user from the database.
- List all active users: under the endpoint GET /api/v1/Users/active, it returns a list of all users with the "Active" status set to true.

## Prerequisites

Before running the project, ensure you have the following installed:

- .NET 6 SDK (or higher)
- MySQL Server (version 8.0 or higher)

*Additional Tools*
- A code editor such as Visual Studio
- MySQL Workbench (optional, for database management)

## Clone the repository

`git clone https://github.com/leilagattas/UserServiceAPI.git`
Open the project with Visual Studio, clicking on the `UserServiceAPI.API.sln`

## Database Setup

This project uses MySQL Server as the database management system. Below are the details regarding the setup and connection configuration.
Database Version: This project is configured to work with MySQL Server version 8.0.23 or later.

*Additional Setup*
- Install MySQL: make sure MySQL Server is installed on your machine. You can download it from the official MySQL website.
- Create Database: after installing MySQL, create a database for your project. This database name will be then added to .env file.
- Run Migrations: make sure to apply any migrations to set up your database schema.
`dotnet ef database update --context UserDbContext --startup-project ../UserServiceAPI.API`

## Enviroment Setup

In the root of the API project, you will find a .env.example file, you should copy it and create your own. 

Go to the API project => `cd cd UserServiceAPI.API`
Copy the .env.example file
For Windows => `copy .env.example .env`
For Linux => `cp .env.example .env`

In your .env file, please complete the values with proper ones, according to your settings. 

Example: 
```
# .env
DB_SCHEMA=UserServiceAPI
DB_USER=root
DB_PASSWORD=your_db_password
JWT_KEY=your_jwt_secret_key
```

## Run the project

Once you have done the previous steps, you are ready to go. Please run the UserServiceAPI.API project from the upper button in Visual Studio.
Swagger website should open and you should see 


## Login
Just for testing purposes, two hardcoded users were created so authorization and authentication can be present in this API. 

*Administrator*
You can login as an administrator with the following data:
```
{
  "username": "admin",
  "password": "admin_pass"
}
```
This user can perform all the actions.


*Manager*
You can login as an administrator with the following data:
```
{
  "username": "manager",
  "password": "manager_pass"
}
```
This user can perform the patch and get actions.

*Anonymous User*
If you don't login, you can perform the get action. 

## Authorization

With the previous data, you can execute the POST request in the login, and you will receive a token. 
Please paste that token in the Authorize button on the top right followed by the "Bearer" word. 

For example: `Bearer eyJhbGciOiJIUzI1NiIsInRJ9.eyJodHRwOi8vc2NoZW1hcy54m....0Hx-wfJwQ1k7i0ZWFpLhFxIE2FDm80VZ`

Now you are ready to execute every endpoint. 

