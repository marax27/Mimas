using System.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Options;
using Mimas.Server.Application.Ports;

namespace Mimas.Server.Infrastructure.Repositories;

public class DbItemRepository : IItemRepository
{
    private readonly IOptions<DatabaseConnectionSettings> _settings;

    public DbItemRepository(IOptions<DatabaseConnectionSettings> settings)
    {
        _settings = settings ?? throw new ArgumentNullException(nameof(settings));
    }

    public async Task<IEnumerable<ItemViewModel>> GetAll()
    {
        await using var connection = new SqlConnection(_settings.Value.ConnectionString);
        connection.Open();
        var result = await connection.QueryAsync<ItemViewModel>(SelectAllItems);
        return result;
    }

    private const string SelectAllItems = $@"
SELECT
    i.id         AS {nameof(ItemViewModel.Id)},
    i.name       AS {nameof(ItemViewModel.Name)},
    i.item_count AS {nameof(ItemViewModel.Count)},
    o.name       AS {nameof(ItemViewModel.OwnerName)},
    b.short_id   AS {nameof(ItemViewModel.AssignedBoxShortId)}
FROM Items i
INNER JOIN Owners o ON o.id = i.owner_id
INNER JOIN Boxes b ON b.short_id = i.box_id
";
}
