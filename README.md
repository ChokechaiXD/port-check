# 🔌 Port Checker

See who's using your ports — kill them if you want

![screenshot](screenshot.png)

## Features

- **Port List** — Shows all TCP & UDP listening ports
- **Filter** — Type a port number to focus on one service
- **Resources** — Real-time Memory (MB) and CPU % per process
- **Path** — Know exactly which executable is listening
- **Kill** — Select a row and kill the process instantly
- **Auto-refresh** — Updates every 5 seconds, toggle on/off
- **Sort** — Click any column header to sort

## Usage

```
port-check.exe
```

Just run it. No installation required. No dependencies.

## Build

Requires .NET Framework 4.8+ and csc.exe.

```powershell
csc.exe /target:winexe /reference:System.Windows.Forms.dll /reference:System.Drawing.dll /reference:System.Management.dll port-check.cs
```

## License

MIT
