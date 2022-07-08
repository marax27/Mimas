namespace Mimas.Server.Infrastructure;

public class DatabaseConnectionSettings
{
    public string ConnectionString { get; }

    public DatabaseConnectionSettings(string connectionString)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
            throw new ArgumentNullException(nameof(connectionString));
        ConnectionString = connectionString;
    }
}
