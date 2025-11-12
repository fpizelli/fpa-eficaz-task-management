namespace EficazAPI.Application.Common
{
    public static class Constants
    {
        public static class ErrorMessages
        {
            public const string TaskNotFound = "Tarefa com ID {0} não foi encontrada no sistema.";
            public const string UserNotFound = "Usuário com ID {0} não foi encontrado no sistema.";
            public const string RequiredFieldEmpty = "{0} é obrigatório e não pode estar vazio.";
            public const string ActionCannotBeEmpty = "Ação não pode ser nula ou vazia";
        }

        public static class FieldNames
        {
            public const string TaskTitle = "Título da tarefa";
            public const string CommentContent = "Conteúdo do comentário";
        }

        public static class AuditActions
        {
            public const string TitleChanged = "Título alterado";
            public const string DescriptionChanged = "Descrição alterada";
            public const string ImpactChanged = "Impacto alterado";
            public const string EffortChanged = "Esforço alterado";
            public const string UrgencyChanged = "Urgência alterada";
            public const string PriorityRecalculated = "Prioridade recalculada";
        }

        public static class ValidationThresholds
        {
            public const float SignificantPriorityDifference = 0.01f;
        }

        public static class ValidationLimits
        {
            public const int MinPriorityValue = 1;
            public const int MaxPriorityValue = 10;
            public const int MaxUserNameLength = 100;
            public const int MaxUserEmailLength = 200;
            public const int MaxTaskTitleLength = 200;
            public const int MaxTaskDescriptionLength = 500;
            public const int MaxNotificationTitleLength = 150;
            public const int MaxNotificationMessageLength = 500;
        }

        public static class HttpStatusMessages
        {
            public const string InternalServerError = "Erro interno do servidor";
            public const string BadRequest = "Dados inválidos fornecidos";
            public const string NotFound = "Recurso não encontrado";
            public const string Unauthorized = "Acesso não autorizado";
        }
    }
}
