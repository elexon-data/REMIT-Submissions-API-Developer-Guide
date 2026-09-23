namespace RemitClient;

public sealed class Settings
{
    public string TenantId { get; } = "4203b7a0-7773-4de5-b830-8b263a20426e";
    public string ClientId { get; set; } = string.Empty;
    public string ClientSecret { get; set; } = string.Empty;
    public string Scope { get; set; } = string.Empty;
    public string SubmitApi { get; set; } = string.Empty;
    public string InsightsUrl { get; set; } = string.Empty;

    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(ClientId))
        {
            throw new InvalidOperationException("ClientId is required in the settings file.");
        }

        if (string.IsNullOrWhiteSpace(ClientSecret))
        {
            throw new InvalidOperationException("ClientSecret is required in the settings file.");
        }

        if (string.IsNullOrWhiteSpace(Scope))
        {
            throw new InvalidOperationException("Scope is required in the settings file.");
        }

        if (string.IsNullOrWhiteSpace(SubmitApi))
        {
            throw new InvalidOperationException("SubmitApi is required in the settings file.");
        }
    }
}
