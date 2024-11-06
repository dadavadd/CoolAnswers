using System.ComponentModel.DataAnnotations;

namespace CoolAnswers.DTOs.Answer;

public class AnswerCreateDto
{
    [Required]
    public string Content { get; set; }

    [Required]
    public int QuestionId { get; set; }
}
