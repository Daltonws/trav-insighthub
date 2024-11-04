// public record Request
// {
//     public string RequestId { get; set; } = string.Empty;
//     public string ModelDeploymentId { get; set; } = string.Empty;
//     public bool Stream { get; set; }
//     public List<Message> Messages { get; set; } = new List<Message>();
//     public required string Message { get; set; }
    
//     public required string User { get; set; }
// }

// public record Message
// {
//     public required string Role { get; set; }
//     public required string Content { get; set; }
// }
public record Request
{
    public bool Stream { get; set; }
    public List<Message> Messages { get; set; } = [];
}