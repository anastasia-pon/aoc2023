namespace aoc2023.Services;
public interface IInputService
{
    Task<string?> GetInput(int day);
}

public class InputService : IInputService
{
    public InputService()
    {
    }
    
    private static readonly HttpClient Client = new();

    public async Task<string?> GetInput(int day)
    {
        var sessionCookie = Environment.GetEnvironmentVariable("SESSION");
        var message = new HttpRequestMessage(HttpMethod.Get, $"https://adventofcode.com/2023/day/{day}/input");
        message.Headers.Add("Cookie", $"session={sessionCookie};");
        var response = await Client.SendAsync(message);
        return response.IsSuccessStatusCode ? response.Content.ReadAsStringAsync().Result : null;
    }
}