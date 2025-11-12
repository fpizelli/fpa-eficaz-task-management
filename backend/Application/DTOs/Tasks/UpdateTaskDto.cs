using EficazAPI.Application.DTOs.Tasks;
using System.ComponentModel.DataAnnotations;

namespace EficazAPI.Application.DTOs.Tasks
{
    public class UpdateTaskDto
    {
        [Required(ErrorMessage = "Título é obrigatório")]
        [StringLength(200, ErrorMessage = "Título deve ter no máximo 200 caracteres")]
        public string Title { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Descrição deve ter no máximo 500 caracteres")]
        public string? Description { get; set; }

        [Range(1, 10, ErrorMessage = "Impact deve estar entre 1 e 10")]
        public int Impact { get; set; }

        [Range(1, 10, ErrorMessage = "Effort deve estar entre 1 e 10")]
        public int Effort { get; set; }

        [Range(1, 10, ErrorMessage = "Urgency deve estar entre 1 e 10")]
        public int Urgency { get; set; }
    }
}
