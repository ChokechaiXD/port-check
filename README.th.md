<div align="center">
  <br>
  <h1>🔌 Port Checker</h1>
  <p><strong>จบปัญหา "port already in use" — คลิกเดียว ไม่ต้องเปิด CMD</strong></p>
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
 ┌───────────────────────────────────────────────────────────────────────────────────────┐
 │ Port: [____] Name: [_______]  [Refresh] [Kill] ☑ Auto 5s  23 con | 5 proc | 180 MB  │
 ├──────┬──────┬─────────┬───────┬───────┬──────────────┬───────┬────────────────────────┤
 │ Port │ PID  │ Name    │ Proto │ State │ Remote       │  Mem  │ Path                   │
 ├──────┼──────┼─────────┼───────┼───────┼──────────────┼───────┼────────────────────────┤
 │ 3000 │ 4404 │ node    │ TCP   │ Listen│ 0.0.0.0:0    │ 42.3  │ C:\Program Files\...   │
 │ 9229 │ 4404 │ node    │ TCP   │ Listen│ 0.0.0.0:0    │ 42.3  │ C:\Program Files\...   │
 │ 5040 │ 7528 │ svchost │ TCP   │ Listen│ 0.0.0.0:0    │ 15.6  │ C:\Windows\System32\  │
 │ 5353 │ 2032 │ svchost │ UDP   │ UDP   │ *:*          │ 14.3  │ C:\Windows\System32\  │
 └──────┴──────┴─────────┴───────┴───────┴──────────────┴───────┴────────────────────────┘
```

<br>

## ปัญหาที่แก้

กำลังจะรัน dev server แล้วเจอ:

```
Error: listen EADDRINUSE: address already in use :::3000
```

วิธีเดิม:

```bash
netstat -ano | findstr :3000  # หาพอร์ต
tasklist /FI "PID eq 4404"     # หาว่าโปรแกรมอะไร
taskkill /PID 4404             # ฆ่าทิ้ง
```

**Port Checker** รวมสามขั้นตอนเป็น GUI เดียว — พิมพ์พอร์ต → กด Kill → จบ

---

## เริ่มต้นใช้

1. ดาวน์โหลด [`port-check.exe`](https://github.com/ChokechaiXD/port-check/releases)
2. เปิดเลย — ไม่ต้องติดตั้ง ไม่ต้องสิทธิ์ Admin
3. พิมพ์ `3000` → Enter → รู้ทันทีว่าใครใช้พอร์ต 3000
4. เลือกแถว → กด **Kill** → เสร็จ

หรือใส่พอร์ตตอนรันเลย:

```bash
port-check.exe 3000
```

ใช้ได้บน **Windows 10 / 11** ไฟล์ `.exe` เดียว **14 KB**

---

## ทำไมไม่ใช้ netstat อย่างเดียว?

| netstat (CMD) | Port Checker |
|---------------|--------------|
| ข้อความดิบ อ่านยาก | ตาราง sortable คลิกหัวคอลัมน์เรียงได้ |
| ไม่ auto-refresh | รีเฟรชอัตโนมัติทุก 5 วิ (เปิด/ปิดได้) |
| ฆ่าไม่ได้ แค่บอก PID | ปุ่ม Kill + ยืนยันอีกครั้ง |
| ไม่มีข้อมูล RAM/CPU | RAM และ CPU% แบบ real-time |
| filter ด้วย `findstr` เท่านั้น | ค้นหาตามพอร์ต + ค้นหาตามชื่อ process |
| ไม่มี Remote Address | มีคอลัมน์ Remote Address |
| บาง flag ต้อง Admin | **ไม่ต้องใช้ Admin** |
| ปรับแต่งคอลัมน์ไม่ได้ | ซ่อน/แสดงคอลัมน์ได้ (คลิกขวาที่หัวตาราง) |
| ข้อความเลื่อนหาย | ตารางอยู่กับที่ อัปเดต in-place |

---

## คุณสมบัติ

| หมวด | รายละเอียด |
|------|-----------|
| **สแกนพอร์ต** | TCP + UDP ผ่าน `netstat -ano` ไม่ต้อง Admin |
| **ข้อมูล real-time** | RAM (MB), CPU% แบบ TotalProcessorTime differential |
| **ข้อมูล process** | ชื่อ, PID, ที่อยู่ไฟล์ .exe |
| **ค้นหา** | ตามพอร์ต หรือตามชื่อ process |
| **ฆ่า process** | คลิกเดียว + ยืนยัน (หรือกด K / Del หรือดับเบิลคลิก) |
| **คัดลอก** | คลิกขวา: คัดลอก Port / PID / Name / Remote / Path หรือทั้งแถว (Ctrl+C) |
| **เรียงลำดับ** | คลิกหัวคอลัมน์ — เรียงตัวเลขได้ กลับลำดับได้ |
| **รีเฟรชอัตโนมัติ** | เปิด/ปิดได้ อัปเดตทุก 5 วิ ไม่กระพริบ |
| **Remote Address** | ดูว่าการเชื่อมต่อนี้ไปไหน |
| **ซ่อน/แสดงคอลัมน์** | คลิกขวาที่หัวตาราง → เลือกแสดงหรือซ่อน |
| **จำค่าต่างๆ** | จำ port filter, name filter, ขนาดหน้าต่าง |
| **พกพา** | .exe เดียว ไม่มี dependencies 14 KB |
| **ไม่ต้องตั้งค่า** | ดาวน์โหลด → เปิดใช้เลย |

---

## หลักการทำงาน

Port Checker ใช้คำสั่ง `netstat -ano` ที่ Windows มีอยู่แล้ว — คำสั่งเดียวกับที่พิมพ์ใน CMD — แต่เอาข้อมูลมาแสดงใน UI ที่ใช้งานง่าย:

- แหล่งข้อมูล: `netstat -ano` (ไม่ต้องใช้สิทธิ์พิเศษ)
- RAM: Process.WorkingSet64
- CPU: Process.TotalProcessorTime differential (ไม่ต้องใช้ WMI)
- ค้นหาและเรียงลำดับแบบ real-time ในเครื่อง

---

## Build จาก source

```powershell
csc.exe /target:winexe /win32icon:port-check.ico ^
  /reference:System.Windows.Forms.dll ^
  /reference:System.Drawing.dll ^
  port-check.cs
```

ต้องมี .NET Framework 4.8+ (มีอยู่ใน Windows 10/11 อยู่แล้ว)

---

## ร่วมพัฒนา

PR ยินดีต้อนรับ ถ้าจะแก้ใหญ่เปิด Issue ก่อนนะ

---

## ภาษาอื่น

- [🇬🇧 English](README.md)

---

<div align="center">
  <small>MIT License · ฟรี · สร้างด้วย C# WinForms</small>
</div>
