<div align="center">
  <br>
  <h1>🔌 Port Checker</h1>
  <p><strong>Runtime Port & Process Manager for Windows</strong></p>
  <p><em>See who's using your ports — kill them if you want</em></p>
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
 ┌─────────────────────────────────────────────────────────────────────────────────────────────────┐
 │ Port: [_____] [Refresh] [Kill] ☐ Auto 5s        23 connection(s)  |  14:30:00                  │
 ├──────┬──────┬─────────────┬───────┬───────┬─────────┬───────┬───────────────────────────────────┤
 │ Port │ PID  │ Name        │ Proto │ State │ Mem(MB) │ CPU%  │ Path                              │
 ├──────┼──────┼─────────────┼───────┼───────┼─────────┼───────┼───────────────────────────────────┤
 │  135 │ 1172 │ svchost     │ TCP   │ Listen│  13.1   │  0.0  │ C:\Windows\System32\svchost.exe   │
 │  139 │    4 │ System      │ TCP   │ Listen│   2.5   │   -   │ C:\Windows\System32\ntoskrnl.exe  │
 │  445 │    4 │ System      │ TCP   │ Listen│   2.5   │   -   │ C:\Windows\System32\ntoskrnl.exe  │
 │ 5040 │ 7528 │ svchost     │ TCP   │ Listen│  15.6   │  0.1  │ C:\Windows\System32\svchost.exe   │
 │ 5353 │ 2032 │ svchost     │ UDP   │ UDP   │  14.3   │  0.0  │ C:\Windows\System32\svchost.exe   │
 │ 5354 │ 2772 │ svchost     │ UDP   │ UDP   │  13.6   │  0.0  │ C:\Windows\System32\svchost.exe   │
 │ 9000 │ 4404 │ node        │ TCP   │ Listen│  42.3   │  2.1  │ C:\Program Files\node\node.exe    │
 └──────┴──────┴─────────────┴───────┴───────┴─────────┴───────┴───────────────────────────────────┘
```

<br>

> **Port Checker** is a lightweight Windows GUI tool that lists all listening TCP & UDP ports, their owning processes, real-time resource usage (Memory & CPU%), and executable paths.  
> Download the [latest release](https://github.com/ChokechaiXD/port-check/releases) — single `.exe`, no install required.

Works on **Windows 10 / 11**. Built with **C# WinForms (.NET Framework 4.8+)**.

---

## 🔧 Install

```
Download port-check.exe from Releases → run it
```

No dependencies. No npm. No config.

Or build from source:

```powershell
csc.exe /target:winexe /reference:System.Windows.Forms.dll /reference:System.Drawing.dll /reference:System.Management.dll port-check.cs
```

---

## 🚀 Quick Start

```
port-check.exe
```

The GUI opens and immediately shows all listening ports.

### Filter by port

Type a port number and press Enter:

```
Type "3000" → grid filters to only port 3000
Press Enter or click "Refresh" to apply
```

### Kill a process

Select a row and click **Kill**:

```
Select node.exe on port 9000
Click Kill → confirm → process terminated
```

### Auto-refresh

Toggle auto-refresh on/off with the checkbox. Updates every **5 seconds** by default.

---

## 📋 Features

<div align="center">
  <table>
    <tr>
      <td align="center" width="160"><b>📋 Port List</b><br><small>TCP & UDP</small></td>
      <td align="center" width="160"><b>🔍 Filter</b><br><small>By port number</small></td>
      <td align="center" width="160"><b>📊 Resources</b><br><small>RAM & CPU %</small></td>
      <td align="center" width="160"><b>📍 Path</b><br><small>Exe location</small></td>
    </tr>
    <tr>
      <td align="center" width="160"><b>💀 Kill</b><br><small>One-click</small></td>
      <td align="center" width="160"><b>🔄 Auto-refresh</b><br><small>Every 5s</small></td>
      <td align="center" width="160"><b>↕️ Sort</b><br><small>Any column</small></td>
      <td align="center" width="160"><b>⚡ Portable</b><br><small>Single .exe</small></td>
    </tr>
  </table>
</div>

---

## 🖥️ How it works

Port Checker uses Windows built-in **`netstat -ano`** to gather connection data — no admin rights needed, no external dependencies:

- Scans all TCP/UDP listening ports
- Maps each port to its owning process (PID)
- Reads real-time **Memory (Working Set)** and **CPU %** via WMI
- Resolves the full **executable path** for each process

The table updates in-place every refresh — no flicker, no flash.

---

## 🤝 Contributing

PRs welcome. Fork → code → PR.

---

## 📖 Languages

- [🇹🇭 ภาษาไทย — อ่านได้ที่นี่](README.th.md)

---

<div align="center">
  <small>MIT Licensed · Free to use · Built with C# WinForms</small>
</div>
