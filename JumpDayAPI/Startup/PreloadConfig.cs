using JumpDayAPI.Services;
namespace JumpDayAPI.Startup;

public static class PreloadConfig
{
    public static async Task PreloadDataAsync(this WebApplication app)
    {
        using IServiceScope scope = app.Services.CreateScope();
        DropZoneService dropZoneService = scope.ServiceProvider.GetRequiredService<DropZoneService>();
        await dropZoneService.RefreshAsync(); // Preload on startup
    }
}
