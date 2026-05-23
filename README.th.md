<div align="center">
  <h1>🔌 Port Checker</h1>
  <p><strong>ดูว่าใครใช้พอร์ตอะไรอยู่ — ฆ่าทิ้งได้ถ้ากวน</strong></p>
  <br>
  <p>
    <img src="https://img.shields.io/badge/Windows-0078D4?style=flat-square&logo=windows11&logoColor=white">
    <img src="https://img.shields.io/badge/.NET_Framework-512BD4?style=flat-square&logo=dotnet&logoColor=white">
    <img src="https://img.shields.io/badge/C%23-239120?style=flat-square&logo=csharp&logoColor=white">
    <img src="https://img.shields.io/github/license/ChokechaiXD/port-check?style=flat-square&color=blue">
    <img src="https://img.shields.io/badge/ฟรี-00B4D8?style=flat-square">
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
      <td align="center" width="160"><b>📋 รายการพอร์ต</b><br><small>TCP & UDP</small></td>
      <td align="center" width="160"><b>🔍 ค้นหา</b><br><small>ตามเลขพอร์ต</small></td>
      <td align="center" width="160"><b>📊 ทรัพยากร</b><br><small>RAM & CPU %</small></td>
      <td align="center" width="160"><b>📍 ตำแหน่ง</b><br><small>ที่อยู่ไฟล์ exe</small></td>
    </tr>
    <tr>
      <td align="center" width="160"><b>💀 ฆ่า</b><br><small>คลิกเดียว</small></td>
      <td align="center" width="160"><b>🔄 อัตโนมัติ</b><br><small>ทุก 5 วินาที</small></td>
      <td align="center" width="160"><b>↕️ เรียงลำดับ</b><br><small>ทุกคอลัมน์</small></td>
      <td align="center" width="160"><b>⚖️ พกพา</b><br><small>ไฟล์ .exe เดียว</small></td>
    </tr>
  </table>
</div>

<br>

---

## 🚀 เริ่มต้นใช้งาน

```powershell
# ดาวน์โหลด port-check.exe แล้วเปิด
port-check.exe
```

ไม่ต้องติดตั้งอะไร เพิ่มแค่เปิดใช้

---

## 🛠️ สร้างจาก source

ต้องมี .NET Framework 4.8+

```powershell
csc.exe /target:winexe `
  /reference:System.Windows.Forms.dll `
  /reference:System.Drawing.dll `
  /reference:System.Management.dll `
  port-check.cs
```

---

## 📖 ภาษาอื่น

- [English — Read in English](README.md)

---

<div align="center">
  <small>MIT License · ฟรี · สร้างด้วย C# WinForms</small>
</div>
