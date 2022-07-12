using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Stl.DependencyInjection;
using Templates.TodoApp.UI.BFF;
using Microsoft.AspNetCore.Components.Web;

using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http;

namespace Templates.TodoApp.UI;

public class Program
{
    public static Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);

        builder.RootComponents.Add<App>("#app");
        builder.RootComponents.Add<HeadOutlet>("head::after");

        // authentication state plumbing
        builder.Services.AddAuthorizationCore();
        builder.Services.AddScoped<AuthenticationStateProvider, BffAuthenticationStateProvider>();

        // HTTP client configuration
        builder.Services.AddTransient<AntiforgeryHandler>();

        builder.Services.AddHttpClient("backend", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
            .AddHttpMessageHandler<AntiforgeryHandler>();
        builder.Services.AddTransient(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("backend"));


        StartupHelper.ConfigureServices(builder.Services, builder);
        var host = builder.Build();
        _ = host.Services.HostedServices().Start();
        return host.RunAsync();
    }
}
