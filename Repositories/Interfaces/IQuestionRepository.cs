using CoolAnswers.Models;

namespace CoolAnswers.Repositories.Interfaces;

public interface IQuestionRepository
{
    Task CreateQuestionAsync(Question question);
    Task UpdateQuestionAsync(Question question);
    Task DeleteQuestionAsync(int questionId);

    Task<Question> GetQuestionByIdAsync(int questionId);

    Task<IEnumerable<Question>> GetAllQuestionsAsync(string topic = null, int page = 0, int pageSize = 10);
}
