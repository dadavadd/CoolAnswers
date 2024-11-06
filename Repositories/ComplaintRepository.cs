using CoolAnswers.Data;
using CoolAnswers.Models;
using CoolAnswers.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CoolAnswers.Repositories;

public class ComplaintRepository : IComplaintRepository
{
    private readonly ApplicationDbContext _context;

    public ComplaintRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddComplaintAsync(Complaint complaint)
    {
        complaint.Created = DateTime.UtcNow;
        await _context.Complaints.AddAsync(complaint);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Complaint>> GetAllComplaintsAsync(int page = 0, int pageSize = 10)
    {
        return await _context.Complaints
            .Include(c => c.User)
            .Include(c => c.Question)
            .OrderByDescending(c => c.Created)
            .Skip(page * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<Complaint?> GetComplaintByIdAsync(int complaintId)
    {
        return await _context.Complaints
            .Include(c => c.User)
            .Include(c => c.Question)
            .FirstOrDefaultAsync(c => c.Id == complaintId);
    }

    public async Task DeleteComplaintAsync(int complaintId)
    {
        var complaint = await _context.Complaints.FindAsync(complaintId);
        if (complaint != null)
        {
            _context.Complaints.Remove(complaint);
            await _context.SaveChangesAsync();
        }
    }
}
