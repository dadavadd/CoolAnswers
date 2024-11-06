using CoolAnswers.Data;
using CoolAnswers.Models;
using CoolAnswers.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CoolAnswers.Repositories;

public class QuestionRepository : IQuestionRepository
{
    private readonly ApplicationDbContext _context;

    public QuestionRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task CreateQuestionAsync(Question question)
    {
        await _context.Questions.AddAsync(question);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteQuestionAsync(int questionId)
    {
        var question = await _context.Questions.FindAsync(questionId);
        if (question != null)
        {
            _context.Questions.Remove(question);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<Question>> GetAllQuestionsAsync(string topic = null, int page = 0, int pageSize = 10)
    {

        var query = _context.Questions.AsQueryable();

        if (!string.IsNullOrEmpty(topic))
            query = query.Where(t => t.Topic.Contains(topic));

        var questions = await query
            .Include(q => q.User)
            .OrderByDescending(t => t.Created)
            .Skip(page * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return questions;
    }

    public async Task<Question?> GetQuestionByIdAsync(int questionId)
    {
        return await _context.Questions
            .Include(q => q.User)
            .FirstOrDefaultAsync(q => q.Id == questionId);
    }

    public async Task UpdateQuestionAsync(Question question)
    {
        var existingQuestion = await _context.Questions.FindAsync(question.Id);
        if (existingQuestion != null)
        {
            existingQuestion.Title = question.Title;
            existingQuestion.Content = question.Content;
            existingQuestion.Topic = question.Topic;

            _context.Questions.Update(existingQuestion);
            await _context.SaveChangesAsync();
        }
    }
}
