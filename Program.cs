using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TeamProjectManagement.Data;
using TeamProjectManagement.Models;
using TeamProjectManagement.Enums;
using TeamProjectManagement.Services.Interfaces;
using TeamProjectManagement.Services.Repositories;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer eyJhbGciOiJIUzI1NiIs...\"",
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});



// Add AutoMapper
builder.Services.AddAutoMapper(typeof(Program));

// Add JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(
                builder.Configuration["Jwt:Secret"] ?? "your-super-secret-key-with-at-least-32-characters")),
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"] ?? "TeamProjectManagement",
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"] ?? "TeamProjectManagement",
            ClockSkew = TimeSpan.Zero
        };
    });

// Add Authorization
builder.Services.AddAuthorization();

// Add Entity Framework
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString(
        "DefaultConnection")));

// Add services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddScoped<IEpicService, EpicService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IJwtService, JwtService>();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");

// Add Authentication & Authorization middleware
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Seed the database with sample data
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await SeedDataAsync(context);
}

app.Run();

// Seed data method
async Task SeedDataAsync(ApplicationDbContext context)
{
    if (context.Users.Any())
        return;

    // Create sample users
    var users = new[]
    {
        new User
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            Username = "johndoe",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
            Role = UserRole.ProjectManager,
            CreatedAt = DateTime.UtcNow,
            IsActive = true
        },
        new User
        {
            FirstName = "Jane",
            LastName = "Smith",
            Email = "jane.smith@example.com",
            Username = "janesmith",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
            Role = UserRole.Developer,
            CreatedAt = DateTime.UtcNow,
            IsActive = true
        },
        new User
        {
            FirstName = "Bob",
            LastName = "Johnson",
            Email = "bob.johnson@example.com",
            Username = "bobjohnson",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
            Role = UserRole.TeamLead,
            CreatedAt = DateTime.UtcNow,
            IsActive = true
        }
    };

    context.Users.AddRange(users);
    await context.SaveChangesAsync();

    // Create sample epics
    var epics = new[]
    {
        new Epic
        {
            Title = "User Authentication System",
            Description = "Implement a comprehensive user authentication and authorization system",
            DueDate = DateTime.UtcNow.AddDays(30),
            Priority = TaskPriority.High,
            Status = WorkItemStatus.InProgress,
            CreatedById = users[0].Id,
            CreatedAt = DateTime.UtcNow
        },
        new Epic
        {
            Title = "Dashboard Development",
            Description = "Create a modern dashboard with analytics and reporting features",
            DueDate = DateTime.UtcNow.AddDays(45),
            Priority = TaskPriority.Medium,
            Status = WorkItemStatus.ToDo,
            CreatedById = users[0].Id,
            CreatedAt = DateTime.UtcNow
        }
    };

    context.Epics.AddRange(epics);
    await context.SaveChangesAsync();

    // Create sample tasks
    var tasks = new[]
    {
        new WorkItem
        {
            Title = "Design Login UI",
            Description = "Create a modern and responsive login interface",
            DueDate = DateTime.UtcNow.AddDays(7),
            Priority = TaskPriority.High,
            Effort = 8,
            Status = WorkItemStatus.InProgress,
            CreatedById = users[0].Id,
            AssignedToId = users[1].Id,
            EpicId = epics[0].Id,
            CreatedAt = DateTime.UtcNow,
            StartedAt = DateTime.UtcNow.AddDays(-2)
        },
        new WorkItem
        {
            Title = "Implement JWT Authentication",
            Description = "Set up JWT token-based authentication system",
            DueDate = DateTime.UtcNow.AddDays(14),
            Priority = TaskPriority.High,
            Effort = 16,
            Status = WorkItemStatus.ToDo,
            CreatedById = users[0].Id,
            AssignedToId = users[2].Id,
            EpicId = epics[0].Id,
            CreatedAt = DateTime.UtcNow
        },
        new WorkItem
        {
            Title = "Create Dashboard Layout",
            Description = "Design the main dashboard layout and navigation",
            DueDate = DateTime.UtcNow.AddDays(10),
            Priority = TaskPriority.Medium,
            Effort = 12,
            Status = WorkItemStatus.ToDo,
            CreatedById = users[0].Id,
            AssignedToId = users[1].Id,
            EpicId = epics[1].Id,
            CreatedAt = DateTime.UtcNow
        }
    };

    context.Tasks.AddRange(tasks);
    await context.SaveChangesAsync();
}
