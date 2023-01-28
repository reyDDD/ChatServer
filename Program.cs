using ChatServer.Hubs;
using TamboliyaLibrary.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddCors();


var app = builder.Build();

var webApiServerPath = builder.Configuration[SolutionPathes.WebApiServer]!;
app.UseCors(policy =>
    policy.WithOrigins("https://localhost:7148", webApiServerPath)
    .AllowAnyMethod()
    .AllowAnyHeader()
    .AllowCredentials()
    );


if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapHub<ChatHub>("/chathub");
app.MapFallbackToPage("/_Host");

app.Run();
