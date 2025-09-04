
# Task Management API

A simple backend API that allows management of Tasks and Users to allow allocation of various task status for example: **IN PROGRESS**, **DONE**, amongst others.


## Tech Stack Used

- **.NET 9 Web API**
- **Entity Framework Core** (PostgreSQL in production, InMemory for tests)
- **JWT Authentication**
- **xUnit** for testing
- **Swagger / OpenAPI** for documentation



## Requirements
- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- [PostgreSQL](https://www.postgresql.org/) (if running in dev/prod)
- `appsettings.json` with valid DB connection string & JWT settings


## Installation and Setup


1. Clone the repository:

   ```bash
   - git clone https://github.com/IanOmbija/task-management-system.git
   - cd task-management-system/src/backend

2. Configure your `appsettings.json` in `src/TaskManagementSystem.Api/`

```json
   {
    "ConnectionStrings": {
        "Default": "Host=localhost;Database=tasks_mgmt_db;Username=postgres;Password=yourpassword"
    },
    "Jwt": {
        "Key": "your-256-bit-secret-key-here",
        "Issuer": "TaskManagementSystem",
        "Audience": "TaskManagementSystemUsers"
        }
    }
```
3. Run the database migrations:

   ```bash
    dotnet ef database update --project src/TaskManagementSystem.Api

4. Run the API:

   ```bash
    dotnet run --project src/TaskManagementSystem.Api


## Running Tests

We use xUnit + WebApplicationFactory with InMemory DB.

1. How to run a single test in `tests/TaskManagementSystem.Tests/` directory:

   ```bash
   dotnet test --filter "FullyQualifiedName~TaskControllerTests.GetTasks_FilterByStatus_ShouldReturnOnlyMatching"

2. How to run all the tests in `tests/TaskManagementSystem.Tests/` directory:

   ```bash
   dotnet test
## API Reference and Sample Endpoints

Sample URL: **`http://localhost:5031`**

**User Management**

#### - Register a new User


| Parameter | Type     | Description                |
| :-------- | :------- | :------------------------- |
| `username` | `string` | **Required**. Your username |
| `email` | `string` | **Required**. Your email |
| `password` | `string` | **Required**. Your password |

```http
POST  api/auth/register
```

Sample `request` sent to the endpoint

```http
POST  http://<yourlocalhosturl>/api/auth/register
```
The above endpoint allows registration of a new User;
#### Sample Request Body
```json
{
    "username": "bob3",
    "email": "bob3@example.com",
    "password": "Password2!"
}

```

#### Success Response 200 OK
```json
{
    "username": "bob3",
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9....",
    "role": "USER",
    "expiresAt": "2025-09-04T18:29:55.726126Z",
    "userId": "defb137f-c16c-4770-b85d-682e8afac466",
    "success": true,
    "message": "New User registered sucessfully."
}
```
##
#### - Login an existing User



| Parameter | Type     | Description                |
| :-------- | :------- | :------------------------- |
| `jwt_token` | `string` | **Required**. Your token that was generated on registration |
| `username` | `string` | **Required**. Your username |
| `password` | `string` | **Required**. Your password |

```http
POST  api/auth/login
```

Sample `request` sent to the endpoint

```http
POST  http://<yourlocalhosturl>/api/auth/login
```
The above endpoint allows an existing User to login;
#### Sample Request Body
```json
{
  "email": "admin@test.com",
  "password": "ian123"
}

```

#### Success Response 200 OK
```json
{
    "username": "Ian",
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "role": "USER",
    "expiresAt": "2025-09-04T15:41:27.63981Z",
    "userId": "bca81151-d8ef-40ff-9a24-063485ccfad4",
    "success": true,
    "message": "User successfully logged in."
}
```
##

**Task Management**

#### - Creating a new task



| Parameter | Type     | Description                |
| :-------- | :------- | :------------------------- |
| `tittle` | `string` | **Required**. New task tittle |
| `description` | `string` | **Required**. Task Description |
| `status` | `string` | **Required**. Task Status|
| `taskPriority` | `int` | **Required**. Task priority|
| `assigneeId` | `guid` | **Required**. Task Assignee ID|

```http
POST  api/tasks
```

Sample `request` sent to the endpoint

```http
POST  http://<yourlocalhosturl>/api/tasks
```
The above endpoint allows creation of a new task;
#### Sample Request Body
```json
{
  "tittle": "Test Task",
  "description": "This is a sample task test",
  "status": "IN_PROGRESS",
  "taskPriority": 1,
  "assigneeId": "bca81151-d8ef-40ff-9a24-063485ccfad4"
}

```

#### Success Response: 201 Created
```json
{
    "statusCode": 201,
    "message": "Task created successfully.",
    "payload": {
        "id": "90c62923-aa51-4dba-886b-73c3f3cd00fc",
        "tittle": "Test Task",
        "description": "This is a sample task test",
        "status": "IN_PROGRESS",
        "taskPriority": "MEDIUM",
        "assigneeId": "bca81151-d8ef-40ff-9a24-063485ccfad4",
        "creatorId": "bca81151-d8ef-40ff-9a24-063485ccfad4",
        "createdAt": "2025-08-30T11:24:53.187693Z",
        "updatedAt": "2025-08-30T11:24:53.187693Z"
    }
}
```
###

#### - Fetch/Filter a task based on Status



| Parameter | Type     | Description                |
| :-------- | :------- | :------------------------- |
| `jwt_token` | `string` | **Required**. JWT token for authorization |


```http
GET  api/tasks?status={status}
```

Sample `request` sent to the endpoint

```http
GET  http://<yourlocalhosturl>/api/tasks?status=TODO
```
The above endpoint allows the user to fetch/filter a task based on the status e.g `TODO`;


#### Success Response: 200 OK
```json
{
    "statusCode": 200,
    "message": "Found 6 task(s).",
    "payload": {
        "output": [
            {
                "id": "90c62923-aa51-4dba-886b-73c3f3cd00fc",
                "tittle": "Test Task",
                "description": "Test task_TODO status",
                "status": "TODO",
                "taskPriority": "MEDIUM",
                "assigneeId": "bca81151-d8ef-40ff-9a24-063485ccfad4",
                "creatorId": "bca81151-d8ef-40ff-9a24-063485ccfad4",
                "createdAt": "2025-08-30T11:24:53.187693Z",
                "updatedAt": "2025-08-30T11:24:53.187693Z"
            },
            {
                "id": "e401dc49-ee37-41c7-a14a-1e427eedfc10",
                "tittle": "Test Task Two",
                "description": "Task two with TODO status",
                "status": "TODO",
                "taskPriority": "MEDIUM",
                "assigneeId": "bca81151-d8ef-40ff-9a24-063485ccfad4",
                "creatorId": "bca81151-d8ef-40ff-9a24-063485ccfad4",
                "createdAt": "2025-08-30T11:24:11.169333Z",
                "updatedAt": "2025-08-30T11:24:11.169333Z"
            },
            {
                "id": "202aa092-c6e3-47bf-ad13-ba69a3f138df",
                "tittle": "Test Task Three",
                "description": "Testing to do status",
                "status": "TODO",
                "taskPriority": "MEDIUM",
                "assigneeId": "bca81151-d8ef-40ff-9a24-063485ccfad4",
                "creatorId": "bca81151-d8ef-40ff-9a24-063485ccfad4",
                "createdAt": "2025-08-30T10:29:13.228677Z",
                "updatedAt": "2025-08-30T10:29:13.228677Z"
            },
            {
                "id": "6c68bf1e-a22b-44b7-933d-957dccf3c4ab",
                "tittle": "Test Task Four",
                "description": "Test Creation of todo status",
                "status": "TODO",
                "taskPriority": "MEDIUM",
                "assigneeId": "d90bb16b-5254-4849-b4e4-0414e54fe7d8",
                "creatorId": "bca81151-d8ef-40ff-9a24-063485ccfad4",
                "createdAt": "2025-08-28T00:41:55.717936Z",
                "updatedAt": "2025-08-28T00:41:55.717936Z"
            },
            {
                "id": "994c4260-da97-4da0-b936-19c8c6981678",
                "tittle": "TTest Task Five",
                "description": "We Test creation of Todo",
                "status": "TODO",
                "taskPriority": "HIGH",
                "assigneeId": "bca81151-d8ef-40ff-9a24-063485ccfad4",
                "creatorId": "bca81151-d8ef-40ff-9a24-063485ccfad4",
                "createdAt": "2025-08-28T00:33:31.284764Z",
                "updatedAt": "2025-08-28T00:33:31.284764Z"
            },
            {
                "id": "6c7e955a-d7d6-4f43-9009-6e3a73a68f2c",
                "tittle": "Test Task Six",
                "description": "We Test creation of TODO status",
                "status": "TODO",
                "taskPriority": "MEDIUM",
                "assigneeId": "bca81151-d8ef-40ff-9a24-063485ccfad4",
                "creatorId": "bca81151-d8ef-40ff-9a24-063485ccfad4",
                "createdAt": "2025-08-28T00:33:17.161359Z",
                "updatedAt": "2025-08-28T00:33:17.161359Z"
            }
        ],
        "rowCount": 6
    }
}
```




## Accessing the Swagger - OpenAPI Documentation

Once the application is running, we will use the base URL to access the documentation.

For example, our base url: `http://localhost:5031/swagger/index.html`


## Authors

-  Developed by [@IanOmbija](https://www.github.com/IanOmbija)

