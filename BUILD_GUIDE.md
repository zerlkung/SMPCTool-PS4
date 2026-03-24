# SMPCTool-PS4 - Build Guide สำหรับ Windows

## ภาพรวม

โปรเจกต์นี้มี project file ให้เลือก 2 แบบ:

| ไฟล์ | Target Framework | เหมาะสำหรับ |
|------|-----------------|-------------|
| `SMPCTool.csproj` | .NET Framework 4.7.2 | Visual Studio 2019/2022 (เหมือนต้นฉบับ) |
| `SMPCTool-net8.csproj` | .NET 8.0 | dotnet CLI หรือ Visual Studio 2022 |

---

## วิธีที่ 1: Visual Studio 2022 (แนะนำ - ง่ายที่สุด)

### ขั้นตอน:
1. ติดตั้ง **Visual Studio 2022** (Community Edition ฟรี) จาก https://visualstudio.microsoft.com/
2. ตอนติดตั้งเลือก Workload: **".NET desktop development"**
3. เปิดไฟล์ `SMPCTool.csproj` (สำหรับ .NET Framework 4.7.2) หรือ `SMPCTool-net8.csproj` (สำหรับ .NET 8)
4. กด **Ctrl+Shift+B** เพื่อ Build
5. ไฟล์ .exe จะอยู่ที่ `bin\Release\` หรือ `bin\Debug\`

### หมายเหตุ:
- ถ้าใช้ `SMPCTool.csproj` ต้องวาง `DotNetZip.dll` ไว้ที่ `..\..\DotNetZip.dll` (2 ระดับเหนือโฟลเดอร์ SMPCTool) หรือแก้ HintPath ใน .csproj
- ถ้าใช้ `SMPCTool-net8.csproj` จะดาวน์โหลด DotNetZip จาก NuGet อัตโนมัติ

---

## วิธีที่ 2: dotnet CLI (ไม่ต้องเปิด Visual Studio)

### ติดตั้ง .NET 8 SDK:
ดาวน์โหลดจาก https://dotnet.microsoft.com/download/dotnet/8.0

### Build:
```cmd
cd SMPCTool
dotnet build SMPCTool-net8.csproj -c Release
```

### ผลลัพธ์:
ไฟล์จะอยู่ที่ `bin\Release\net8.0-windows\`

### Publish เป็น .exe เดี่ยว (Self-Contained):
```cmd
dotnet publish SMPCTool-net8.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -o publish
```
จะได้ไฟล์ `SMPCTool-PS4.exe` ไฟล์เดียวที่รันได้เลยโดยไม่ต้องติดตั้ง .NET Runtime

### Publish แบบ Framework-Dependent (ไฟล์เล็กกว่า):
```cmd
dotnet publish SMPCTool-net8.csproj -c Release -r win-x64 --self-contained false -o publish
```
ต้องติดตั้ง .NET 8 Runtime บนเครื่องที่จะรัน

---

## วิธีที่ 3: MSBuild (สำหรับ .NET Framework 4.7.2)

### ติดตั้ง:
- ติดตั้ง **Visual Studio Build Tools 2022** จาก https://visualstudio.microsoft.com/downloads/#build-tools-for-visual-studio-2022
- เลือก Workload: ".NET desktop build tools"

### Build:
```cmd
msbuild SMPCTool.csproj /p:Configuration=Release /p:Platform=x64
```

### หมายเหตุ:
- ต้องวาง `DotNetZip.dll` ให้ตรงตาม HintPath ใน .csproj

---

## Dependencies

| Library | Version | วิธีได้มา |
|---------|---------|----------|
| DotNetZip | 1.16.0 | NuGet (อัตโนมัติสำหรับ .NET 8) หรือดาวน์โหลด DLL |
| .NET Framework 4.7.2 | - | ติดตั้งผ่าน Visual Studio (สำหรับ SMPCTool.csproj) |
| .NET 8.0 SDK | 8.0+ | https://dotnet.microsoft.com/download (สำหรับ SMPCTool-net8.csproj) |

---

## การใช้งาน

1. วาง `AssetHashes.txt` ไว้ข้างๆ ไฟล์ .exe (ใช้ของ PC ได้ครอบคลุม ~89% ของ base game)
2. เปิดโปรแกรมแล้วชี้ไปที่โฟลเดอร์ที่มีไฟล์ PS4 (toc, dag, p000xxx)
3. โปรแกรมจะ auto-detect ว่าเป็นไฟล์ PS4 โดยอัตโนมัติ
