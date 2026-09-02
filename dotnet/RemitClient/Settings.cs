namespace RemitClient;

public sealed class Settings
{
    public string ClientId { get; set; } = string.Empty;
    public string ClientSecret { get; set; } = string.Empty;
    public string XmlFilePath { get; set; } = string.Empty;

    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(ClientId))
        {
            throw new InvalidOperationException("ClientId is required.");
        }

        if (string.IsNullOrWhiteSpace(ClientSecret))
        {
            throw new InvalidOperationException("ClientSecret is required.");
        }

        if (string.IsNullOrWhiteSpace(XmlFilePath))
        {
            throw new InvalidOperationException("XmlFilePath is required.");
        }
    }
}
