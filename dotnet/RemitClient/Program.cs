using Azure.Core;
using Azure.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace RemitClient;

internal class Program
{
    private const string TenantId = "4203b7a0-7773-4de5-b830-8b263a20426e";
    private static readonly string[] ValidEnvironments = { "Test", "Prod" };

    private static async Task Main(string[] args)
    {
        var (environmentName, xmlPath, unrecognizedArgs) = ParseArgs(args);

        if (xmlPath is null || unrecognizedArgs.Count > 0)
        {
            Console.Error.WriteLine("Usage: dotnet run -- [--env=Test|Prod] --xml=<path-to-remit-notification.xml>");
            Environment.Exit(1);
            return;
        }

        if (!ValidEnvironments.Contains(environmentName, StringComparer.OrdinalIgnoreCase))
        {
            Console.Error.WriteLine($"--env must be one of: {string.Join(", ", ValidEnvironments)}. Got '{environmentName}'.");
            Environment.Exit(1);
            return;
        }

        var configuration = new ConfigurationBuilder()
            .AddJsonFile($"appsettings.{environmentName}.json", optional: false)
            .AddEnvironmentVariables()
            .Build();

        var settings = configuration.Get<Settings>()
            ?? throw new InvalidOperationException("Settings could not be loaded.");

        settings.Validate(environmentName);

        var xml = await File.ReadAllTextAsync(xmlPath);

        using var client = new HttpClient();

        var token = await GetTokenAsync(settings);

        var response = await SubmitAsync(client, token, xml, settings.SubmitApi);

        Console.WriteLine($"Status: {(int)response.StatusCode} {response.StatusCode}");
        var responseBody = await response.Content.ReadAsStringAsync();
        Console.WriteLine(PrettifyJson(responseBody));

        if (!response.IsSuccessStatusCode)
        {
            Console.Error.WriteLine($"Submission failed with status {(int)response.StatusCode} {response.StatusCode}.");
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

    private static (string EnvironmentName, string? XmlPath, List<string> UnrecognizedArgs) ParseArgs(string[] args)
    {
        const string EnvPrefix = "--env=";
        const string XmlPrefix = "--xml=";
        var environmentName = "Test";
        string? xmlPath = null;
        var unrecognizedArgs = new List<string>();

        foreach (var arg in args)
        {
            if (arg.StartsWith(EnvPrefix, StringComparison.OrdinalIgnoreCase))
            {
                environmentName = arg[EnvPrefix.Length..];
            }
            else if (arg.StartsWith(XmlPrefix, StringComparison.OrdinalIgnoreCase))
            {
                xmlPath = arg[XmlPrefix.Length..];
            }
            else
            {
                unrecognizedArgs.Add(arg);
            }
        }

        return (environmentName, xmlPath, unrecognizedArgs);
    }
}
