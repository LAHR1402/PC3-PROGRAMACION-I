using System;

namespace SharedModels.Models;

public class Feedback
{
    public int Id { get; set; }
    public int PostId { get; set; }
    public string Sentimiento { get; set; } // "like" o "dislike"
    public DateTime Fecha { get; set; }
    public string UserId { get; set; } // ID único por usuario (ejemplo: Guid.NewGuid().ToString())
}
