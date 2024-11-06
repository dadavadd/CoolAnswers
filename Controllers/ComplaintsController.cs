using CoolAnswers.DTOs.Complaint;
using CoolAnswers.Models;
using CoolAnswers.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CoolAnswers.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Moderator")]
public class ComplaintsController : ControllerBase
{
    private readonly IComplaintRepository _complaintRepo;

    public ComplaintsController(IComplaintRepository complaintRepo)
    {
        _complaintRepo = complaintRepo;
    }

    [HttpPost("complaint")]
    public async Task<IActionResult> SumbitComplaint([FromBody] ComplaintDto complaintDto)
    {
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { Message = "Пользователь не авторизован." });

            var question = await _complaintRepo.GetComplaintByIdAsync(complaintDto.QuestionId);

            if (question == null)
                return NotFound(new { Message = "Вопрос не найден." });

            var complaint = new Complaint
            {
                Reason = complaintDto.Reason,
                Created = DateTime.UtcNow,
                UserId = userId,
                QuestionId = complaintDto.QuestionId
            };

            await _complaintRepo.AddComplaintAsync(complaint);
            return Ok(new { Message = "Жалоба отправлена модераторам." });
        }
        catch (Exception)
        {
            return StatusCode(500, new { Message = "Произошла ошибка при обработке жалобы." });
        }
    }

    [HttpGet("getComplaints")]
    public async Task<IActionResult> GetComplaints([FromQuery] int page = 0, [FromQuery] int pageSize = 10)
    {
        try
        {
            if (pageSize <= 0 || pageSize > 100)
                return BadRequest(new { Message = "Недопустимый размер страницы." });

            var complaints = await _complaintRepo.GetAllComplaintsAsync(page, pageSize);
            return Ok(complaints);
        }
        catch (Exception)
        {
            return StatusCode(500, new { Message = "Произошла ошибка при получении списка жалоб." });
        }
    }
}