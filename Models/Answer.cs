namespace CoolAnswers.Models;

public class Answer
{
    public int Id { get; set; }
    public string Content { get; set; }
    public DateTime Created { get; set; }
    public string UserId { get; set; }
    public int QuestionId { get; set; }
    public virtual User User { get; set; }
    public virtual Question Question { get; set; }
}
