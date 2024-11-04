// using Microsoft.AspNetCore.Mvc;
// using Octokit;
// using System.Net.Http.Headers;



// var builder = WebApplication.CreateBuilder(args);
// var app = builder.Build();



// app.MapGet("/", () => "Hello Copilot!");


// // make sure you change the App Name below
// string yourGitHubAppName = " InsightHub ";
// string githubCopilotCompletionsUrl = 
//     "https://api.githubcopilot.com/chat/completions";



// app.MapPost("/InsightHub", async (
//     [FromHeader(Name = "X-GitHub-Token")] string githubToken, 
//     [FromBody] Request userRequest) =>
// {



// });

// app.Run();

// using Microsoft.AspNetCore.Mvc;
// using Octokit;
// using System.Net.Http.Headers;

// var builder = WebApplication.CreateBuilder(args);
// var app = builder.Build();

// app.MapGet("/", () => "Hello Copilot!");

// // make sure you change the App Name below
// string yourGitHubAppName = "InsightHub";
// string githubCopilotCompletionsUrl = "https://api.githubcopilot.com/chat/completions";

// app.MapPost("/InsightHub", async (
//     [FromHeader(Name = "X-GitHub-Token")] string githubToken, 
//     [FromBody] Request userRequest) =>
// {
//     // Identify the user using the GitHub API token provided in the request headers
//     var octokitClient = new GitHubClient(new Octokit.ProductHeaderValue(yourGitHubAppName))
//     {
//         Credentials = new Credentials(githubToken)
//     };
//     var user = await octokitClient.User.Current();

//     // Insert special system messages
//     userRequest.Messages.Insert(0, new Message
//     {
//         Role = "system",
//         Content = $"Start every response with the user's name, which is @{user.Login}"
//     });
//     userRequest.Messages.Insert(0, new Message
//     {
//         Role = "system",
//         Content = "You are a helpful assistant that replies to user messages as if you were Blackbeard the Pirate."
//     });

//     // Use the HttpClient class to communicate back to Copilot
//     var httpClient = new HttpClient();
//     httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", githubToken);
//     userRequest.Stream = true;

//     // Use Copilot's LLM to generate a response to the user's messages
//     var copilotLLMResponse = await httpClient.PostAsJsonAsync(githubCopilotCompletionsUrl, userRequest);

//     // Stream the response straight back to the user
//     var responseStream = await copilotLLMResponse.Content.ReadAsStreamAsync();
//     return Results.Stream(responseStream, "application/json");
// });

// // Callback endpoint for GitHub app installation
// app.MapGet("/callback", () => "You may close this tab and " + 
//     "return to GitHub.com (where you should refresh the page " +
//     "and start a fresh chat). If you're using VS Code or " +
//     "Visual Studio, return there.");

// app.Run();

// using Microsoft.AspNetCore.Mvc;
// using Octokit;
// using System.Net.Http.Headers;
// using System.Net.WebSockets;
// using System.Text;
// using System.Text.Json;
// using System.Threading;


// var builder = WebApplication.CreateBuilder(args);
// builder.Services.AddControllers();
// var app = builder.Build();

// app.MapGet("/", () => "Hello Copilot!");

// // Define the URLs for WebSocket and GitHub Copilot API
// string awsWebSocketUrl = "wss://i6gi2atm4i.execute-api.us-east-1.amazonaws.com/dev/";
// string githubCopilotCompletionsUrl = "https://api.githubcopilot.com/chat/completions";

// var httpClient = new HttpClient();

// app.MapPost("/InsightHub", async (
//     [FromHeader(Name = "X-GitHub-Token")] string githubToken, 
//     [FromBody] Request userRequest) =>
// {
//     // Get GitHub token from environment variable
//     var token = Environment.GetEnvironmentVariable("GITHUB_TOKEN") ?? githubToken;
//     if (string.IsNullOrEmpty(token))
//     {
//         return Results.Problem("GitHub token is missing.");
//     }

//     // Set the Authorization header with the GitHub token
//     httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
//     userRequest.Stream = true;

//     try
//     {
//         // Send the request to GitHub Copilot's API
//         var copilotLLMResponse = await httpClient.PostAsJsonAsync(githubCopilotCompletionsUrl, userRequest);
//         if (!copilotLLMResponse.IsSuccessStatusCode)
//         {
//             var errorContent = await copilotLLMResponse.Content.ReadAsStringAsync();
//             Console.WriteLine($"Error: {copilotLLMResponse.StatusCode}, Content: {errorContent}");
//             return Results.Problem("Failed to get a response from GitHub Copilot API.");
//         }

//         // Read the Copilot response and send it via WebSocket
//         var responseContent = await copilotLLMResponse.Content.ReadAsStringAsync();
//         using (var client = new ClientWebSocket())
//         {
//             var uri = new Uri(awsWebSocketUrl);
//             await client.ConnectAsync(uri, CancellationToken.None);
//             Console.WriteLine("WebSocket connection established.");

//             // Send the Copilot response message through WebSocket
//             var bytes = Encoding.UTF8.GetBytes(responseContent);
//             await client.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, CancellationToken.None);

//             // Receive a message from WebSocket as an acknowledgment
//             var buffer = new byte[1024];
//             var result = await client.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
//             var receivedMessage = Encoding.UTF8.GetString(buffer, 0, result.Count);
//             Console.WriteLine($"Received from WebSocket: {receivedMessage}");

//             await client.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
//             Console.WriteLine("WebSocket connection closed.");
//         }

//         // Stream the Copilot response back to the user
//         var responseStream = await copilotLLMResponse.Content.ReadAsStreamAsync();
//         return Results.Stream(responseStream, "application/json");
//     }
//     catch (WebSocketException wsEx)
//     {
//         Console.WriteLine($"WebSocket Exception: {wsEx.Message}");
//         return Results.Problem("An error occurred with the WebSocket connection.");
//     }
//     catch (Exception ex)
//     {
//         Console.WriteLine($"Exception: {ex.Message}");
//         return Results.Problem("An unexpected error occurred while processing the request.");
//     }
// });
// app.MapControllers();
// app.Run();

using Microsoft.AspNetCore.Mvc;
using Octokit;
using System.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello Copilot!");

// make sure you change the App Name below
string yourGitHubAppName = "InsightHub";
string githubCopilotCompletionsUrl = "https://api.githubcopilot.com/chat/completions";

app.MapPost("/InsightHub", async (
    [FromHeader(Name = "X-GitHub-Token")] string githubToken, 
    [FromBody] Request userRequest) =>
{
    // Identify the user using the GitHub API token provided in the request headers
    var octokitClient = new GitHubClient(new Octokit.ProductHeaderValue(yourGitHubAppName))
    {
        Credentials = new Credentials(githubToken)
    };
    var user = await octokitClient.User.Current();

    // Insert special system messages
    userRequest.Messages.Insert(0, new Message
    {
        Role = "system",
        Content = $"Start every response with the user's name, which is @{user.Login}"
    });
    userRequest.Messages.Insert(0, new Message
    {
        Role = "system",
        Content = "You are a helpful assistant that replies to user messages as if you were Blackbeard the Pirate."
    });

    // Use the HttpClient class to communicate back to Copilot
    var httpClient = new HttpClient();
    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", githubToken);
    userRequest.Stream = true;

    // Use Copilot's LLM to generate a response to the user's messages
    var copilotLLMResponse = await httpClient.PostAsJsonAsync(githubCopilotCompletionsUrl, userRequest);

    // Stream the response straight back to the user
    var responseStream = await copilotLLMResponse.Content.ReadAsStreamAsync();
    return Results.Stream(responseStream, "application/json");

    // Commented logic for making an API call to AWS via a WebSocket
    /*
    var awsWebSocketUrl = "wss://ms36mojhz3.execute-api.us-east-1.amazonaws.com/dev/";
    using (var webSocket = new ClientWebSocket())
    {
        await webSocket.ConnectAsync(new Uri(awsWebSocketUrl), CancellationToken.None);
        
        // Send a message to the WebSocket
        var message = Encoding.UTF8.GetBytes("Your message here");
        await webSocket.SendAsync(new ArraySegment<byte>(message), WebSocketMessageType.Text, true, CancellationToken.None);

        // Receive a message from the WebSocket
        var buffer = new byte[1024];
        var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
        var responseMessage = Encoding.UTF8.GetString(buffer, 0, result.Count);

        // Process the response message
        Console.WriteLine("Received message from AWS WebSocket: " + responseMessage);
    }
    */
});

// Callback endpoint for GitHub app installation
app.MapGet("/callback", () => "You may close this tab and " + 
    "return to GitHub.com (where you should refresh the page " +
    "and start a fresh chat). If you're using VS Code or " +
    "Visual Studio, return there.");

app.Run();