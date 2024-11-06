using CoolAnswers.DTOs.Answer;
using CoolAnswers.Models;
using CoolAnswers.Repositories.Interfaces;
using CoolAnswers.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CoolAnswers.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class AnswersController : ControllerBase
{
    private readonly IAnswerRepository _answerRepo;
    private readonly UserManager<User> _userManager;
    private readonly IRankService _rankService;

    public AnswersController(
        IAnswerRepository answerRepo,
        UserManager<User> userManager,
        IRankService rankService)
    {
        _answerRepo = answerRepo;
        _userManager = userManager;
        _rankService = rankService;
    }

    [HttpGet("{questionId}")]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<Answer>>> GetAnswers(int questionId)
    {
        try
        {
            var answers = await _answerRepo.GetAllAnswersAsync(questionId);
            return Ok(answers);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Ошибка при получении ответов", error = ex.Message });
        }
    }

    [HttpGet("single/{id}")]
    [AllowAnonymous]
    public async Task<ActionResult<Answer>> GetAnswer(int id)
    {
        try
        {
            var answer = await _answerRepo.GetAnswerByIdAsync(id);
            if (answer == null)
            {
                return NotFound(new { message = "Ответ не найден" });
            }
            return Ok(answer);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Ошибка при получении ответа", error = ex.Message });
        }
    }

    [HttpPost]
    public async Task<ActionResult<Answer>> CreateAnswer([FromBody] AnswerCreateDto answerDto)
    {
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { message = "Пользователь не авторизован" });

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound(new { message = "Пользователь не найден" });

            var answer = new Answer
            {
                Content = answerDto.Content,
                Created = DateTime.UtcNow,
                QuestionId = answerDto.QuestionId,
                UserId = userId
            };

            await _answerRepo.AddAnswerAsync(answer);

            user.Point += 10;
            user.Rank = _rankService.GetRank(user.Point);

            await _userManager.UpdateAsync(user);

            return CreatedAtAction(nameof(GetAnswer), new { id = answer.Id }, answer);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Ошибка при создании ответа", error = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAnswer(int id)
    {
        try
        {
            var answer = await _answerRepo.GetAnswerByIdAsync(id);

            if (answer == null)
                return NotFound(new { message = "Ответ не найден" });

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var isModerator = User.IsInRole("Moderator");

            if (answer.UserId != userId && !isModerator)
                return Forbid();

            await _answerRepo.DeleteAnswerAsync(id);

            if (!isModerator && answer.UserId == userId)
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user != null)
                {
                    user.Point -= 10;
                    if (user.Point < 0) user.Point = 0;
                    user.Rank = _rankService.GetRank(user.Point);
                    await _userManager.UpdateAsync(user);
                }
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Ошибка при удалении ответа", error = ex.Message });
        }
    }
}
