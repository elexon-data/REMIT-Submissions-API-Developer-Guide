using System.Linq;

namespace RemitClient;

public sealed class Settings
{
    public string ClientId { get; set; } = string.Empty;
    public string ClientSecret { get; set; } = string.Empty;
    public string Environment { get; set; } = "Test";
    public string Scope { get; set; } = string.Empty;
    public string SubmitApi { get; set; } = string.Empty;

    private static readonly string[] ValidEnvironments = { "Test", "Prod" };

    public void Validate()
    {
        if (!ValidEnvironments.Contains(Environment, StringComparer.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException(
                $"Environment must be one of: {string.Join(", ", ValidEnvironments)}. Got '{Environment}'.");
        }

        var envFile = $"appsettings.{Environment}.json";

        if (string.IsNullOrWhiteSpace(ClientId))
        {
            throw new InvalidOperationException($"ClientId is required in {envFile}.");
        }

        if (string.IsNullOrWhiteSpace(ClientSecret))
        {
            throw new InvalidOperationException($"ClientSecret is required in {envFile}.");
        }

        if (string.IsNullOrWhiteSpace(Scope))
        {
            throw new InvalidOperationException($"Scope is required in {envFile}.");
        }

        if (string.IsNullOrWhiteSpace(SubmitApi))
        {
            throw new InvalidOperationException($"SubmitApi is required in {envFile}.");
        }
    }
}
