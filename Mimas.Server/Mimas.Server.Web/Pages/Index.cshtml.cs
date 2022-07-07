using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Mimas.Server.Application.Features;

namespace Mimas.Server.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IMediator _mediator;

        public int itemCount;

        public IndexModel(ILogger<IndexModel> logger, IMediator mediator)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task OnGet()
        {
            var items = await _mediator.Send(new GetAllItemsQuery());
            itemCount = items.Items.Count();
        }
    }
}