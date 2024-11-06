namespace CoolAnswers.DTOs.Question
{
    public class QuestionRepsonseDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Topic { get; set; }
        public DateTime Created { get; set; }
        public string AuthorUsername { get; set; }
    }
}
