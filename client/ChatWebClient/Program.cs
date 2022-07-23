using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseStaticFiles();

app.UseStaticFiles(new StaticFileOptions()
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "node_modules")),
    RequestPath = new PathString("/vendor")
});


app.UseFileServer(new FileServerOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "Chat")),
    RequestPath = "/Chat",
    EnableDefaultFiles = true
});

app.UseRouter(routeBuilder =>
{
    routeBuilder.MapGet("/", context =>
    {
        context.Response.Redirect("chat/client.html", permanent: false);
        return Task.FromResult(0);
    });
});


app.Run();