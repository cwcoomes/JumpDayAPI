using Scalar.AspNetCore;

namespace JumpDayAPI.Startup
{
    // IAmTimCorey Video #1
    public static class OpenApiConfig
    {
        // IAmTimCorey Video #1
        public static void AddOpenApiServices(this IServiceCollection services)
        {
            services.AddOpenApi();
        }

        // IAmTimCorey Video #1
        public static void UseOpenApi(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.MapScalarApiReference(options =>
                {
                    options.Title = "JumpDay API";
                    options.Theme = ScalarTheme.Saturn;
                    options.Layout = ScalarLayout.Classic;
                    options.HideClientButton = true;
                });

            }
        }
    }
}
