using CoolAnswers.DTOs.Question;
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
public class QuestionsController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly IQuestionRepository _questionRepo;
    private readonly IRankService _rankService;

    public QuestionsController(
        IQuestionRepository questionRepo,
        UserManager<User> userManager,
        IRankService rankService)
    {
        _questionRepo = questionRepo;
        _userManager = userManager;
        _rankService = rankService;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateQuestion([FromBody] QuestionCreateDto questionDto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(new { Message = "Неверные данные вопроса", Errors = ModelState });

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { Message = "Пользователь не авторизован." });

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return NotFound(new { Message = "Пользователь не найден." });

            var question = new Question
            {
                Title = questionDto.Title,
                Content = questionDto.Content,
                Topic = questionDto.Topic,
                Created = DateTime.UtcNow,
                UserId = userId
            };

            await _questionRepo.CreateQuestionAsync(question);

            user.Point += 5;
            user.Rank = _rankService.GetRank(user.Point);

            var updateResult = await _userManager.UpdateAsync(user);

            return Ok(new { Message = "Вопрос успешно создан", Question = question });
        }
        catch (Exception)
        {
            return StatusCode(500, new { Message = "Произошла ошибка при создании вопроса." });
        }
    }

    [HttpGet("getQuestions")]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<QuestionRepsonseDto>>> GetQuestions(
        [FromQuery] string? topic = null,
        [FromQuery] int page = 0,
        [FromQuery] int pageSize = 10)
    {
        if (page < 0)
            return BadRequest(new { Message = "Номер страницы не может быть отрицательным" });

        if (pageSize <= 0 || pageSize > 100)
            return BadRequest(new { Message = "Размер страницы должен быть от 1 до 100" });

        if (!string.IsNullOrEmpty(topic) && topic.Length > 100)
            return BadRequest(new { Message = "Тема не может быть длиннее 100 символов" });

        try
        { 
            var questions = await _questionRepo.GetAllQuestionsAsync(topic, page, pageSize);

            var questionsDto = questions.Select(q => new QuestionRepsonseDto()
            {
                Id = q.Id,
                AuthorUsername = q.User.UserName,
                Title = q.Title,
                Created = q.Created,
                Topic = q.Topic,
            });


            return Ok(questionsDto);
        }
        catch (Exception)
        {
            return StatusCode(500, new { Message = "Произошла ошибка при получении списка вопросов." });
        }
    }
}
