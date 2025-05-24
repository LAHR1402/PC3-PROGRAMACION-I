using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharedModels.Models;
using FeedbackAPI.Models;

namespace FeedbackAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FeedbackController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<FeedbackController> _logger;

    public FeedbackController(ApplicationDbContext context, ILogger<FeedbackController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Feedback>>> GetFeedbacks()
    {
        return await _context.Feedbacks.ToListAsync();
    }

    [HttpGet("count/{postId}")]
    public async Task<ActionResult<FeedbackCount>> GetFeedbackCount(int postId)
    {
        try
        {
            var likes = await _context.Feedbacks
                .CountAsync(f => f.PostId == postId && f.Sentimiento == "like");

            var dislikes = await _context.Feedbacks
                .CountAsync(f => f.PostId == postId && f.Sentimiento == "dislike");

            var result = new FeedbackCount
            {
                PostId = postId,
                Likes = likes,
                Dislikes = dislikes
            };

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener el conteo de feedback");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    [HttpPost]
    public async Task<ActionResult<Feedback>> CreateFeedback(Feedback feedback)
    {
        try
        {
            // Verificar si ya existe un feedback para este usuario y post
            var existingFeedback = await _context.Feedbacks
                .FirstOrDefaultAsync(f => f.PostId == feedback.PostId && f.UserId == feedback.UserId);

            if (existingFeedback != null)
            {
                return BadRequest("Ya has dejado un feedback para este post");
            }

            _context.Feedbacks.Add(feedback);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetFeedbacks), new { id = feedback.Id }, feedback);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear feedback");
            return StatusCode(500, "Error interno del servidor");
        }
    }
}
