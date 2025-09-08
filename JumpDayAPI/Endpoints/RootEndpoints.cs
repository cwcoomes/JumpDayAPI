namespace JumpDayAPI.Endpoints
{
    public static class RootEndpoints
    {
        public static void AddRootEndpoints(this WebApplication app)
        {
            app.MapGet("/", () => "Hey silly skydiver!"); // IAmTimCorey Video #1
        }
    }
}
