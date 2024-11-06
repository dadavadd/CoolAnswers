using CoolAnswers.Models;

namespace CoolAnswers.Repositories.Interfaces;

public interface IComplaintRepository
{
    Task AddComplaintAsync(Complaint complaint);
    Task<IEnumerable<Complaint>> GetAllComplaintsAsync(int page = 0, int pageSize = 10);
    Task<Complaint?> GetComplaintByIdAsync(int complaintId);
    Task DeleteComplaintAsync(int complaintId);
}
