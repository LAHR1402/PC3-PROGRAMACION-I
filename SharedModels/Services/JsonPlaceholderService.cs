using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace SharedModels.Services;

public class JsonPlaceholderService
{
    private readonly HttpClient _httpClient;

    public JsonPlaceholderService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri("https://jsonplaceholder.typicode.com/");
    }

    public async Task<Post[]> GetPostsAsync()
    {
        var response = await _httpClient.GetAsync("posts");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<Post[]>();
    }

    public async Task<Comment[]> GetCommentsForPostAsync(int postId)
    {
        var response = await _httpClient.GetAsync($"comments?postId={postId}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<Comment[]>();
    }

    public async Task<User> GetUserAsync(int userId)
    {
        var response = await _httpClient.GetAsync($"users/{userId}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<User>();
    }
}

public class Post
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
}

public class Comment
{
    public int Id { get; set; }
    public int PostId { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Body { get; set; }
}

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string Website { get; set; }
}
