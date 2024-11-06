using CoolAnswers.Models;

namespace CoolAnswers.Repositories.Interfaces;

public interface IAnswerRepository
{
    Task<IEnumerable<Answer>> GetAllAnswersAsync(int questionId);
    Task<Answer?> GetAnswerByIdAsync(int id);
    Task AddAnswerAsync(Answer answer);
    Task DeleteAnswerAsync(int id);
}
