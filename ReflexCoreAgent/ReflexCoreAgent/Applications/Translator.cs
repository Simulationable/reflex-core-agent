using ReflexCoreAgent.Domain.Response;
using ReflexCoreAgent.Helpers;
using ReflexCoreAgent.Interfaces;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace ReflexCoreAgent.Applications
{
    public class Translator : ITranslator
    {
        public async Task<string> TranslateToEnglish(string thai)
        {
            string prompt = $"Translate this Thai sentence to English. Respond with only the English sentence:\n\"{thai}\"";
            var response = await LlamaCppHelper.RunAsync("thaigpt", prompt, "translation");
            var parsed = JsonSerializer.Deserialize<LlamaCppResponse>(response);
            var contentOnly = parsed?.content?.Trim('"', '\n', '\r', ' ');
            return contentOnly;
        }

        public async Task<string> TranslateToThai(string english)
        {
            string prompt = $"Translate this English sentence to Thai. Respond with only the Thai sentence:\n\"{english}\"";
            var response = await LlamaCppHelper.RunAsync("thaigpt", prompt, "translation");
            var parsed = JsonSerializer.Deserialize<LlamaCppResponse>(response);
            var contentOnly = parsed?.content?.Trim('"', '\n', '\r', ' ');
            return contentOnly;
        }

        public async Task<string> Answer(string intent, string knowledge)
        {
            if (intent.Contains("เจาะระบบ") || intent.Contains("แฮก") || intent.Contains("hack"))
            {
                return "ขออภัยค่ะ ระบบไม่สามารถให้ข้อมูลในส่วนนี้ได้";
            }

            string prompt = $@"
                คุณคือตัวแทนฝ่ายบริการลูกค้าที่ตอบคำถามอย่างสุภาพ เป็นธรรมชาติ และมีความหลากหลาย โดยใช้เฉพาะข้อมูลที่ให้ไว้ในระบบเท่านั้น ห้ามเดาหรือแต่งเติมข้อมูลเพิ่มเติมโดยเด็ดขาด หากไม่มีข้อมูล ให้ตอบด้วยถ้อยคำที่ต่างกันเล็กน้อย ไม่ซ้ำรูปแบบเดิม แม้เนื้อหาจะคล้ายกันก็ตาม

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
                {knowledge ?? "ไม่มีข้อมูล"}

                คำถาม:
                {intent}
                คำตอบ:";

            var response = await LlamaCppHelper.RunAsync("thaigpt", prompt, "translation");
            var parsed = JsonSerializer.Deserialize<LlamaCppResponse>(response);
            var result = Regex.Replace(parsed.content, @"^\s+|\s+$", "");
            return result;
        }
    }
}
