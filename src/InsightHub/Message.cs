// public record ChatMessage // Rename to avoid conflict
// {
//     public required string Role { get; set; }
//     public required string Content { get; set; }
// }
public record Message
{
    public required string Role { get; set; }
    public required string Content { get; set; }
}