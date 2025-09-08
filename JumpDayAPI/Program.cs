using JumpDayAPI.Endpoints;
using JumpDayAPI.Startup;

var builder = WebApplication.CreateBuilder(args);

builder.AddDependencies(); // IAmTimCorey Video #1

var app = builder.Build();

app.UseOpenApi(); // IAmTimCorey Video #1
app.UseHttpsRedirection();

await app.PreloadDataAsync(); // nice and clean

app.AddRootEndpoints(); // IAmTimCorey Video #1
app.AddDropZoneEndpoints();

app.Run();


