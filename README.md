<div align="center">
  <h1>🔌 Port Checker</h1>
  <p><strong>See who's using your ports — kill them if you want</strong></p>
  <br>
  <p>
    <img src="https://img.shields.io/badge/Windows-0078D4?style=flat-square&logo=windows11&logoColor=white">
    <img src="https://img.shields.io/badge/.NET_Framework-512BD4?style=flat-square&logo=dotnet&logoColor=white">
    <img src="https://img.shields.io/badge/C%23-239120?style=flat-square&logo=csharp&logoColor=white">
    <img src="https://img.shields.io/github/license/ChokechaiXD/port-check?style=flat-square&color=blue">
    <img src="https://img.shields.io/badge/Free-00B4D8?style=flat-square">
  </p>
</div>

<br>

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

<br>

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

<br>

---

## 🚀 Quick start

```powershell
# Download port-check.exe and run
port-check.exe
```

No installation. No dependencies. Just works.

---

## 🛠️ Build from source

Requires .NET Framework 4.8+.

```powershell
csc.exe /target:winexe `
  /reference:System.Windows.Forms.dll `
  /reference:System.Drawing.dll `
  /reference:System.Management.dll `
  port-check.cs
```

---

## 📖 Languages

- [ไทย — อ่านภาษาไทย](README.th.md)

---

<div align="center">
  <small>MIT Licensed · Free to use · Built with C# WinForms</small>
</div>
