using EficazAPI.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EficazAPI.Infrastructure.Extensions
{
    public static class SeedExtensions
    {
        public static async Task<WebApplication> RunSeedIfRequestedAsync(this WebApplication app, string[] args)
        {
            if (args.Contains("--seed") || args.Contains("-s"))
            {
                Console.WriteLine(" Executando seed via linha de comando...");
                
                using var scope = app.Services.CreateScope();
                try
                {
                    var context = scope.ServiceProvider.GetRequiredService<DataContext>();
                    await SimpleDbSeeder.SeedData(context);
                    
                    var userCount = await context.Users.CountAsync();
                    var taskCount = await context.Tasks.CountAsync();
                    
                    int commentCount = 0;
                    int auditCount = 0;
                    try
                    {
                        commentCount = await context.Comments.CountAsync();
                        auditCount = await context.AuditLogs.CountAsync();
                    }
                    catch
                    {
                    }
                    
                    Console.WriteLine(" [CLI] Seed executado com sucesso!");
                    Console.WriteLine(" [CLI] Estatísticas:");
                    Console.WriteLine("   Usuários: {0}", userCount);
                    Console.WriteLine("   Tarefas: {0}", taskCount);
                    Console.WriteLine("   Comentários: {0}", commentCount);
                    Console.WriteLine("   Logs de Auditoria: {0}", auditCount);
                    Console.WriteLine();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(" [CLI] Erro ao executar seed: {0}", ex.Message);
                    Environment.Exit(1);
                }
            }
            
            return app;
        }

        public static async Task<WebApplication> ClearDataIfRequestedAsync(this WebApplication app, string[] args)
        {
            if (args.Contains("--clear") || args.Contains("-c"))
            {
                var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                if (environment != "Development")
                {
                    Console.WriteLine(" [CLI] Limpeza só pode ser executada em ambiente de desenvolvimento");
                    Environment.Exit(1);
                }

                Console.WriteLine(" [CLI] Limpando dados via linha de comando...");
                
                using var scope = app.Services.CreateScope();
                try
                {
                    var context = scope.ServiceProvider.GetRequiredService<DataContext>();
                    
                    context.AuditLogs.RemoveRange(context.AuditLogs);
                    context.Comments.RemoveRange(context.Comments);
                    context.Tasks.RemoveRange(context.Tasks);
                    context.Users.RemoveRange(context.Users);
                    
                    await context.SaveChangesAsync();
                    
                    Console.WriteLine(" [CLI] Dados limpos com sucesso!");
                    Console.WriteLine();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(" [CLI] Erro ao limpar dados: {0}", ex.Message);
                    Environment.Exit(1);
                }
            }
            
            return app;
        }

        public static WebApplication ShowHelpIfRequested(this WebApplication app, string[] args)
        {
            if (args.Contains("--help") || args.Contains("-h"))
            {
                Console.WriteLine("Comandos CLI Disponíveis:");
                Console.WriteLine();
                Console.WriteLine("  --seed, -s     Executa o seed de dados de teste");
                Console.WriteLine("  --clear, -c    Limpa todos os dados (apenas em Development)");
                Console.WriteLine("  --help, -h     Mostra esta ajuda");
                Console.WriteLine();
                Console.WriteLine("Exemplos:");
                Console.WriteLine("  dotnet run --seed");
                Console.WriteLine("  dotnet run --clear");
                Console.WriteLine("  dotnet run --clear --seed");
                Console.WriteLine();
                Console.WriteLine("Endpoints da API:");
                Console.WriteLine("  POST /api/seed/run     - Executa seed via HTTP");
                Console.WriteLine("  GET  /api/seed/stats   - Mostra estatísticas do banco");
                Console.WriteLine("  DELETE /api/seed/clear - Limpa dados via HTTP");
                Console.WriteLine();
                
                Environment.Exit(0);
            }
            
            return app;
        }
    }
}
