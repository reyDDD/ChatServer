using ChatServer.Hubs;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddCors();


var app = builder.Build();

app.UseCors(policy =>
    policy.WithOrigins("https://localhost:7147")
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
