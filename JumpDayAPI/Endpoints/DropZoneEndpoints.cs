using JumpDayAPI.Services;
namespace JumpDayAPI.Endpoints;

public static class DropZoneEndpoints
{
    public static void AddDropZoneEndpoints(this WebApplication app)
    {
        app.MapGet("/dropzones", LoadAllDropZones);
        app.MapGet("/dropzones/{id}", LoadDropZoneById);
    }

    private static IResult LoadAllDropZones(DropZoneService service)
    {
        Console.WriteLine($"DropZones count: {service.DropZones.Count}");
        return Results.Ok(service.DropZones);
    }

    private static IResult LoadDropZoneById(DropZoneService service, string id)
    {
        return Results.Ok(service.DropZones.SingleOrDefault(dz => dz.Id == id));
    }
}
