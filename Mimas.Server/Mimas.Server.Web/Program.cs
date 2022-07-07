using Mimas.Server.Web;

var app = WebApplication.CreateBuilder(args)
    .SetupApplicationBuilder()
    .Build()
    .SetupApplication();

app.Run();
