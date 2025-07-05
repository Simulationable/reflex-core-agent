
# ReflexCore

**ReflexCore** คือระบบ AI Automation แบบรันได้จริง สำหรับนักพัฒนาที่ต้องการใช้งาน LLM, ระบบบริหารความรู้, และเอเจนต์อัตโนมัติ  
ระบบนี้ถูกออกแบบให้เรียบง่าย ใช้โครงสร้างแบบ Monolith (ASP.NET MVC) และรันได้ทันทีผ่าน Docker เพียงหนึ่งคำสั่ง

---

## 🔧 คุณสมบัติหลัก

- 🧠 ใช้ LLM ฝังในระบบ (llama.cpp) พร้อม CUDA
- 📚 ระบบจัดการความรู้ (Knowledge Entry) พร้อมค้นหาด้วยเวกเตอร์
- 🤖 Agent ที่ทำงานด้วยกฎ Rule-based ไม่พึ่ง LLM ตัดสินใจ
- 📦 ติดตั้งได้ง่ายในเครื่องเดียวผ่าน Docker Image เดียว
- ❌ ไม่มี Multi-tenant, ไม่มี RBAC
- ⚙️ ไม่ใช้ Modular, เป็น Monolith MVC อย่างเรียบง่าย

---

## 🚀 เริ่มต้นใช้งานอย่างรวดเร็ว

### 1. Clone โปรเจกต์

```bash
git clone https://github.com/Simulationable/reflex-core-agent
cd reflexcore
```

### 2. ตั้งค่า Environment Variable สำหรับ PostgreSQL

```bash
export POSTGRES_CONNECTION="Host=ep-red-art-a16dnquz-pooler.ap-southeast-1.aws.neon.tech;Port=5432;Database=neondb;Username=neondb_owner;Password=npg_MygPNjGAFB17;SslMode=Require;Trust Server Certificate=true"
```

### 3. Build & Run (ต้องใช้เครื่องที่รองรับ GPU)

```bash
docker build -t reflexcore .
docker run --gpus all -p 5000:5000 -p 8001:8001   -e POSTGRES_CONNECTION="$POSTGRES_CONNECTION"   reflexcore
```

> - พอร์ต `5000` สำหรับ ReflexCore API
> - พอร์ต `8001` สำหรับ llama.cpp inference server

---

## 🧪 ตัวอย่างการทดสอบ API

```bash
curl http://localhost:5000/api/agents
curl http://localhost:8001/completion
```

---

## 🧠 Embedding และ LLM

- โมเดลที่รองรับ (.gguf):
  - `Nous-Hermes-2-Mistral-7B-DPO.Q4_K_M.gguf`
  - `OpenThaiGPT-1.5-7B.Q4_K_M.gguf`

- เวกเตอร์ฝังแบบ float + cosine/similarity ปกติ ไม่ใช้ AI Reasoning

---

## 🧰 คำสั่ง EF Core สำหรับ Dev

ใช้สำหรับเพิ่ม migration (ไม่ต้อง run update เอง เพราะ `Program.cs` จัดการแล้ว):

```bash
dotnet ef migrations add AddInitialSchema -p ReflexCoreAgent.csproj -s ReflexCoreAgent.csproj
```

---

## 🔬 ด้านเทคนิค

- ใช้ .NET 9 + Ubuntu 22.04 + CUDA 12.2
- LLM ใช้ llama.cpp ที่ build จาก source พร้อม server mode (`-DLLAMA_BUILD_SERVER=ON`)
- ไม่มีระบบ security แบบ Enterprise

---

## 📄 License

เผยแพร่แบบ MIT License — สามารถนำไปใช้งาน ปรับแต่ง หรือโฮสต์เองได้ทันที  
หากต้องการใช้งานเชิงพาณิชย์ กรุณาให้เครดิตตามสมควร

---

## 💡 คำอธิบายปิดท้าย

> ReflexCore ไม่ใช่ AI อัจฉริยะ แต่เป็นระบบที่ **ช่วยให้คนทั่วไปเข้าถึงการใช้งาน AI อย่างมีโครงสร้าง**  
> ไม่ต้องพึ่ง API ภายนอก ไม่ต้องมีความรู้ด้าน Machine Learning  
> แค่รัน Docker แล้วเริ่มต้นสร้าง Agent ของคุณได้ทันที
