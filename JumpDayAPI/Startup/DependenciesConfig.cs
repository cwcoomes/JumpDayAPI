using JumpDayAPI.Services;

namespace JumpDayAPI.Startup
{
    // IAmTimCorey Video #1
    public static class DependenciesConfig
    {
        // IAmTimCorey Video #1
        public static void AddDependencies(this WebApplicationBuilder builder)
        {
            builder.Services.AddOpenApiServices();

            // Register the HttpClient *named* for DropZoneService
            builder.Services.AddHttpClient<DropZoneService>(client =>
            {
                client.BaseAddress = new Uri("https://www.uspa.org");
            });

            // This line is needed so all injections use the same instance
            builder.Services.AddSingleton<DropZoneService>(sp =>
            {
                var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
                var client = httpClientFactory.CreateClient(nameof(DropZoneService));
                return new DropZoneService(client);
            });

        }
    }
}


