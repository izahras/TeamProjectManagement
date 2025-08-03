# Team Project Management API

A comprehensive .NET Web API for team project management with task tracking, user management, and epic organization.

## Features

### 🎯 Task Management
- Create, read, update, and delete tasks
- Assign tasks to team members
- Track task status (ToDo, InProgress, Review, Done, Cancelled)
- Set task priority levels (Low, Medium, High, Critical)
- Define effort estimates (story points/hours)
- Set due dates and track progress
- Add acceptance criteria and notes

### 👥 User Management
- Complete user CRUD operations
- Role-based user system (Developer, TeamLead, ProjectManager, ProductOwner, ScrumMaster, Tester)
- User activation/deactivation
- Password hashing with BCrypt
- Email and username uniqueness validation

### 📋 Epic Management
- Create and manage large work items (Epics)
- Organize tasks under epics
- Track epic progress and completion
- Set epic priorities and due dates

### 🔧 Additional Features
- RESTful API design
- Entity Framework Core with In-Memory Database
- AutoMapper for clean object mapping
- Swagger/OpenAPI documentation
- CORS support
- Comprehensive error handling
- Sample data seeding

## Architecture

The project follows a clean architecture pattern with the following layers:

```
TeamProjectManagement/
├── Controllers/          # API endpoints
├── Services/            # Business logic
├── Data/               # Data access layer
├── Models/             # Entity models
├── DTOs/               # Data transfer objects
├── Mappings/           # AutoMapper profiles
└── Enums/              # Enumerations
```

## Technology Stack

- **.NET 8.0** - Latest .NET framework
- **ASP.NET Core Web API** - RESTful API framework
- **Entity Framework Core** - ORM for data access
- **AutoMapper** - Object-to-object mapping
- **BCrypt.Net-Next** - Password hashing
- **Swagger/OpenAPI** - API documentation
- **In-Memory Database** - Development database

## Getting Started

### Prerequisites
- .NET 8.0 SDK or later
- Visual Studio 2022 or VS Code

### Installation

1. Clone the repository:
```bash
git clone <repository-url>
cd TeamProjectManagement
```

2. Restore dependencies:
```bash
dotnet restore
```

3. Run the application:
```bash
dotnet run
```

4. Access the API:
- API Base URL: `https://localhost:7001` or `http://localhost:5000`
- Swagger UI: `https://localhost:7001/swagger` or `http://localhost:5000/swagger`

## API Endpoints

### Users
- `GET /api/users` - Get all users
- `GET /api/users/{id}` - Get user by ID
- `POST /api/users` - Create new user
- `PUT /api/users/{id}` - Update user
- `DELETE /api/users/{id}` - Delete user
- `GET /api/users/role/{role}` - Get users by role
- `POST /api/users/{id}/deactivate` - Deactivate user
- `POST /api/users/{id}/activate` - Activate user

### Tasks
- `GET /api/tasks` - Get all tasks
- `GET /api/tasks/{id}` - Get task by ID
- `POST /api/tasks` - Create new task
- `PUT /api/tasks/{id}` - Update task
- `DELETE /api/tasks/{id}` - Delete task
- `GET /api/tasks/status/{status}` - Get tasks by status
- `GET /api/tasks/assignee/{assigneeId}` - Get tasks by assignee
- `GET /api/tasks/epic/{epicId}` - Get tasks by epic
- `POST /api/tasks/{id}/assign/{assigneeId}` - Assign task to user
- `POST /api/tasks/{id}/status/{status}` - Update task status
- `GET /api/tasks/priority/{priority}` - Get tasks by priority

### Epics
- `GET /api/epics` - Get all epics
- `GET /api/epics/{id}` - Get epic by ID
- `POST /api/epics` - Create new epic
- `PUT /api/epics/{id}` - Update epic
- `DELETE /api/epics/{id}` - Delete epic
- `GET /api/epics/status/{status}` - Get epics by status
- `GET /api/epics/creator/{creatorId}` - Get epics by creator
- `GET /api/epics/priority/{priority}` - Get epics by priority

## Data Models


```

## Enums

### TaskStatus
- `1` - ToDo
- `2` - InProgress
- `3` - Review
- `4` - Done
- `5` - Cancelled

### TaskPriority
- `1` - Low
- `2` - Medium
- `3` - High
- `4` - Critical

### UserRole
- `1` - Developer
- `2` - TeamLead
- `3` - ProjectManager
- `4` - ProductOwner
- `5` - ScrumMaster
- `6` - Tester

## AutoMapper Configuration

The project uses AutoMapper for clean object mapping between entities and DTOs. The mapping configuration is defined in `Mappings/AutoMapperProfile.cs`:

```csharp
// User mappings
CreateMap<User, UserDto>();
CreateMap<CreateUserDto, User>();

// Task mappings
CreateMap<WorkItem, TaskDto>();
CreateMap<WorkItem, TaskListDto>();
CreateMap<CreateTaskDto, WorkItem>();

// Epic mappings
CreateMap<Epic, EpicDto>();
CreateMap<Epic, EpicListDto>();
CreateMap<CreateEpicDto, Epic>();
```

## Sample Data

The application comes with pre-seeded sample data including:
- 3 sample users with different roles
- 2 sample epics
- 3 sample tasks with various statuses and assignments

## Development

### Adding New Features
1. Create models in the `Models/` folder
2. Add DTOs in the `DTOs/` folder
3. Create services in the `Services/` folder
4. Add controllers in the `Controllers/` folder
5. Update `ApplicationDbContext` if needed
6. Add AutoMapper mappings in `Mappings/AutoMapperProfile.cs`
7. Register services in `Program.cs`

### Database
Currently using Entity Framework Core with In-Memory Database for development. To switch to SQL Server:

1. Update connection string in `appsettings.json`
2. Change database provider in `Program.cs`:
```csharp
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
```

3. Run migrations:
```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

## Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Add tests if applicable
5. Submit a pull request
