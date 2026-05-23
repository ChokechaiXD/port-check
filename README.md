# 🔌 Port Checker

See who's using your ports — kill them if you want

```
 ┌────────────────────────────────────────────────────────────────────────────────────────────────┐
 │ Port: [_____] [Refresh] [Kill] ☐ Auto 5s       23 connection(s)  |  14:30:00                  │
 ├──────┬──────┬─────────────┬───────┬───────┬─────────┬───────┬──────────────────────────────────┤
 │ Port │ PID  │ Name        │ Proto │ State │ Mem(MB) │ CPU%  │ Path                             │
 ├──────┼──────┼─────────────┼───────┼───────┼─────────┼───────┼──────────────────────────────────┤
 │  135 │ 1172 │ svchost     │ TCP   │ Listen│  13.1   │  0.0  │ C:\Windows\System32\svchost.exe  │
 │  139 │    4 │ System      │ TCP   │ Listen│   2.5   │   -   │ C:\Windows\System32\ntoskrnl.exe │
 │  445 │    4 │ System      │ TCP   │ Listen│   2.5   │   -   │ C:\Windows\System32\ntoskrnl.exe │
 │ 5040 │ 7528 │ svchost     │ TCP   │ Listen│  15.6   │  0.1  │ C:\Windows\System32\svchost.exe  │
 │ 5353 │ 2032 │ svchost     │ UDP   │ UDP   │  14.3   │  0.0  │ C:\Windows\System32\svchost.exe  │
 │ 5354 │ 2772 │ svchost     │ UDP   │ UDP   │  13.6   │  0.0  │ C:\Windows\System32\svchost.exe  │
 │ 9000 │ 4404 │ node        │ TCP   │ Listen│  42.3   │  2.1  │ C:\Program Files\node\node.exe   │
 └──────┴──────┴─────────────┴───────┴───────┴─────────┴───────┴──────────────────────────────────┘
```

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
