using EficazAPI.Domain.Entities;
using EficazAPI.Domain.Enums;
using EficazAPI.Domain.ValueObjects;
using EficazAPI.Application.Services.Auth;
using Microsoft.EntityFrameworkCore;
using TaskStatus = EficazAPI.Domain.Entities.TaskStatus;

namespace EficazAPI.Infrastructure.Persistence
{
    public static class SimpleDbSeeder
    {
        public static async Task SeedData(DataContext context)
        {
            Console.WriteLine("[SEEDER] Iniciando verificação de seeding...");

            List<User> users;
            if (!context.Users.Any())
            {
                Console.WriteLine("[SEEDER] Criando usuários...");
                users = CreateUsers();
                context.Users.AddRange(users);
                await context.SaveChangesAsync();
                Console.WriteLine($"[SEEDER] {users.Count} usuários criados");
            }
            else
            {
                Console.WriteLine("[SEEDER] Usuários já existem, carregando...");
                users = await context.Users.ToListAsync();
                Console.WriteLine($"[SEEDER] {users.Count} usuários carregados");
            }
            
            List<TaskItem> tasks;
            if (!context.Tasks.Any())
            {
                Console.WriteLine("[SEEDER] Criando tarefas...");
                try
                {
                    tasks = CreateTasks(users);
                    Console.WriteLine($"[SEEDER] Tarefas criadas em memória: {tasks.Count}");
                    
                    context.Tasks.AddRange(tasks);
                    Console.WriteLine("[SEEDER] Tarefas adicionadas ao contexto");
                    
                    await context.SaveChangesAsync();
                    Console.WriteLine($"[SEEDER] {tasks.Count} tarefas salvas no banco");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[SEEDER] ERRO ao criar tarefas: {ex.Message}");
                    Console.WriteLine($"[SEEDER] Stack trace: {ex.StackTrace}");
                    throw;
                }
            }
            else
            {
                Console.WriteLine("[SEEDER] Tarefas já existem, carregando...");
                tasks = await context.Tasks.ToListAsync();
                Console.WriteLine($"[SEEDER] {tasks.Count} tarefas carregadas");
            }
            
            if (!context.Comments.Any())
            {
                Console.WriteLine("[SEEDER] Criando comentários...");
                try
                {
                    var comments = CreateComments(tasks, users);
                    context.Comments.AddRange(comments);
                    await context.SaveChangesAsync();
                    Console.WriteLine($"[SEEDER] {comments.Count} comentários criados");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[SEEDER] ERRO ao criar comentários: {ex.Message}");
                    Console.WriteLine($"[SEEDER] Stack trace: {ex.StackTrace}");
                    throw;
                }
            }
            else
            {
                Console.WriteLine("[SEEDER] Comentários já existem");
            }
            
            if (!context.AuditLogs.Any())
            {
                Console.WriteLine("[SEEDER] Criando logs de auditoria...");
                try
                {
                    var auditLogs = CreateAuditLogs(tasks, users);
                    context.AuditLogs.AddRange(auditLogs);
                    await context.SaveChangesAsync();
                    Console.WriteLine($"[SEEDER] {auditLogs.Count} logs de auditoria criados");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[SEEDER] ERRO ao criar logs de auditoria: {ex.Message}");
                    Console.WriteLine($"[SEEDER] Stack trace: {ex.StackTrace}");
                    throw;
                }
            }
            else
            {
                Console.WriteLine("[SEEDER] Logs de auditoria já existem");
            }

            Console.WriteLine("[SEEDER] Seeding completo finalizado!");
        }

        private static List<User> CreateUsers()
        {
            var passwordService = new PasswordService();
            var defaultPassword = passwordService.HashPassword("senhapadrao");

            return new List<User>
            {
                new User("Carlos Silva", "carlos.admin@eficaz.com", defaultPassword, Role.Admin)
                {
                    Id = Guid.Parse("135732ff-9500-4468-b406-1350ee0c3e81"),
                    CreatedAt = DateTime.UtcNow.AddDays(-30)
                },
                
                new User("Ana Santos", "ana.gerente@eficaz.com", defaultPassword, Role.Gerente)
                {
                    Id = Guid.NewGuid(),
                    CreatedAt = DateTime.UtcNow.AddDays(-25)
                },
                new User("Roberto Lima", "roberto.gerente@eficaz.com", defaultPassword, Role.Gerente)
                {
                    Id = Guid.NewGuid(),
                    CreatedAt = DateTime.UtcNow.AddDays(-20)
                },
                
                new User("Maria Oliveira", "maria.dev@eficaz.com", defaultPassword, Role.Desenvolvedor)
                {
                    Id = Guid.NewGuid(),
                    CreatedAt = DateTime.UtcNow.AddDays(-15)
                },
                new User("João Costa", "joao.dev@eficaz.com", defaultPassword, Role.Desenvolvedor)
                {
                    Id = Guid.NewGuid(),
                    CreatedAt = DateTime.UtcNow.AddDays(-12)
                },
                new User("Pedro Alves", "pedro.dev@eficaz.com", defaultPassword, Role.Desenvolvedor)
                {
                    Id = Guid.NewGuid(),
                    CreatedAt = DateTime.UtcNow.AddDays(-10)
                },
                
                new User("Lucia Ferreira", "lucia.qa@eficaz.com", defaultPassword, Role.QA)
                {
                    Id = Guid.NewGuid(),
                    CreatedAt = DateTime.UtcNow.AddDays(-8)
                },
                new User("Rafael Souza", "rafael.qa@eficaz.com", defaultPassword, Role.QA)
                {
                    Id = Guid.NewGuid(),
                    CreatedAt = DateTime.UtcNow.AddDays(-5)
                }
            };
        }

        private static List<TaskItem> CreateTasks(List<User> users)
        {
            var tasks = new List<TaskItem>();

            tasks.AddRange(new[]
            {
                CreateTask("Implementar sistema de autenticação JWT", "Desenvolver autenticação completa com refresh tokens", 9, 3, 8, users[3].Id, TaskStatus.Todo, -7),
                CreateTask("Criar API de relatórios", "Desenvolver endpoints para geração de relatórios em PDF", 7, 6, 9, users[4].Id, TaskStatus.Todo, -6),
                CreateTask("Implementar cache Redis", "Adicionar cache distribuído para melhorar performance", 8, 8, 4, null, TaskStatus.Todo, -5),
                CreateTask("Configurar CI/CD pipeline", "Setup completo de integração e deploy contínuo", 6, 9, 7, users[1].Id, TaskStatus.Todo, -4)
            });

            tasks.AddRange(new[]
            {
                CreateTask("Refatoração do frontend", "Migrar componentes para TypeScript", 9, 4, 9, users[3].Id, TaskStatus.InProgress, -10),
                CreateTask("Otimização de queries", "Melhorar performance das consultas ao banco", 8, 7, 6, users[4].Id, TaskStatus.InProgress, -8),
                CreateTask("Testes de integração", "Implementar testes end-to-end", 5, 5, 8, users[6].Id, TaskStatus.InProgress, -6)
            });

            tasks.AddRange(new[]
            {
                CreateTask("Setup inicial do projeto", "Configuração base do projeto React + .NET", 9, 2, 9, users[0].Id, TaskStatus.Done, -20),
                CreateTask("Configuração do banco de dados", "Setup SQLite e Entity Framework", 8, 3, 7, users[1].Id, TaskStatus.Done, -18),
                CreateTask("Implementação do Kanban básico", "Quadro Kanban com drag and drop", 7, 5, 8, users[3].Id, TaskStatus.Done, -15),
                CreateTask("Sistema de usuários", "CRUD completo de usuários", 6, 4, 6, users[4].Id, TaskStatus.Done, -12)
            });

            tasks.AddRange(new[]
            {
                CreateTask("Implementar GraphQL", "Substituir REST por GraphQL - cancelado por complexidade", 5, 9, 3, users[4].Id, TaskStatus.Archived, -40),
                CreateTask("Sistema de chat interno", "Chat entre usuários - removido do escopo", 4, 7, 2, users[5].Id, TaskStatus.Archived, -35)
            });

            return tasks;
        }

        private static TaskItem CreateTask(string title, string description, int impact, int effort, int urgency, Guid? userId, TaskStatus status, int daysAgo)
        {
            var priority = TaskPriority.Create(impact, effort, urgency);
            var task = new TaskItem(title, description, priority, userId);
            
            task.Status = status;
            task.CreatedAt = DateTime.UtcNow.AddDays(daysAgo);
            
            return task;
        }

        private static List<Comment> CreateComments(List<TaskItem> tasks, List<User> users)
        {
            var comments = new List<Comment>();
            var random = new Random();

            var commentsData = new List<(TaskItem? task, User user, string content)>();
            
            var authTask = tasks.FirstOrDefault(t => t.Title.Contains("autenticação"));
            if (authTask != null)
            {
                commentsData.Add((authTask, users[0], "Precisamos definir a estratégia de tokens. JWT com refresh token seria ideal."));
                commentsData.Add((authTask, users[3], "Concordo. Vou começar pela implementação do login básico."));
                commentsData.Add((authTask, users[1], "Não esqueçam da validação de roles e permissões."));
            }
            
            var frontendTask = tasks.FirstOrDefault(t => t.Title.Contains("frontend"));
            if (frontendTask != null)
            {
                commentsData.Add((frontendTask, users[1], "A migração para TypeScript está 60% completa. Faltam os componentes de formulário."));
                commentsData.Add((frontendTask, users[3], "Posso ajudar com os formulários. Já tenho experiência com React + TS."));
                commentsData.Add((frontendTask, users[6], "Vamos fazer testes unitários para os novos componentes?"));
            }
            
            var queriesTask = tasks.FirstOrDefault(t => t.Title.Contains("queries"));
            if (queriesTask != null)
            {
                commentsData.Add((queriesTask, users[4], "Identifiquei 3 queries lentas na dashboard. Vou otimizar hoje."));
                commentsData.Add((queriesTask, users[2], "Considere adicionar índices nas colunas mais consultadas."));
            }
            
            var kanbanTask = tasks.FirstOrDefault(t => t.Title.Contains("Kanban"));
            if (kanbanTask != null)
            {
                commentsData.Add((kanbanTask, users[0], "Excelente trabalho! O drag and drop está funcionando perfeitamente."));
                commentsData.Add((kanbanTask, users[7], "Testei em mobile e está responsivo. Aprovado!"));
            }
            
            var reportsTask = tasks.FirstOrDefault(t => t.Title.Contains("relatórios"));
            if (reportsTask != null)
            {
                commentsData.Add((reportsTask, users[2], "Quais tipos de relatório são prioridade? Vendas, usuários ou performance?"));
                commentsData.Add((reportsTask, users[4], "Comece com relatórios de vendas. É o que o cliente mais solicita."));
            }
            
            var testTask = tasks.FirstOrDefault(t => t.Title.Contains("Testes"));
            if (testTask != null)
            {
                commentsData.Add((testTask, users[6], "Configurei o Cypress. Agora vou criar os primeiros testes E2E."));
                commentsData.Add((testTask, users[7], "Ótimo! Vou ajudar com os testes de API usando Postman/Newman."));
            }
            
            var cacheTask = tasks.FirstOrDefault(t => t.Title.Contains("cache"));
            if (cacheTask != null)
            {
                commentsData.Add((cacheTask, users[2], "Redis vai melhorar muito a performance. Vou configurar clustering também."));
                commentsData.Add((cacheTask, users[0], "Boa! Não esqueça de implementar cache invalidation."));
            }
            
            var cicdTask = tasks.FirstOrDefault(t => t.Title.Contains("CI/CD"));
            if (cicdTask != null)
            {
                commentsData.Add((cicdTask, users[1], "Configurando GitHub Actions. Deploy automático para staging funcionando."));
                commentsData.Add((cicdTask, users[2], "Adicione testes automatizados no pipeline antes do deploy."));
            }
            
            var userTask = tasks.FirstOrDefault(t => t.Title.Contains("usuários"));
            if (userTask != null)
            {
                commentsData.Add((userTask, users[4], "CRUD finalizado. Inclui validação de email único e hash de senha."));
                commentsData.Add((userTask, users[0], "Perfeito! Agora podemos implementar os diferentes níveis de acesso."));
            }
            
            var graphqlTask = tasks.FirstOrDefault(t => t.Title.Contains("GraphQL"));
            if (graphqlTask != null)
            {
                commentsData.Add((graphqlTask, users[0], "Projeto cancelado. REST API atende nossas necessidades atuais."));
                commentsData.Add((graphqlTask, users[4], "Concordo. GraphQL seria over-engineering para nosso caso."));
            }
            
            var chatTask = tasks.FirstOrDefault(t => t.Title.Contains("chat"));
            if (chatTask != null)
            {
                commentsData.Add((chatTask, users[1], "Funcionalidade removida do escopo. Foco no core do produto."));
                commentsData.Add((chatTask, users[5], "Podemos reavaliar em versões futuras se houver demanda."));
            }

            foreach (var (task, user, content) in commentsData)
            {
                if (task != null)
                {
                    var comment = new Comment(task.Id, user.Id, content);
                    comment.CreatedAt = DateTime.UtcNow.AddDays(-random.Next(1, 15));
                    comments.Add(comment);
                }
            }

            return comments;
        }

        private static List<AuditLog> CreateAuditLogs(List<TaskItem> tasks, List<User> users)
        {
            var auditLogs = new List<AuditLog>();
            var random = new Random();

            foreach (var task in tasks)
            {
                auditLogs.Add(new AuditLog(
                    task.Id,
                    AuditAction.TaskCreated,
                    null,
                    $"'{task.Title}' criada",
                    task.UserId)
                {
                    CreatedAt = task.CreatedAt
                });
            }

            foreach (var task in tasks.Where(t => t.Status != TaskStatus.Todo))
            {
                auditLogs.Add(new AuditLog(
                    task.Id,
                    AuditAction.TaskStatusChanged,
                    "Todo",
                    task.Status.ToString(),
                    task.UserId)
                {
                    CreatedAt = task.CreatedAt.AddHours(random.Next(1, 48))
                });
            }

            var unassignedTasks = tasks.Where(t => !t.UserId.HasValue).Take(2).ToList();
            foreach (var task in unassignedTasks)
            {
                var assignedUser = users[random.Next(1, users.Count)]; 
                auditLogs.Add(new AuditLog(
                    task.Id,
                    AuditAction.TaskAssigned,
                    null,
                    assignedUser.Name,
                    users[0].Id) 
                {
                    CreatedAt = task.CreatedAt.AddMinutes(random.Next(30, 120))
                });
            }

            return auditLogs;
        }
    }
}
