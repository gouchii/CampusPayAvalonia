# Avalonia UI Frontend

A cross-platform desktop frontend built with [Avalonia UI](https://avaloniaui.net/), designed to interact with the ASP.NET Core Web API backend.

---

## 🚀 Features

- Cross-platform desktop UI (Windows, macOS, Linux)
- Clean MVVM architecture
- SignalR client integration
- Real-time updates via backend API

---

## 🛠️ Setup Instructions

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- The [ASP.NET Core Web API backend](https://github.com/gouchii/CampusPay) must be running

### Installation

1. **Clone the repository**

```bash
git clone https://github.com/gouchii/CampusPayAvalonia.git
```

2. **Ensure the backend is running**

Start the ASP.NET Core Web API before running this frontend. The frontend expects the backend to be running at:
```bash
http://localhost:5019
```


**Restore dependencies and build**
```bash
dotnet restore
```
```bash
dotnet build
```

**Run the application**
```bash
dotnet run
```
