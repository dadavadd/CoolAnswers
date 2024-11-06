using CoolAnswers.Data;
using CoolAnswers.Models;
using CoolAnswers.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CoolAnswers.Repositories;

public class AnswerRepository : IAnswerRepository
{
    private readonly ApplicationDbContext _context;

    public AnswerRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Answer>> GetAllAnswersAsync(int questionId)
    {
        return await _context.Answers
            .Where(a => a.QuestionId == questionId)
            .Include(a => a.User)
            .ToListAsync();
    }

    public async Task<Answer?> GetAnswerByIdAsync(int id)
    {
        return await _context.Answers
            .Include(a => a.User)
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task AddAnswerAsync(Answer answer)
    {
        await _context.Answers.AddAsync(answer);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAnswerAsync(int id)
    {
        var answer = await _context.Answers.FindAsync(id);
        if (answer != null)
        {
            _context.Answers.Remove(answer);
            await _context.SaveChangesAsync();
        }
    }
}
