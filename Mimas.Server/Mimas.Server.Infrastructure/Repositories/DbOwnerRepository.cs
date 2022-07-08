using Dapper;
using Microsoft.Extensions.Options;
using Mimas.Server.Application.Ports;
using System.Data.SqlClient;

namespace Mimas.Server.Infrastructure.Repositories;

public class DbOwnerRepository : IOwnerRepository
{
    private readonly IOptions<DatabaseConnectionSettings> _settings;

    public DbOwnerRepository(IOptions<DatabaseConnectionSettings> settings)
    {
        _settings = settings ?? throw new ArgumentNullException(nameof(settings));
    }

    public async Task<IEnumerable<Owner>> GetAll()
    {
        await using var connection = new SqlConnection(_settings.Value.ConnectionString);
        await connection.OpenAsync();
        var result = await connection.QueryAsync<Owner>(GetAllOwners);
        return result;
    }

    private const string GetAllOwners = $@"
SELECT
    id   AS {nameof(Owner.Id)},
    name AS {nameof(Owner.Name)}
FROM Owners
";
}
