using EficazAPI.Infrastructure.Persistence;
using EficazAPI.Infrastructure.Repositories.Tasks;
using EficazAPI.Infrastructure.Repositories.Comments;
using EficazAPI.Infrastructure.Repositories.Users;
using EficazAPI.Infrastructure.Repositories.AuditLogs;
using EficazAPI.Infrastructure.Extensions;
using EficazAPI.Application.Services.Auth;
using EficazAPI.Application.Services.AuditLogs;
using EficazAPI.Application.Services.Shared;
using EficazAPI.Application.Ports.Tasks;
using EficazAPI.Application.Ports.Comments;
using EficazAPI.Application.Ports.Users;
using EficazAPI.Application.Ports.AuditLogs;
using EficazAPI.Application.Ports.Shared;
using EficazAPI.Application.UseCases.Tasks;
using EficazAPI.Application.UseCases.Comments;
using EficazAPI.Application.UseCases.Users;
using EficazAPI.Application.UseCases.AuditLogs;
using EficazAPI.Domain.Services;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlite("Data Source=app.db"));

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
        options.JsonSerializerOptions.AllowTrailingCommas = true;
        options.JsonSerializerOptions.ReadCommentHandling = JsonCommentHandling.Skip;
        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});



builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddScoped<ITaskQueryRepository, TaskRepository>();
builder.Services.AddScoped<ITaskCommandRepository, TaskRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<ISimpleAuditLogRepository, AuditLogRepositorySimple>();

builder.Services.AddScoped<ITaskDomainService, TaskDomainService>();

builder.Services.AddScoped<ISimpleUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ITaskUnitOfWork, TaskUnitOfWork>();
builder.Services.AddScoped<ICommentUnitOfWork, CommentUnitOfWork>();
builder.Services.AddScoped<ISimpleAuditUnitOfWork, SimpleAuditUnitOfWork>();
builder.Services.AddScoped<IUserUnitOfWork, UserUnitOfWork>();

builder.Services.AddScoped<IValidationService, ValidationService>();

builder.Services.AddScoped<IPasswordService, PasswordService>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IPermissionService, PermissionService>();

builder.Services.AddScoped<CreateTaskUseCase>();
builder.Services.AddScoped<GetTasksUseCase>();
builder.Services.AddScoped<DeleteTaskUseCase>();
builder.Services.AddScoped<MoveTaskStatusUseCase>();
builder.Services.AddScoped<UpdateTaskUseCase>();
builder.Services.AddScoped<RecalculatePrioritiesUseCase>();

builder.Services.AddScoped<GetUsersUseCase>();
builder.Services.AddScoped<CreateUserUseCase>();
builder.Services.AddScoped<DeleteUserUseCase>();

builder.Services.AddScoped<ISimpleAuditLogService, AuditLogServiceSimple>();

builder.Services.AddScoped<AddCommentUseCase>();
builder.Services.AddScoped<GetCommentsByTaskUseCase>();
builder.Services.AddScoped<GetAuditLogsByTaskUseCase>();

var app = builder.Build();

app.ShowHelpIfRequested(args);
await app.ClearDataIfRequestedAsync(args);
await app.RunSeedIfRequestedAsync(args);

using (var scope = app.Services.CreateScope())
{
    try
    {
        var context = scope.ServiceProvider.GetRequiredService<DataContext>();
        
        await context.Database.EnsureCreatedAsync();
        
        Console.WriteLine("Database created successfully!");
        
        await SimpleDbSeeder.SeedData(context);
        Console.WriteLine("Database seeded successfully!");
        
        var userCount = await context.Users.CountAsync();
        var taskCount = await context.Tasks.CountAsync();
        var commentCount = await context.Comments.CountAsync();
        var auditCount = await context.AuditLogs.CountAsync();
        
        Console.WriteLine($"[SEEDER VERIFICATION] Users: {userCount}, Tasks: {taskCount}, Comments: {commentCount}, AuditLogs: {auditCount}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error with database: {ex.Message}");
        Console.WriteLine($"Stack trace: {ex.StackTrace}");
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();
app.MapControllers();

app.Run();
