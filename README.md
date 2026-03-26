# SMPCTool-PS4 (Spider-Man PS4 Modding Tool) WIP 
Python version here. [SMPCTool-PS4_python](https://github.com/zerlkung/SMPCTool-PS4_python)

เครื่องมือสำหรับ Mod เกม **Marvel's Spider-Man** เวอร์ชัน **PS4** ที่พัฒนาต่อยอดมาจาก SMPCTool (PC) เพื่อให้สามารถแก้ไขไฟล์และติดตั้ง Mod บนเครื่อง PS4 ได้อย่างสมบูรณ์

## ฟีเจอร์ใหม่สำหรับ PS4 (PS4 Support Features)
- **Auto-Detect Platform**: ระบบตรวจจับแพลตฟอร์ม (PC/PS4) อัตโนมัติจากไฟล์เกม
- **PS4 TOC Support**: รองรับการ Decompress ไฟล์ TOC ของ PS4 (Multi-block zlib) และรองรับ Archive Entry ขนาด 24 ไบต์
- **Asset Header Skip**: ปรับปรุงให้ข้าม Header พิเศษ 38 ไบต์ของ PS4 เพื่ออ่านข้อมูล Asset (DAT1) ได้ถูกต้อง
- **Archive Writing**: รองรับการเขียนไฟล์เข้า Archive ของ PS4 โดยตรง (ไม่ต้องมี Padding blocks เหมือน PC)
- **DAG Support**: รองรับไฟล์ความสัมพันธ์ (Dependencies) ของเวอร์ชัน PS4 (Magic: `0x891F77AF`)
- **Modern Build Support**: เพิ่มการรองรับ .NET 8.0 สำหรับการ Build ในปัจจุบัน

## วิธีการ Build (How to Build)
โปรเจกต์นี้รองรับทั้ง .NET Framework 4.7.2 และ .NET 8.0:

### 1. ใช้ Visual Studio 2022 (แนะนำ)
- เปิดไฟล์ `SMPCTool.csproj` (.NET Framework 4.7.2) หรือ `SMPCTool-net8.csproj` (.NET 8)
- กด `Ctrl+Shift+B` เพื่อ Build

### 2. ใช้ dotnet CLI (.NET 8)
```cmd
cd SMPCTool
dotnet build SMPCTool-net8.csproj -c Release
```

ดูรายละเอียดเพิ่มเติมใน [BUILD_GUIDE.md](./BUILD_GUIDE.md)

## ความแตกต่างทางเทคนิค (Technical Details)
สำหรับรายละเอียดเชิงลึกเกี่ยวกับความแตกต่างระหว่างไฟล์เวอร์ชัน PC และ PS4 สามารถอ่านได้ที่ [PS4_Modding_Technical_Report.md](./PS4_Modding_Technical_Report.md)

## เครดิต (Credits)
โปรเจกต์นี้เป็นการนำซอร์สโค้ดต้นฉบับมาพัฒนาต่อยอดเพื่อรองรับ PS4 โดยยังคงให้เครดิตผู้พัฒนาเดิม:
- **jedijosh920** - ผู้พัฒนา SMPCTool ต้นฉบับ
- **Phew** - ผู้เผยแพร่ซอร์สโค้ด SMPCTool-src
- **zerlkung** - ผู้ริเริ่มโปรเจกต์เวอร์ชัน PS4
- และทีมงานทุกคนที่มีส่วนร่วมในการพัฒนาเครื่องมือนี้

## ลิงก์ต้นฉบับ (Original Links)
- **SMPCTool (PC) on NexusMods**: [https://www.nexusmods.com/marvelsspidermanremastered/mods/51](https://www.nexusmods.com/marvelsspidermanremastered/mods/51)
- **Original Source**: [https://github.com/Phew/SMPCTool-src](https://github.com/Phew/SMPCTool-src)

---

<p align="center">
  <img width="700" src="https://a.pomf.cat/hndpga.png">
</p>

<p align="center">
  <img width="700" src="https://a.pomf.cat/ygqtof.png">
</p>
