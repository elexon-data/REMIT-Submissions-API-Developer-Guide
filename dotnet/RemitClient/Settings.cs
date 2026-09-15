namespace RemitClient;

public sealed class Settings
{
    public string ClientId { get; set; } = string.Empty;
    public string ClientSecret { get; set; } = string.Empty;
    public string Scope { get; set; } = string.Empty;
    public string SubmitApi { get; set; } = string.Empty;

    public void Validate(string environment)
    {
        var envFile = $"appsettings.{environment}.json";

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
