namespace Core.Config;

public class MongoDbConfig
{
    public string? ConnectionString { get; set; }
    public string? Database { get; set; }
    public string? Collection { get; set; }
    public string? AuthenticationDatabase { get; set; }
    public string? Username { get; set; }
    public string? Password { get; set; }
}