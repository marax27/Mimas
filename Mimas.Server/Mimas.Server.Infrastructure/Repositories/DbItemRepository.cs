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
        await connection.OpenAsync();
        var result = await connection.QueryAsync<ItemViewModel>(SelectAllItems);
        return result;
    }

    public async Task AddMany(IEnumerable<NewItemModel> itemViewModels)
    {
        await using var connection = new SqlConnection(_settings.Value.ConnectionString);
        await connection.OpenAsync();
        foreach (var item in itemViewModels)
        {
            var query = CreateNewItemQuery(item.AssignedBoxShortId);
            await connection.ExecuteAsync(query, item);
        }
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

    private static string CreateNewItemQuery(string? boxShortId)
    {
        var piece = boxShortId is null ? AddNewItemBoxNotAssigned : AddNewItemBoxAssigned;
        return $@"
INSERT INTO Items
    (name, item_count, owner_id, box_id)
VALUES (
    @{nameof(NewItemModel.Name)},
    @{nameof(NewItemModel.ItemCount)},
    (SELECT o.id FROM Owners o WHERE o.name = @{nameof(NewItemModel.OwnerName)}),
    {piece}
)
";
    }

    private const string AddNewItemBoxNotAssigned = "NULL";
    private const string AddNewItemBoxAssigned = $"(SELECT b.id FROM Boxes b WHERE b.short_id = @{nameof(NewItemModel.AssignedBoxShortId)})";
}
