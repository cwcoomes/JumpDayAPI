using JumpDayAPI.Endpoints;
using JumpDayAPI.Startup;

var builder = WebApplication.CreateBuilder(args);

builder.AddDependencies(); 

var app = builder.Build();

app.UseOpenApi();
app.UseHttpsRedirection();

await app.PreloadDataAsync(); // nice and clean

app.AddRootEndpoints(); 
app.AddDropZoneEndpoints();
app.AddJumpScoreEndpoints();

app.Run();


