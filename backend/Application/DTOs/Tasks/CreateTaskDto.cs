using EficazAPI.Application.DTOs.Tasks;
using System.ComponentModel.DataAnnotations;

namespace EficazAPI.Application.DTOs.Tasks
{
    public class CreateTaskDto
    {
        [Required(ErrorMessage = "Título é obrigatório")]
        [StringLength(200, ErrorMessage = "Título não pode exceder 200 caracteres")]
        public string Title { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Descrição não pode exceder 500 caracteres")]
        public string? Description { get; set; }

        [Range(1, 10, ErrorMessage = "Impacto deve ser entre 1 e 10")]
        public int Impact { get; set; } = 5;

        [Range(1, 10, ErrorMessage = "Esforço deve ser entre 1 e 10")]
        public int Effort { get; set; } = 5;

        [Range(1, 10, ErrorMessage = "Urgência deve ser entre 1 e 10")]
        public int Urgency { get; set; } = 5;

        public string? UserId { get; set; }
    }
}
