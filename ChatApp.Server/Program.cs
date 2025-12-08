using ChatApp.Hubs;

var builder = WebApplication.CreateBuilder(args);

// SignalR-Dienst hinzufügen
builder.Services.AddSignalR();

// CORS für lokale Entwicklung konfigurieren (damit Maui-App zugreifen kann)
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
       policy.AllowAnyHeader()
             .AllowAnyMethod()
             .AllowCredentials()
             .SetIsOriginAllowed(_ => true); // Erlaube alle Ursprünge 
    });
});

var app = builder.Build();

app.UseHttpsRedirection();
app.UseCors("AllowAll"); // CORS-Middleware hinzufügen und die Richtlinie anwenden

// SignalR-Hub Route konfigurieren
app.MapHub<ChatHub>("/chathub");

app.Run();