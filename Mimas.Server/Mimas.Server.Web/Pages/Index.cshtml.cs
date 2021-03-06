using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Mimas.Server.Application.Features;
using Mimas.Server.Application.Ports;

namespace Mimas.Server.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IMediator _mediator;

        public IReadOnlyCollection<Owner> Owners = Array.Empty<Owner>();
        public IReadOnlyCollection<BoxViewModel> Boxes = Array.Empty<BoxViewModel>();

        [BindProperty]
        public string? ShippingManifestText { get; set; }

        [BindProperty]
        public string? OwnerName { get; set; }
        
        [BindProperty]
        public string? AssignedBoxShortId { get; set; }

        [BindProperty]
        public string? QuickActionsShortId { get; set; }

        public IndexModel(ILogger<IndexModel> logger, IMediator mediator)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task OnGet()
        {
            await InitialisePage();
        }

        public async Task OnPostDelivered()
        {
            if (string.IsNullOrWhiteSpace(QuickActionsShortId))
                throw new ArgumentException("Empty short ID.", nameof(QuickActionsShortId));
            
            var command = new BoxDeliveredCommand(QuickActionsShortId);
            await _mediator.Send(command);
            await InitialisePage();
        }

        public async Task OnPostManifest()
        {
            var items = SplitManifestIntoItemNames();
            var ownerName = OwnerName ?? throw new ArgumentNullException(nameof(OwnerName));

            var command = new RegisterItemBatchCommand(items, ownerName, AssignedBoxShortId);
            await _mediator.Send(command);
            await InitialisePage();
        }

        private IReadOnlyCollection<string> SplitManifestIntoItemNames()
        {
            if (string.IsNullOrWhiteSpace(ShippingManifestText))
                throw new ArgumentException("Empty manifest.", nameof(ShippingManifestText));

            var splitStrategy = StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries;
            var result = new List<string>();
            
            var lines = ShippingManifestText.Split(new[] { '\r', '\n' }, splitStrategy);
            foreach (var line in lines)
            {
                result.AddRange(line.Split(',', splitStrategy));
            }

            return result;
        }

        private async Task InitialisePage()
        {
            var ownerTask = _mediator.Send(new GetAllOwnersQuery());
            var boxTask = _mediator.Send(new GetAllBoxesQuery());
            await Task.WhenAll(ownerTask, boxTask);
            var (owners, boxes) = (await ownerTask, await boxTask);

            Owners = owners.Owners.ToArray();
            Boxes = boxes.Boxes
                .OrderBy(box => box.DeliveredOn is null ? 0 : 1)
                .ThenByDescending(box => box.RegisteredOn)
                .ToArray();
        }
    }
}