using Microsoft.AspNetCore.Mvc;
using EficazAPI.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using EficazAPI.Application.UseCases.Tasks;
using EficazAPI.Application.DTOs.Tasks;

namespace EficazAPI.WebApi.Controllers
{
    [ApiController]
    [Route("api/tasks")]
    public class TasksMinimalController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly MoveTaskStatusUseCase _moveTaskStatusUseCase;

        public TasksMinimalController(DataContext context, MoveTaskStatusUseCase moveTaskStatusUseCase)
        {
            _context = context;
            _moveTaskStatusUseCase = moveTaskStatusUseCase;
        }

        [HttpGet("test")]
        public IActionResult Test()
        {
            return Ok(new { message = "API funcionando", timestamp = DateTime.Now });
        }

        [HttpPost("debug")]
        public IActionResult Debug([FromBody] object request)
        {
            return Ok(new { 
                message = "Debug endpoint", 
                receivedData = request?.ToString(),
                type = request?.GetType().Name
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetTasks()
        {
            try
            {
                var tasks = await _context.Tasks
                    .Include(t => t.User)
                    .AsNoTracking()
                    .OrderByDescending(t => t.Score)
                    .ThenByDescending(t => t.CreatedAt)
                    .ToListAsync();

                var tasksDto = tasks.Select(t => new
                {
                    id = t.Id.ToString(),
                    title = t.Title,
                    description = t.Description,
                    status = t.Status.ToString(),
                    priority = new
                    {
                        score = t.Score,
                        impact = t.Impact,
                        effort = t.Effort,
                        urgency = t.Urgency
                    },
                    priorityScore = t.Score,
                    effort = t.Effort,
                    impact = t.Impact,
                    urgency = t.Urgency,
                    userId = t.UserId?.ToString(),
                    userName = t.User?.Name,
                    user = t.User != null ? new
                    {
                        id = t.User.Id.ToString(),
                        name = t.User.Name,
                        email = t.User.Email,
                        role = t.User.Role.ToString()
                    } : null,
                    createdAt = t.CreatedAt.ToString("yyyy-MM-ddTHH:mm:ssZ")
                }).ToList();

                return Ok(tasksDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { 
                    error = ex.Message, 
                    message = "Erro ao buscar tarefas do banco de dados"
                });
            }
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateTaskStatus(Guid id, [FromBody] UpdateTaskStatusRequest request)
        {
            try
            {
                if (!Enum.TryParse<Domain.Entities.TaskStatus>(request.Status, out var newStatus))
                {
                    return BadRequest(new { message = "Status inválido" });
                }

                var dto = new UpdateTaskStatusDto
                {
                    TaskId = id,
                    Status = newStatus
                };

                var updatedTaskDto = await _moveTaskStatusUseCase.ExecuteAsync(id, dto);

                var responseDto = new
                {
                    id = updatedTaskDto.Id.ToString(),
                    title = updatedTaskDto.Title,
                    description = updatedTaskDto.Description,
                    status = updatedTaskDto.Status.ToString(),
                    priority = new
                    {
                        score = updatedTaskDto.PriorityScore,
                        impact = updatedTaskDto.Impact,
                        effort = updatedTaskDto.Effort,
                        urgency = updatedTaskDto.Urgency
                    },
                    priorityScore = updatedTaskDto.PriorityScore,
                    effort = updatedTaskDto.Effort,
                    impact = updatedTaskDto.Impact,
                    urgency = updatedTaskDto.Urgency,
                    userId = (string?)null, 
                    userName = updatedTaskDto.UserName,
                    user = !string.IsNullOrEmpty(updatedTaskDto.UserName) ? new
                    {
                        id = (string?)null, 
                        name = updatedTaskDto.UserName,
                        email = "", 
                        role = "" 
                    } : null,
                    createdAt = updatedTaskDto.CreatedAt.ToString("yyyy-MM-ddTHH:mm:ssZ")
                };

                return Ok(responseDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { 
                    error = ex.Message, 
                    message = "Erro ao atualizar status da tarefa"
                });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(Guid id, [FromBody] JsonElement request)
        {
            try
            {
                var task = await _context.Tasks.FindAsync(id);
                if (task == null)
                {
                    return NotFound(new { message = "Tarefa não encontrada" });
                }

                if (request.TryGetProperty("title", out var titleProp)) 
                    task.Title = titleProp.GetString() ?? task.Title;
                    
                if (request.TryGetProperty("description", out var descProp)) 
                    task.Description = descProp.GetString() ?? task.Description;
                    
                if (request.TryGetProperty("impact", out var impactProp)) 
                    task.Impact = impactProp.GetInt32();
                    
                if (request.TryGetProperty("effort", out var effortProp)) 
                    task.Effort = effortProp.GetInt32();
                    
                if (request.TryGetProperty("urgency", out var urgencyProp)) 
                    task.Urgency = urgencyProp.GetInt32();
                
                if (request.TryGetProperty("userId", out var userIdProp) || request.TryGetProperty("assignee", out userIdProp))
                {
                    var userIdStr = userIdProp.GetString();
                    if (string.IsNullOrEmpty(userIdStr) || userIdStr == "unassigned")
                    {
                        task.UserId = null;
                    }
                    else if (Guid.TryParse(userIdStr, out var userGuid))
                    {
                        task.UserId = userGuid;
                    }
                }
                
                var priority = EficazAPI.Domain.ValueObjects.TaskPriority.Create(task.Impact, task.Effort, task.Urgency);
                task.Score = priority.Score;

                await _context.SaveChangesAsync();

                var updatedTask = await _context.Tasks
                    .Include(t => t.User)
                    .FirstOrDefaultAsync(t => t.Id == id);

                if (updatedTask == null)
                {
                    return StatusCode(500, new { message = "Erro ao recarregar tarefa atualizada" });
                }

                var taskDto = new
                {
                    id = updatedTask.Id.ToString(),
                    title = updatedTask.Title,
                    description = updatedTask.Description,
                    status = updatedTask.Status.ToString(),
                    priority = new
                    {
                        score = updatedTask.Score,
                        impact = updatedTask.Impact,
                        effort = updatedTask.Effort,
                        urgency = updatedTask.Urgency
                    },
                    priorityScore = updatedTask.Score,
                    effort = updatedTask.Effort,
                    impact = updatedTask.Impact,
                    urgency = updatedTask.Urgency,
                    userId = updatedTask.UserId?.ToString(),
                    userName = updatedTask.User?.Name,
                    user = updatedTask.User != null ? new
                    {
                        id = updatedTask.User.Id.ToString(),
                        name = updatedTask.User.Name,
                        email = updatedTask.User.Email,
                        role = updatedTask.User.Role.ToString()
                    } : null,
                    createdAt = updatedTask.CreatedAt.ToString("yyyy-MM-ddTHH:mm:ssZ")
                };

                return Ok(taskDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { 
                    error = ex.Message, 
                    message = "Erro ao atualizar tarefa",
                    stackTrace = ex.StackTrace
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] CreateTaskRequest request)
        {
            try
            {
                Console.WriteLine($"[CREATE] Request recebido - Title: {request.Title}, UserId: {request.UserId}");
                
                if (string.IsNullOrWhiteSpace(request.Title))
                {
                    return BadRequest(new { message = "Título é obrigatório" });
                }

                var priority = EficazAPI.Domain.ValueObjects.TaskPriority.Create(request.Impact, request.Effort, request.Urgency);
                var task = new EficazAPI.Domain.Entities.TaskItem(request.Title, request.Description, priority, request.UserId);
                
                _context.Tasks.Add(task);
                await _context.SaveChangesAsync();

                Console.WriteLine($"[CREATE] Tarefa criada com sucesso: {task.Id}");
                return Ok(new { id = task.Id, message = "Tarefa criada com sucesso" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[CREATE] ERRO: {ex.Message}");
                Console.WriteLine($"[CREATE] Stack: {ex.StackTrace}");
                return StatusCode(500, new { 
                    error = ex.Message, 
                    message = "Erro ao criar tarefa",
                    stackTrace = ex.StackTrace
                });
            }
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                var users = await _context.Users
                    .AsNoTracking()
                    .OrderBy(u => u.Name)
                    .ToListAsync();

                var usersDto = users.Select(u => new
                {
                    id = u.Id.ToString(),
                    name = u.Name,
                    email = u.Email,
                    role = u.Role.ToString(),
                    createdAt = u.CreatedAt.ToString("yyyy-MM-ddTHH:mm:ssZ")
                }).ToList();

                return Ok(usersDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { 
                    error = ex.Message, 
                    message = "Erro ao buscar usuários"
                });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(Guid id)
        {
            try
            {
                var task = await _context.Tasks.FindAsync(id);
                if (task == null)
                {
                    return NotFound(new { message = "Tarefa não encontrada" });
                }

                _context.Tasks.Remove(task);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Tarefa excluída com sucesso" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { 
                    error = ex.Message, 
                    message = "Erro ao excluir tarefa"
                });
            }
        }

        [HttpPost("users")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
        {
            try
            {
                if (!Enum.TryParse<EficazAPI.Domain.Enums.Role>(request.Role, out var role))
                {
                    return BadRequest(new { message = "Role inválido" });
                }

                var passwordService = new EficazAPI.Application.Services.Auth.PasswordService();
                var hashedPassword = passwordService.HashPassword(request.Password);

                var user = new EficazAPI.Domain.Entities.User(request.Name, request.Email, hashedPassword, role);
                
                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return Ok(new { id = user.Id, message = "Usuário criado com sucesso" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { 
                    error = ex.Message, 
                    message = "Erro ao criar usuário"
                });
            }
        }

        [HttpPut("users/{id}")]
        public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UpdateUserRequest request)
        {
            try
            {
                var user = await _context.Users.FindAsync(id);
                if (user == null)
                {
                    return NotFound(new { message = "Usuário não encontrado" });
                }

                if (!Enum.TryParse<EficazAPI.Domain.Enums.Role>(request.Role, out var role))
                {
                    return BadRequest(new { message = "Role inválido" });
                }

                user.Name = request.Name;
                user.Email = request.Email;
                user.Role = role;

                await _context.SaveChangesAsync();

                return Ok(new { message = "Usuário atualizado com sucesso" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { 
                    error = ex.Message, 
                    message = "Erro ao atualizar usuário"
                });
            }
        }

        [HttpDelete("users/{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            try
            {
                var user = await _context.Users.FindAsync(id);
                if (user == null)
                {
                    return NotFound(new { message = "Usuário não encontrado" });
                }

                _context.Users.Remove(user);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Usuário excluído com sucesso" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { 
                    error = ex.Message, 
                    message = "Erro ao excluir usuário"
                });
            }
        }

        [HttpGet("{id}/audit")]
        public async Task<IActionResult> GetTaskAuditLogs(Guid id)
        {
            try
            {
                var auditLogs = await _context.AuditLogs
                    .Include(a => a.User)
                    .Where(a => a.TaskId == id)
                    .AsNoTracking()
                    .OrderByDescending(a => a.CreatedAt)
                    .ToListAsync();

                var auditDto = auditLogs.Select(a => new
                {
                    id = a.Id.ToString(),
                    action = a.Action.Value,
                    oldValue = a.OldValue,
                    newValue = a.NewValue,
                    userId = a.UserId?.ToString(),
                    userName = a.User?.Name,
                    user = a.User != null ? new
                    {
                        id = a.User.Id.ToString(),
                        name = a.User.Name,
                        email = a.User.Email
                    } : null,
                    createdAt = a.CreatedAt.ToString("yyyy-MM-ddTHH:mm:ssZ")
                }).ToList();

                return Ok(auditDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { 
                    error = ex.Message, 
                    message = "Erro ao buscar logs de auditoria"
                });
            }
        }

        [HttpGet("stats")]
        public async Task<IActionResult> GetStats()
        {
            try
            {
                var totalTasks = await _context.Tasks.CountAsync();
                var totalUsers = await _context.Users.CountAsync();
                var totalComments = await _context.Comments.CountAsync();
                var totalAuditLogs = await _context.AuditLogs.CountAsync();

                var todoTasks = await _context.Tasks.CountAsync(t => t.Status == Domain.Entities.TaskStatus.Todo);
                var inProgressTasks = await _context.Tasks.CountAsync(t => t.Status == Domain.Entities.TaskStatus.InProgress);
                var doneTasks = await _context.Tasks.CountAsync(t => t.Status == Domain.Entities.TaskStatus.Done);
                var archivedTasks = await _context.Tasks.CountAsync(t => t.Status == Domain.Entities.TaskStatus.Archived);

                return Ok(new
                {
                    totalTasks,
                    totalUsers,
                    totalComments,
                    totalAuditLogs,
                    tasksByStatus = new
                    {
                        todo = todoTasks,
                        inProgress = inProgressTasks,
                        done = doneTasks,
                        archived = archivedTasks
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { 
                    error = ex.Message, 
                    message = "Erro ao buscar estatísticas"
                });
            }
        }
    }

    public class UpdateTaskStatusRequest
    {
        public string Status { get; set; } = string.Empty;
    }

    public class CreateTaskRequest
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Impact { get; set; }
        public int Effort { get; set; }
        public int Urgency { get; set; }
        public Guid? UserId { get; set; }
    }

    public class CreateUserRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }

    public class UpdateUserRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }

    public class UpdateTaskRequest
    {
        public string title { get; set; } = string.Empty;
        public string? description { get; set; } = string.Empty;
        public int impact { get; set; }
        public int effort { get; set; }
        public int urgency { get; set; }
        public string? userId { get; set; }
    }
}
