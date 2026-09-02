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
    private const string Scope = "https://data.dev.elexon.co.uk/account-api-v2/.default";
    private const string SubmitApi = "https://data.dev.elexon.co.uk/account/v2/remit/submit-api";

    private static async Task Main()
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false)
            .AddEnvironmentVariables()
            .Build();

        var settings = configuration.Get<Settings>()
            ?? throw new InvalidOperationException("Settings could not be loaded.");

        settings.Validate();

        var xml = await File.ReadAllTextAsync(settings.XmlFilePath);

        using var client = new HttpClient();

        var token = await GetTokenAsync(settings);

        var response = await SubmitAsync(client, token, xml);

        Console.WriteLine($"Status: {(int)response.StatusCode} {response.StatusCode}");
        var responseBody = await response.Content.ReadAsStringAsync();
        Console.WriteLine("Response:");
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

        var token = await credential.GetTokenAsync(new TokenRequestContext(new[] { Scope }));

        return token.Token;
    }

    private static async Task<HttpResponseMessage> SubmitAsync(HttpClient client, string token, string xml)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, SubmitApi)
        {
            Content = new StringContent(xml, Encoding.UTF8, "application/xml")
        };
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        return await client.SendAsync(request);
    }
}
