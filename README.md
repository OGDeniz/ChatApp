# ChatApp - Projektdokumentation

## Übersicht

Eine vollständige WhatsApp-ähnliche Chat-Anwendung für Android, entwickelt mit .NET MAUI und C#. Die App unterstützt Echtzeit-Kommunikation über SignalR, Light/Dark Mode und bietet eine moderne, benutzerfreundliche Oberfläche.

---

## Inhaltsverzeichnis

1. [Technologie-Stack](#technologie-stack)
2. [Projektstruktur](#projektstruktur)
3. [Features](#features)
4. [Architektur](#architektur)
5. [Setup & Installation](#setup--installation)
6. [Implementierte Komponenten](#implementierte-komponenten)
7. [Deployment](#deployment)
8. [Zukünftige Erweiterungen](#zukünftige-erweiterungen)

---

## Technologie-Stack

### Frontend (Client)
- **.NET MAUI** (Multi-platform App UI) - Cross-platform Framework
- **C#** - Programmiersprache
- **XAML** - UI-Markup
- **CommunityToolkit.Mvvm** - MVVM Hilfsframework
- **Microsoft.AspNetCore.SignalR.Client** - Echtzeit-Kommunikation Client

### Backend (Server)
- **ASP.NET Core 8.0** - Web-Framework
- **SignalR** - WebSocket-basierte Echtzeit-Kommunikation
- **CORS** - Cross-Origin Resource Sharing für lokale Entwicklung

### Design Pattern
- **MVVM** (Model-View-ViewModel) - Trennung von UI und Logik
- **Dependency Injection** - Loose Coupling
- **Observable Pattern** - Automatische UI-Updates

---

## Projektstruktur

```
ChatApp/
├── ChatApp (MAUI Client)
│   ├── Models/
│   │   └── ChatMessage.cs              # Nachrichtenmodell
│   ├── PageModels/
│   │   ├── ChatPageModel.cs            # Chat-Logik
│   │   └── LoginPageModel.cs           # Login-Logik
│   ├── Pages/
│   │   ├── ChatPage.xaml               # Chat UI
│   │   ├── ChatPage.xaml.cs            # Chat Code-Behind
│   │   ├── LoginPage.xaml              # Login UI
│   │   └── LoginPage.xaml.cs           # Login Code-Behind
│   ├── Services/
│   │   └── ChatService.cs              # SignalR Client Service
│   ├── Converters/
│   │   ├── InverseBoolConverter.cs     # Bool Inverter
│   │   ├── MessageBackgroundConverter.cs
│   │   └── BoolToAlignmentConverter.cs
│   ├── App.xaml                        # App-Ressourcen & Theming
│   ├── App.xaml.cs                     # App Entry Point
│   ├── AppShell.xaml                   # Navigation Shell
│   └── MauiProgram.cs                  # DI Konfiguration
│
└── ChatApp.Server (ASP.NET Backend)
    ├── Hubs/
    │   └── ChatHub.cs                  # SignalR Hub
    └── Program.cs                      # Server-Konfiguration
```

---

## Features

### ✅ Implementiert

#### 1. **Echtzeit-Chat**
- WebSocket-basierte Kommunikation via SignalR
- Automatische Nachrichtensynchronisation zwischen allen Clients
- Persistente Verbindung mit Auto-Reconnect

#### 2. **WhatsApp-ähnliche UI**
- Sprechblasen-Design
  - Eigene Nachrichten: rechts, grün (#DCF8C6)
  - Fremde Nachrichten: links, weiß
- Abgerundete Ecken (Border Radius)
- Sender-Name bei fremden Nachrichten
- Zeitstempel (HH:mm Format)
- Status-Häkchen System:
  - Leer: Gesendet
  - ✓: Zugestellt
  - ✓✓: Gelesen

#### 3. **Login-System**
- Name-basierte Authentifizierung
- Persistente Speicherung mit Preferences
- Automatische Weiterleitung zum Chat bei bestehendem Login

#### 4. **Theming (Light/Dark Mode)**
- Automatische Erkennung des System-Themes
- Angepasste Farbschemata:
  - **Light Mode**: Beiger Hintergrund (#ECE5DD)
  - **Dark Mode**: Dunkler Hintergrund (#0B141A)
- Theme-abhängige Farben für:
  - Hintergründe
  - Sprechblasen
  - Text
  - Eingabefelder

#### 5. **MVVM Architecture**
- Saubere Trennung von UI und Logik
- Data Binding mit ObservableProperties
- Commands für User-Interaktionen
- Wiederverwendbare ViewModels

#### 6. **Auto-Scroll**
- Automatisches Scrollen zu neuen Nachrichten
- Smooth Animation

---

## Architektur

### MVVM Pattern

```
┌─────────────┐         ┌──────────────┐         ┌─────────┐
│    View     │ Binding │  ViewModel   │  Logic  │  Model  │
│   (XAML)    │◄───────►│   (C#)       │────────►│  (C#)   │
└─────────────┘         └──────────────┘         └─────────┘
                              │
                              │ Commands
                              ▼
                        ┌──────────┐
                        │ Services │
                        └──────────┘
```

### SignalR Kommunikation

```
┌──────────┐                  ┌──────────┐                  ┌──────────┐
│ Client 1 │                  │  Server  │                  │ Client 2 │
│  (MAUI)  │                  │ (SignalR)│                  │  (MAUI)  │
└────┬─────┘                  └────┬─────┘                  └────┬─────┘
     │                             │                             │
     │ SendMessage("User", "Hi")   │                             │
     ├────────────────────────────►│                             │
     │                             │                             │
     │                             │ ReceiveMessage("User", "Hi")│
     │                             ├────────────────────────────►│
     │                             │                             │
     │ ReceiveMessage("User", "Hi")│                             │
     │◄────────────────────────────┤                             │
     │                             │                             │
```

---

## Setup & Installation

### Voraussetzungen

1. **Visual Studio 2022** (Community Edition oder höher)
2. **.NET 9 SDK**
3. **Workloads:**
   - .NET Multi-platform App UI development
   - ASP.NET and web development

### Installation

#### 1. Repository klonen / Projekt öffnen
```bash
# Visual Studio öffnen
# Datei → Lösung öffnen → ChatApp.sln
```

#### 2. NuGet Packages wiederherstellen
```bash
# Automatisch beim ersten Build
# Oder manuell: Rechtsklick auf Solution → NuGet-Pakete wiederherstellen
```

#### 3. Server-URL konfigurieren
In `ChatPageModel.cs` (Zeile ~35):
```csharp
await _chatService.ConnectAsync("https://localhost:7235");
```
⚠️ **Wichtig:** Port 7235 durch den tatsächlichen Server-Port ersetzen!

#### 4. Mehrere Startprojekte konfigurieren
1. Rechtsklick auf Solution → Eigenschaften
2. Startprojekt → Mehrere Startprojekte
3. ChatApp.Server → **Starten**
4. ChatApp → **Starten**
5. OK

#### 5. App starten
```
F5 (Start Debugging)
```

---

## Implementierte Komponenten

### 1. ChatMessage Model

**Datei:** `Models/ChatMessage.cs`

```csharp
public class ChatMessage : INotifyPropertyChanged
{
    public string Text { get; set; }
    public string Sender { get; set; }
    public DateTime Timestamp { get; set; }
    public bool IsMyMessage { get; set; }
    public MessageStatus Status { get; set; }
    
    // Computed Properties
    public string FormattedTime => Timestamp.ToString("HH:mm");
    public string StatusIcon { get; }  // ✓ oder ✓✓
}
```

**Zweck:** 
- Repräsentiert eine einzelne Chat-Nachricht
- Implementiert INotifyPropertyChanged für UI-Updates
- Status-Änderungen (Sent → Delivered → Read)

---

### 2. ChatService

**Datei:** `Services/ChatService.cs`

```csharp
public class ChatService
{
    private HubConnection? _hubConnection;
    public event Action<string, string>? MessageReceived;
    
    public async Task ConnectAsync(string serverUrl);
    public async Task SendMessageAsync(string sender, string message);
    public async Task DisconnectAsync();
}
```

**Funktionen:**
- Verwaltet SignalR HubConnection
- Automatisches Reconnect bei Verbindungsabbruch
- Event-basierte Nachrichtenempfang
- Thread-safe Operationen

---

### 3. ChatPageModel

**Datei:** `PageModels/ChatPageModel.cs`

**Wichtige Properties:**
```csharp
public ObservableCollection<ChatMessage> Messages { get; set; }
[ObservableProperty] private string newMessageText;
[ObservableProperty] private string userName;
```

**Wichtige Commands:**
```csharp
[RelayCommand]
private async Task SendMessage()
```

**Funktionen:**
- Verbindungsaufbau zum Server
- Nachrichtenverwaltung
- UI-Thread-sichere Updates mit `MainThread.BeginInvokeOnMainThread`
- Status-Simulation (für Entwicklung)

---

### 4. ChatHub (Server)

**Datei:** `ChatApp.Server/Hubs/ChatHub.cs`

```csharp
public class ChatHub : Hub
{
    public async Task SendMessage(string sender, string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", sender, message);
    }
}
```

**Funktionalität:**
- Broadcasting von Nachrichten an alle verbundenen Clients
- Automatische Verbindungsverwaltung durch SignalR

---

### 5. Server-Konfiguration

**Datei:** `ChatApp.Server/Program.cs`

```csharp
builder.Services.AddSignalR();
builder.Services.AddCors(options => {
    options.AddPolicy("AllowAll", policy => {
        policy.AllowAnyHeader()
              .AllowAnyMethod()
              .SetIsOriginAllowed(_ => true)
              .AllowCredentials();
    });
});

app.MapHub<ChatHub>("/chathub");
```

**Wichtige Konfigurationen:**
- CORS für lokale Entwicklung
- SignalR Endpoint: `/chathub`
- HTTPS Redirect
- Entwicklungs-Umgebung

---

### 6. Login-System

**Komponenten:**
- `LoginPage.xaml` - UI
- `LoginPageModel.cs` - Logik
- `Preferences` - Persistente Speicherung

**Flow:**
```
App Start → Check Preferences
    ├── Name vorhanden? → ChatPage
    └── Kein Name? → LoginPage
                        ├── Name eingeben
                        └── Save & Navigate → ChatPage
```

---

### 7. Theming System

**Definiert in:** `App.xaml`

**Farb-Ressourcen:**
```xml
<Color x:Key="ChatBackgroundLight">#ECE5DD</Color>
<Color x:Key="ChatBackgroundDark">#0B141A</Color>
<Color x:Key="MyMessageLight">#DCF8C6</Color>
<Color x:Key="MyMessageDark">#005C4B</Color>
```

**Usage mit AppThemeBinding:**
```xml
BackgroundColor="{AppThemeBinding 
    Light={StaticResource ChatBackgroundLight}, 
    Dark={StaticResource ChatBackgroundDark}}"
```

---

## UI-Komponenten Details

### Chat-Sprechblasen

**Eigene Nachrichten (rechts):**
```xml
<Border BackgroundColor="#DCF8C6"
        Padding="12,8"
        MaximumWidthRequest="280">
    <Border.StrokeShape>
        <RoundRectangle CornerRadius="15,15,2,15" />
    </Border.StrokeShape>
    <!-- Content -->
</Border>
```

**Fremde Nachrichten (links):**
```xml
<Border BackgroundColor="White"
        Padding="12,8"
        MaximumWidthRequest="280">
    <Border.StrokeShape>
        <RoundRectangle CornerRadius="15,15,15,2" />
    </Border.StrokeShape>
    <!-- Content -->
</Border>
```

**Sichtbarkeits-Steuerung:**
```xml
IsVisible="{Binding IsMyMessage}"  <!-- Eigene Nachricht -->
IsVisible="{Binding IsMyMessage, Converter={StaticResource InverseBool}}"  <!-- Fremde -->
```

---

### Senden-Button

```xml
<Border BackgroundColor="#25D366"  <!-- WhatsApp Grün -->
        WidthRequest="50"
        HeightRequest="50">
    <Border.StrokeShape>
        <RoundRectangle CornerRadius="25"/>  <!-- Kreis -->
    </Border.StrokeShape>
    <Border.GestureRecognizers>
        <TapGestureRecognizer Command="{Binding SendMessageCommand}"/>
    </Border.GestureRecognizers>
    <Label Text="→" FontSize="28" TextColor="White"/>
</Border>
```

---

## Dependency Injection

**Konfiguration in** `MauiProgram.cs`:

```csharp
builder.Services.AddSingleton<ChatService>();
```

**Verwendung:**
```csharp
// In ChatPage.xaml.cs
var chatService = IPlatformApplication.Current.Services.GetService<ChatService>();
```

**Warum Singleton?**
- Eine ChatService-Instanz für die gesamte App
- Persistente WebSocket-Verbindung
- Geteilter State zwischen Views

---

## Navigation

**Konfiguration in** `AppShell.xaml.cs`:

```csharp
Routing.RegisterRoute(nameof(ChatPage), typeof(ChatPage));
```

**Verwendung:**
```csharp
// Absolute Navigation
await Shell.Current.GoToAsync("///LoginPage");

// Route-basiert
await Shell.Current.GoToAsync(nameof(ChatPage));
```

---

## Deployment

### Android APK erstellen

#### Option 1: Debug APK (zum Testen)

1. **Build Configuration ändern:**
   - Oben: Debug → **Release**
   - Any CPU → **Android**

2. **APK erstellen:**
   ```
   Erstellen → ChatApp veröffentlichen → Ad-hoc
   ```

3. **APK finden:**
   ```
   ChatApp\bin\Release\net9.0-android\publish\
   ```

#### Option 2: Signierte APK (für Distribution)

1. **Keystore erstellen:**
   - Projekt → Eigenschaften → Android → Paket signieren
   - Neuen Keystore erstellen

2. **Release Build:**
   ```
   Erstellen → Archive erstellen...
   ```

3. **Verteilung:**
   - APK per E-Mail/WhatsApp an Familie senden
   - "Installation aus unbekannten Quellen" aktivieren
   - APK installieren

### Server Deployment

#### Lokales Netzwerk

**Server läuft auf:** `https://<deine-ip>:7235`

1. **Firewall-Regel erstellen:**
   - Port 7235 freigeben
   - Nur lokales Netzwerk

2. **IP-Adresse herausfinden:**
   ```bash
   ipconfig  # Windows
   ```

3. **In App konfigurieren:**
   ```csharp
   await _chatService.ConnectAsync("https://192.168.1.x:7235");
   ```

#### Cloud Deployment (Optional)

**Optionen:**
- Azure App Service
- Heroku
- Railway
- Render (kostenlos)

---

## Kosten-Übersicht

### Entwicklung: **0€**
- Visual Studio Community ✅ Kostenlos
- .NET SDK ✅ Kostenlos
- NuGet Packages ✅ Kostenlos

### Testing: **0€**
- Lokale Entwicklung ✅ Kostenlos
- APK-Installation ✅ Kostenlos

### Production (Familie): **0€**
- APK-Verteilung ✅ Kostenlos
- Lokaler Server ✅ Kostenlos

### Optional (falls gewünscht):
- Google Play Store: ~25€ einmalig
- Cloud Hosting: ab 0€ (Free Tier)
- Domain: ab 10€/Jahr

---

## Zukünftige Erweiterungen

### Geplante Features

#### 1. **Persistente Nachrichten**
- SQLite Datenbank Integration
- Nachrichtenverlauf speichern
- Offline-Modus

**Aufwand:** 2-3 Stunden
**Technologien:** SQLite-net-pcl

#### 2. **Medien-Support**
- Bilder versenden
- Vorschau-Thumbnails
- Galerie-Integration

**Aufwand:** 3-4 Stunden
**Technologien:** Media Picker, Image Compression

#### 3. **Push-Benachrichtigungen**
- Firebase Cloud Messaging
- Benachrichtigungen bei neuen Nachrichten
- Badge Counter

**Aufwand:** 4-5 Stunden
**Technologien:** Firebase, Background Services

#### 4. **Gruppen-Chats**
- Mehrere Chat-Räume
- Raum-basierte Nachrichtenverteilung
- Raum-Liste

**Aufwand:** 3-4 Stunden
**Server-Änderungen:** ChatHub erweitern mit Groups

#### 5. **Typing Indicator**
- "XY schreibt..." Anzeige
- Echtzeit-Statusupdate
- Auto-Timeout nach 3 Sekunden

**Aufwand:** 1-2 Stunden

#### 6. **Voice Messages**
- Audio-Aufnahme
- Waveform-Visualisierung
- Abspielen

**Aufwand:** 5-6 Stunden
**Technologien:** Audio Recorder Plugin

#### 7. **End-to-End Verschlüsselung**
- Message Encryption
- Key Exchange
- Secure Storage

**Aufwand:** 8-10 Stunden
**Komplexität:** Hoch

#### 8. **Profilbilder**
- Avatar Upload
- Bildanzeige in Nachrichten
- Profil-Screen

**Aufwand:** 2-3 Stunden

---

## Troubleshooting

### Problem: Server nicht erreichbar

**Symptom:** ❌ Verbindungsfehler in Debug-Output

**Lösung:**
1. Server läuft? (Konsolen-Fenster geöffnet?)
2. Richtige URL? (Port 7235?)
3. Firewall blockiert?
4. HTTPS-Zertifikat akzeptiert?

### Problem: Nachrichten erscheinen nicht

**Symptom:** Nachricht wird gesendet, aber nicht empfangen

**Lösung:**
1. SignalR Verbindung aktiv? Check Debug-Output
2. Beide Clients verbunden?
3. Server-Console zeigt Fehler?

### Problem: Dark Mode funktioniert nicht

**Symptom:** Farben ändern sich nicht

**Lösung:**
1. App.xaml: UserAppTheme NICHT gesetzt?
2. System-Theme umschalten
3. App neu starten

### Problem: Build-Fehler nach Git Pull

**Lösung:**
```
1. Erstellen → Projektmappe bereinigen
2. NuGet-Pakete wiederherstellen
3. Erstellen → Projektmappe neu erstellen
```

---

## Performance-Optimierungen

### Aktuelle Optimierungen

1. **ObservableCollection**
   - Automatisches UI-Update nur bei Änderungen
   - Kein manuelles Refresh nötig

2. **Auto-Scroll**
   - 100ms Delay für Layout-Fertigstellung
   - Verhindert "Springen"

3. **SignalR Auto-Reconnect**
   - Automatische Wiederverbindung
   - Keine verlorenen Nachrichten bei kurzen Disconnects

### Potenzielle Verbesserungen

1. **Virtualisierung**
   - Bei >1000 Nachrichten CollectionView Virtualisierung
   - Speicher-Optimierung

2. **Image Caching**
   - Wenn Bilder implementiert
   - FFImageLoading nutzen

3. **Message Batching**
   - Server: Nachrichten in Batches senden
   - Reduziert Netzwerk-Overhead

---

## Sicherheitshinweise

### ⚠️ Aktuelle Einschränkungen

1. **Keine Authentifizierung**
   - Jeder kann sich als jeder ausgeben
   - Nur für vertrauenswürdige Umgebung (Familie)

2. **Keine Verschlüsselung**
   - Nachrichten im Klartext
   - Server kann alle Nachrichten lesen

3. **CORS AllowAll**
   - Nur für Entwicklung
   - Production: Spezifische Origins

### ✅ Für Production-Deployment

1. **Authentifizierung hinzufügen:**
   - JWT Tokens
   - OAuth 2.0
   - ASP.NET Identity

2. **HTTPS erzwingen:**
   - Gültiges SSL-Zertifikat
   - Let's Encrypt (kostenlos)

3. **CORS einschränken:**
   ```csharp
   policy.WithOrigins("https://your-domain.com")
   ```

4. **Rate Limiting:**
   - Schutz vor Spam
   - ASP.NET Core Rate Limiting

---

## Testing

### Manuelle Tests

#### Test 1: Lokale Kommunikation
1. App starten
2. Name eingeben → Login
3. Nachricht senden
4. Nachricht erscheint sofort? ✅

#### Test 2: Multi-Client
1. Erste Instanz: Name "Papa"
2. Zweite Instanz (Strg+F5): Name "Mama"
3. Nachricht in Instanz 1 senden
4. Erscheint in Instanz 2? ✅
5. Richtige Farbe (weiß/links)? ✅

#### Test 3: Reconnect
1. App laufen lassen
2. Server stoppen (Konsole schließen)
3. Server neu starten
4. Nachricht senden → sollte funktionieren ✅

#### Test 4: Theme-Wechsel
1. Windows Dark Mode aktivieren
2. App hat dunklen Hintergrund? ✅
3. Windows Light Mode aktivieren
4. App hat hellen Hintergrund? ✅

---

## Lessons Learned

### Erfolge ✅

1. **MVVM Pattern**
   - Saubere Architektur von Anfang an
   - Einfache Testbarkeit
   - Wiederverwendbare ViewModels

2. **SignalR Integration**
   - Echtzeit funktioniert zuverlässig
   - Auto-Reconnect out-of-the-box
   - Einfache API

3. **Dark Mode Support**
   - AppThemeBinding macht es einfach
   - Konsistentes Theme switching

### Herausforderungen 🔧

1. **Dependency Injection in Shell**
   - Shell erstellt Pages selbst
   - Lösung: Services in OnAppearing holen

2. **Navigation mit Routen**
   - Manuelle Registrierung nötig
   - Shell Routes vs. Normal Navigation

3. **Thread-Safety**
   - SignalR Events kommen auf Background Thread
   - Lösung: MainThread.BeginInvokeOnMainThread

---

## Credits & Ressourcen

### Verwendete Libraries

- **.NET MAUI** - Microsoft
- **CommunityToolkit.Mvvm** - .NET Foundation
- **SignalR** - Microsoft

### Hilfreiche Ressourcen

1. **Microsoft Docs:**
   - https://learn.microsoft.com/dotnet/maui/
   - https://learn.microsoft.com/aspnet/core/signalr/

2. **Community Toolkit:**
   - https://learn.microsoft.com/dotnet/communitytoolkit/mvvm/

3. **GitHub Samples:**
   - https://github.com/dotnet/maui-samples

---

## Kontakt & Support

**Entwickler:** [Dein Name]
**Projekt:** ChatApp - Family Messenger
**Version:** 1.0.0
**Datum:** Dezember 2024

---

## Changelog

### Version 1.0.0 (07.12.2024)
- ✅ Initiales Release
- ✅ Echtzeit-Chat mit SignalR
- ✅ Login-System
- ✅ WhatsApp-ähnliche UI
- ✅ Dark Mode Support
- ✅ Auto-Scroll
- ✅ Status-Häkchen (Simulation)

---

## Lizenz

Dieses Projekt ist für den privaten, familiären Gebrauch entwickelt.

**MIT License empfohlen für Open Source:**
```
Copyright (c) 2024 [Dein Name]

Permission is hereby granted, free of charge...
```

---

**Ende der Dokumentation**

*Viel Erfolg mit deiner Chat-App! 🚀💬*
