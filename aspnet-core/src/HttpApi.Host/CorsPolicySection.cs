namespace aspnet_core;


public class CorsPolicySection
{
  public string AllowedHeaders { get; set; } = string.Empty;
  public string AllowedOrigins { get; set; } = string.Empty;
  public string AllowedMethods { get; set; } = string.Empty;
}

