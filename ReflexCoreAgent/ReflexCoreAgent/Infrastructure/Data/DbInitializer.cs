using Microsoft.EntityFrameworkCore;
using ReflexCoreAgent.Domain.Entities;

namespace ReflexCoreAgent.Infrastructure.Data
{
    public static class DbInitializer
    {
        public static async Task SeedAsync(AppDbContext context)
        {
            if (!await context.Users.AnyAsync())
            {
                var admin = new User
                {
                    Username = "admin",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("P@ssw0rd"),
                    DisplayName = "Administrator",
                    Email = "admin@example.com",
                    Role = "Admin",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };
                await context.Users.AddAsync(admin);
            }

            if (!await context.Agents.AnyAsync())
            {
                var agent = new Agent
                {
                    Name = "LINE Support Agent",
                    Purpose = "ตอบคำถามลูกค้า LINE",
                    PromptTemplate = @"คุณคือตัวแทนฝ่ายบริการลูกค้าที่ตอบคำถามอย่างสุภาพ เป็นธรรมชาติ และมีความหลากหลาย โดยใช้เฉพาะข้อมูลที่ให้ไว้ในระบบเท่านั้น ห้ามเดาหรือแต่งเติมข้อมูลเพิ่มเติมโดยเด็ดขาด หากไม่มีข้อมูล ให้ตอบด้วยถ้อยคำที่ต่างกันเล็กน้อย ไม่ซ้ำรูปแบบเดิม แม้เนื้อหาจะคล้ายกันก็ตาม
                        ### ตัวอย่าง:

                        ข้อมูลจากระบบ:
                        ไม่มีข้อมูล

                        คำถาม:
                        มีโปรโมชั่นอะไรบ้างคะ?
                        คำตอบ:
                        ขออภัยค่ะ ขณะนี้ยังไม่มีข้อมูลเกี่ยวกับโปรโมชั่นในระบบนะคะ

                        ข้อมูลจากระบบ:
                        ไม่มีข้อมูล

                        คำถาม:
                        มีแพ็กเกจอะไรแนะนำไหม
                        คำตอบ:
                        ตอนนี้ยังไม่มีข้อมูลเกี่ยวกับแพ็กเกจให้บริการในระบบค่ะ

                        ข้อมูลจากระบบ:
                        ไม่มีข้อมูล

                        คำถาม:
                        มีรายละเอียดเพิ่มเติมไหม
                        คำตอบ:
                        ข้อมูลสำหรับคำถามนี้ยังไม่มีในระบบขณะนี้ค่ะ

                        ข้อมูลจากระบบ:
                        ไม่มีข้อมูล

                        คำถาม:
                        บริการนี้มีเงื่อนไขอย่างไรบ้าง
                        คำตอบ:
                        ขออภัยค่ะ ยังไม่มีการระบุข้อมูลเกี่ยวกับเงื่อนไขของบริการนี้ไว้ในระบบนะคะ

                        ### ตอนนี้:

                        ข้อมูลจากระบบ:
                        {knowledge}

                        คำถาม:
                        {intent}
                        คำตอบ:",
                    EnableModeration = true,
                    Config = new LlamaRequestConfig
                    {
                        Prompt = "", // จะ override ตอนเรียกใช้
                        NPredict = 64,
                        Temperature = 0.7,
                        TopP = 0.9,
                        TopK = 40,
                        Stop = new[] { "\n\n", "\n ", "\n#", "คำถาม", "สอบถาม", "ทีมงาน" }
                    }
                };

                agent.ModerationRules = new List<ModerationRule>
                {
                    new() { Keyword = "เจาะระบบ", ResponseMessage = "ขออภัยค่ะ ระบบไม่สามารถให้ข้อมูลในส่วนนี้ได้" },
                    new() { Keyword = "แฮก", ResponseMessage = "ขออภัยค่ะ ระบบไม่สามารถให้ข้อมูลในส่วนนี้ได้" },
                    new() { Keyword = "hack", ResponseMessage = "ขออภัยค่ะ ระบบไม่สามารถให้ข้อมูลในส่วนนี้ได้" },
                };

                await context.Agents.AddAsync(agent);
            }

            await context.SaveChangesAsync();
        }

    }
}
