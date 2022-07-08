using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Mimas.Server.Application.Ports;
using Mimas.Server.Web.ViewModels;

namespace Mimas.Server.Web.Pages
{
    public class DashboardModel : PageModel
    {
        private readonly ILogger<DashboardModel> _logger;
        private readonly IItemRepository _itemRepository;
        private readonly IBoxRepository _boxRepository;
        private readonly ITimeProvider _timeProvider;

        public ProgressChartViewModel? ItemsChart { get; private set; }

        public DashboardModel(ILogger<DashboardModel> logger,
            IItemRepository itemRepository, IBoxRepository boxRepository, ITimeProvider timeProvider)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _itemRepository = itemRepository ?? throw new ArgumentNullException(nameof(itemRepository));
            _boxRepository = boxRepository ?? throw new ArgumentNullException(nameof(boxRepository));
            _timeProvider = timeProvider ?? throw new ArgumentNullException(nameof(timeProvider));
        }

        public async Task OnGet()
        {
            ItemsChart = await GenerateItemsChart();
        }

        private async Task<ProgressChartViewModel> GenerateItemsChart()
        {
            var (dark, danger, warning, success) = (0, 0, 0, 0);

            var (boxTask, itemTask) = (_boxRepository.GetAll(), _itemRepository.GetAll());
            await Task.WhenAll(boxTask, itemTask);
            var (allBoxes, allItems) = ((await boxTask).ToArray(), await itemTask);
            var timeNow = _timeProvider.GetTimeNow();

            foreach (var item in allItems)
            {
                if (ItemValidation.FindErrors(item).Any())
                {
                    ++danger;
                    continue;
                }

                if (item.AssignedBoxShortId is null)
                {
                    ++dark;
                    continue;
                }

                var assignedBox = allBoxes.SingleOrDefault(box => box.ShortId.Equals(item.AssignedBoxShortId));
                if (assignedBox is null)
                {
                    ++danger;
                }
                else
                {
                    var hasBeenDelivered = assignedBox.DeliveredOn != null && assignedBox.DeliveredOn <= timeNow;
                    if (hasBeenDelivered)
                        ++success;
                    else
                        ++warning;
                }
            }

            return new(dark, danger, warning, success);
        }
    }

    internal static class ItemValidation
    {
        public static IReadOnlyCollection<string> FindErrors(ItemViewModel item)
        {
            var result = new List<string>();

            if (item.Count < 1) result.Add($"Invalid item count: {item.Count}");
            if (string.IsNullOrWhiteSpace(item.Name)) result.Add("Empty item name.");

            return result;
        }
    }
}