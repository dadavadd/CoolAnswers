namespace CoolAnswers.Models;

public class Question
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public string Topic { get; set; }
    public DateTime Created { get; set; }
    public string UserId { get; set; }
    public virtual User User { get; set; }
    public virtual ICollection<Answer> Answers { get; set; }
}
