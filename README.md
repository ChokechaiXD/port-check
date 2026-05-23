<div align="center">
  <br>
  <h1>🔌 Port Checker</h1>
  <p><strong>The "port already in use" fixer — one click, no terminal</strong></p>
  <br>
  <p>
    <a href="https://github.com/ChokechaiXD/port-check/releases">
      <img src="https://img.shields.io/badge/version-1.0-2563eb?style=flat-square" alt="version">
    </a>
    <a href="https://github.com/ChokechaiXD/port-check/releases">
      <img src="https://img.shields.io/github/downloads/ChokechaiXD/port-check/total?style=flat-square&color=00B4D8" alt="downloads">
    </a>
    <img src="https://img.shields.io/badge/Windows-0078D4?style=flat-square&logo=windows11&logoColor=white" alt="Windows">
    <img src="https://img.shields.io/badge/.NET_Framework-512BD4?style=flat-square&logo=dotnet&logoColor=white" alt=".NET">
    <img src="https://img.shields.io/github/license/ChokechaiXD/port-check?style=flat-square&color=blue" alt="license">
  </p>
  <br>
</div>

```
 ┌──────────────────────────────────────────────────────────────────────────────────────────────┐
 │ Port: [____] Name: [_______]  [Refresh] [Kill] ☑ Auto 5s [Export]  23 con | 5 proc | 180 MB │
 ├──────┬──────┬─────────┬───────┬───────┬──────────────┬───────┬──────┬────────────────────────┤
 │ Port │ PID  │ Name    │ Proto │ State │ Remote       │  Mem  │ CPU% │ Path                   │
 ├──────┼──────┼─────────┼───────┼───────┼──────────────┼───────┼──────┼────────────────────────┤
 │ 3000 │ 4404 │ node    │ TCP   │ Listen│ 0.0.0.0:0    │ 42.3  │ 2.1  │ C:\Program Files\...   │
 │ 9229 │ 4404 │ node    │ TCP   │ Listen│ 0.0.0.0:0    │ 42.3  │ 0.0  │ C:\Program Files\...   │
 │ 5040 │ 7528 │ svchost │ TCP   │ Listen│ 0.0.0.0:0    │ 15.6  │ 0.1  │ C:\Windows\System32\   │
 │ 5353 │ 2032 │ svchost │ UDP   │ UDP   │ *:*          │ 14.3  │ 0.0  │ C:\Windows\System32\   │
 └──────┴──────┴─────────┴───────┴───────┴──────────────┴───────┴──────┴────────────────────────┘
```

<br>

## Why

You're about to start your dev server and get this:

```
Error: listen EADDRINUSE: address already in use :::3000
```

The old way:

```bash
netstat -ano | findstr :3000  # find the port
tasklist /FI "PID eq 4404"     # find what process
taskkill /PID 4404             # kill it
```

**Port Checker** does all of that in one GUI. Type the port → press Kill → done.

<p align="center">
  <img src="architecture.svg" alt="Port Checker Architecture" width="700">
</p>

---

## Quick Start

1. Download [`port-check.exe`](https://github.com/ChokechaiXD/port-check/releases)
2. Run it — no install, no admin rights needed
3. Type `3000` → hit Enter → see who owns port 3000
4. Select the row → click **Kill** → done

Or pass the port as a CLI argument:

```bash
port-check.exe 3000
```

Works on **Windows 10 / 11**. Single `.exe`, **22 KB**.

---

## Why a GUI instead of netstat?

| netstat (cmd) | Port Checker |
|---------------|--------------|
| Raw text, hard to scan | Sortable table, click any header |
| No auto-refresh | Auto-refresh every 5s (toggle) |
| Can't kill, just shows PID | Kill button + confirm dialog |
| No memory or CPU info | Real-time RAM & CPU% per process |
| Filter by `findstr` only | Port filter + Name search |
| No remote address | Remote Address column |
| Need admin for some flags | No admin rights required |
| Can't customize columns | Show/hide any column (right-click header) |
| No export | Export to CSV with Save dialog |
| Text scrolls away | Stays in view, updates in-place |

---

## Features

| Category | Details |
|----------|---------|
| **Port scanning** | TCP + UDP via `netstat -ano` — no admin needed |
| **Real-time data** | RAM (MB), CPU% via TotalProcessorTime differential |
| **Process info** | Name, PID, executable path |
| **Filter** | By port number or process name |
| **Kill** | One-click with confirmation (or K / Del key, or double-click) |
| **Copy** | Right-click: copy Port / PID / Name / Remote / Path or whole row (Ctrl+C) |
| **Sort** | Click any column header — numeric-aware, asc/desc toggle |
| **Auto-refresh** | Toggle on/off, updates in-place every 5s — no flicker |
| **Remote Address** | See where each connection goes |
| **Show/hide columns** | Right-click column header → toggle visibility |
| **Settings** | Remembers port filter, name filter, window size between sessions |
| **Export CSV** | Save currently filtered/sorted data to file |
| **Portable** | Single `.exe`, no dependencies, 22 KB |
| **Zero config** | Download → run. That's it |

---

## How it works

Port Checker uses Windows' built-in `netstat -ano` — the same command you'd type manually — and wraps it in a proper UI. No elevated privileges, no background services, no external dependencies.

- Data source: `netstat -ano` (zero-permission port info)
- Memory: Process.WorkingSet64
- CPU: Process.TotalProcessorTime differential (no WMI needed)
- Filtering & sorting done client-side in real-time

---

## Build from source

```powershell
csc.exe /target:winexe /win32icon:port-check.ico ^
  /reference:System.Windows.Forms.dll ^
  /reference:System.Drawing.dll ^
  port-check.cs
```

Requires .NET Framework 4.8+ (built into Windows 10/11).

---

## Contributing

PRs welcome. Open an issue first for bigger changes.

---

## Languages

- [🇹🇭 ภาษาไทย](README.th.md)

---

<div align="center">
  <small>MIT Licensed · Free to use · Built with C# WinForms</small>
</div>
