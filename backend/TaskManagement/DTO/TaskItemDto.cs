using System.ComponentModel.DataAnnotations;

namespace TaskManagement.DTO
{
    public class TaskItemDto
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "Title length can't be more than 100 characters.")]
        public string Title { get; set; }
        [Required]
        [StringLength(500, ErrorMessage = "Description length can't be more than 500 characters.")]
        public string Description { get; set; }
        public DateTime DueDate { get; set; } = DateTime.MinValue;
    }
}
