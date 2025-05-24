using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SharedModels.Services;
using SharedModels.Models;
using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using NewsPortal.Models;

namespace NewsPortal.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly JsonPlaceholderService _jsonPlaceholderService;
    private readonly HttpClient _httpClient;

    public HomeController(ILogger<HomeController> logger, JsonPlaceholderService jsonPlaceholderService, IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _jsonPlaceholderService = jsonPlaceholderService;
        _httpClient = httpClientFactory.CreateClient();
    }

    public async Task<IActionResult> Index()
    {
        try
        {
            var posts = await _jsonPlaceholderService.GetPostsAsync();
            return View(posts);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener posts");
            return View("Error");
        }
    }

    public async Task<IActionResult> Post(int id)
    {
        try
        {
            var post = await _jsonPlaceholderService.GetPostsAsync()
                .ContinueWith(t => t.Result.FirstOrDefault(p => p.Id == id));

            if (post == null)
            {
                return NotFound();
            }

            // Obtener comentarios
            var comments = await _jsonPlaceholderService.GetCommentsForPostAsync(id);

            // Obtener autor
            var author = await _jsonPlaceholderService.GetUserAsync(post.UserId);

            // Obtener feedback local
            var feedbackResponse = await _httpClient.GetAsync($"http://localhost:5212/api/feedback?postId={id}");
            var feedbacks = await feedbackResponse.Content.ReadFromJsonAsync<Feedback[]>();

            // Calcular estadísticas de feedback
            var likes = feedbacks?.Count(f => f.Sentimiento == "like") ?? 0;
            var dislikes = feedbacks?.Count(f => f.Sentimiento == "dislike") ?? 0;

            var viewModel = new PostViewModel
            {
                Post = post,
                Author = author,
                Comments = comments,
                Likes = likes,
                Dislikes = dislikes
            };

            return View(viewModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener detalles del post");
            return View("Error");
        }
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
