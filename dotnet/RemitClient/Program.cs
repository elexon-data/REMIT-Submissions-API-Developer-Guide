using Azure.Core;
using Azure.Identity;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace RemitClient;

internal class Program
{
    private const string TenantId = "4203b7a0-7773-4de5-b830-8b263a20426e";

    private static async Task Main(string[] args)
    {
        var bootstrapConfiguration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false)
            .AddEnvironmentVariables()
            .Build();

        var environmentName = bootstrapConfiguration["Environment"] ?? "Test";

        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false)
            .AddJsonFile($"appsettings.{environmentName}.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        var settings = configuration.Get<Settings>()
            ?? throw new InvalidOperationException("Settings could not be loaded.");

        settings.Validate();

        if (args.Length != 1)
        {
            Console.Error.WriteLine("Usage: dotnet run -- <path-to-remit-notification.xml>");
            Environment.Exit(1);
            return;
        }

        var xml = await File.ReadAllTextAsync(args[0]);

        using var client = new HttpClient();

        var token = await GetTokenAsync(settings);

        var response = await SubmitAsync(client, token, xml, settings.SubmitApi);

        Console.WriteLine($"Status: {(int)response.StatusCode} {response.StatusCode}");
        var responseBody = await response.Content.ReadAsStringAsync();
        if (!string.IsNullOrWhiteSpace(responseBody))
        {
            Console.WriteLine("Response:");
            Console.WriteLine(PrettifyJson(responseBody));
        }

        if (!response.IsSuccessStatusCode)
        {
            Console.Error.WriteLine($"Submission failed with status {(int)response.StatusCode} {response.StatusCode}.");
            if (!string.IsNullOrWhiteSpace(responseBody))
            {
                Console.Error.WriteLine(PrettifyJson(responseBody));
            }
            Environment.Exit(1);
        }
    }

    private static string PrettifyJson(string json)
    {
        try
        {
            using var document = JsonDocument.Parse(json);
            return JsonSerializer.Serialize(document.RootElement, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (JsonException)
        {
            return json;
        }
    }

    private static async Task<string> GetTokenAsync(Settings settings)
    {
        var credential = new ClientSecretCredential(TenantId, settings.ClientId, settings.ClientSecret);

        var token = await credential.GetTokenAsync(new TokenRequestContext(new[] { settings.Scope }));

        return token.Token;
    }

    private static async Task<HttpResponseMessage> SubmitAsync(HttpClient client, string token, string xml, string submitApi)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, submitApi)
        {
            Content = new StringContent(xml, Encoding.UTF8, "application/xml")
        };
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        return await client.SendAsync(request);
    }
}
