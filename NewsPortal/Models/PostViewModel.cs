using SharedModels.Services;

namespace NewsPortal.Models;

public class PostViewModel
{
    public required SharedModels.Services.Post Post { get; set; }
    public required SharedModels.Services.User Author { get; set; }
    public required SharedModels.Services.Comment[] Comments { get; set; }
    public int Likes { get; set; }
    public int Dislikes { get; set; }
}
