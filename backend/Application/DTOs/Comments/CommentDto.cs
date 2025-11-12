using EficazAPI.Application.DTOs.Comments;
using System.ComponentModel.DataAnnotations;

namespace EficazAPI.Application.DTOs.Comments
{
    public class CommentDto
    {
        public Guid Id { get; set; }
        public Guid TaskId { get; set; }
        public Guid UserId { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public string UserName { get; set; } = string.Empty;
    }

    public class CreateCommentDto
    {
        [Required(ErrorMessage = "TaskId é obrigatório")]
        public Guid TaskId { get; set; }

        [Required(ErrorMessage = "UserId é obrigatório")]
        public Guid UserId { get; set; }

        [Required(ErrorMessage = "Conteúdo é obrigatório")]
        [StringLength(1000, ErrorMessage = "Comentário deve ter no máximo 1000 caracteres")]
        public string Content { get; set; } = string.Empty;
    }
}
