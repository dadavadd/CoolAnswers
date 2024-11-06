using System.ComponentModel.DataAnnotations;

namespace CoolAnswers.DTOs.Question
{
    public class QuestionCreateDto
    {
        [Required]
        [StringLength(1000)]
        public string Title { get; set; }

        [Required]
        [StringLength(200)]
        public string Content { get; set; }

        [Required]
        [StringLength(10)]
        public string Topic { get; set; }

    }
}
