<div align="center">
  <br>
  <h1>🔌 Port Checker</h1>
  <p><strong>เครื่องมือจัดการพอร์ตและโปรเซสสำหรับ Windows</strong></p>
  <p><em>ดูว่าใครใช้พอร์ตอะไรอยู่ — ฆ่าทิ้งได้ถ้ากวน</em></p>
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

> **Port Checker** เป็นโปรแกรม Windows ขนาดเล็กที่ใช้ดูว่าพอร์ตไหนกำลังถูกโปรแกรมอะไรใช้อยู่ พร้อมดูการใช้ RAM, CPU และตำแหน่งไฟล์แบบ real-time  
> ดาวน์โหลดได้ที่ [latest release](https://github.com/ChokechaiXD/port-check/releases) — ไฟล์ `.exe` เดียว ไม่ต้องติดตั้ง

ใช้ได้บน **Windows 10 / 11** สร้างด้วย **C# WinForms (.NET Framework 4.8+)**

---

## 🔧 ติดตั้ง

```
ดาวน์โหลด port-check.exe จาก Releases → เปิดใช้เลย
```

ไม่ต้องติดตั้งอะไร ไม่ต้องใช้ npm ไม่มี config

หรือสร้างจาก source:

```powershell
csc.exe /target:winexe /reference:System.Windows.Forms.dll /reference:System.Drawing.dll /reference:System.Management.dll port-check.cs
```

---

## 🚀 เริ่มต้นใช้งาน

```
port-check.exe
```

เปิดมาแล้วจะเห็นพอร์ตทั้งหมดที่กำลังใช้งาน

### ค้นหาตามพอร์ต

พิมพ์เลขพอร์ตแล้วกด Enter:

```
พิมพ์ "3000" → ตารางจะแสดงเฉพาะพอร์ต 3000
กด Enter หรือ Refresh เพื่อค้นหา
```

### ฆ่าโปรเซส

เลือกแถวแล้วกด **Kill**:

```
เลือก node.exe บนพอร์ต 9000
กด Kill → ยืนยัน → โปรเซสถูกปิด
```

### อัปเดตอัตโนมัติ

เปิด/ปิดได้ที่ checkbox Auto-refresh อัปเดตทุก **5 วินาที**

---

## 📋 คุณสมบัติ

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
      <td align="center" width="160"><b>🔄 อัตโนมัติ</b><br><small>ทุก 5 วิ</small></td>
      <td align="center" width="160"><b>↕️ เรียง</b><br><small>ทุกคอลัมน์</small></td>
      <td align="center" width="160"><b>⚡ พกพา</b><br><small>ไฟล์ .exe เดียว</small></td>
    </tr>
  </table>
</div>

---

## 🖥️ หลักการทำงาน

Port Checker ใช้คำสั่ง `netstat -ano` ของ Windows เพื่อดึงข้อมูลพอร์ต — ไม่ต้องใช้สิทธิ์ Admin:

- สแกนพอร์ต TCP และ UDP ทั้งหมด
- จับคู่พอร์ตกับโปรเซสที่เป็นเจ้าของ (PID)
- อ่านค่า **หน่วยความจำ** และ **CPU %** แบบ real-time ผ่าน WMI
- หาตำแหน่ง **ไฟล์ .exe** ของแต่ละโปรเซส

ตารางอัปเดตแบบ in-place — ไม่กระพริบ ไม่สะดุด

---

## 🤝 ร่วมพัฒนา

PR ยินดีต้อนรับ Fork → เขียนโค้ด → PR

---

## 📖 ภาษาอื่น

- [🇬🇧 English — Read in English](README.md)

---

<div align="center">
  <small>MIT License · ฟรี · สร้างด้วย C# WinForms</small>
</div>
