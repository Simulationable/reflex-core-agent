using ReflexCoreAgent.Domain.Model;
using ReflexCoreAgent.Interfaces.Services;
using System.Text.RegularExpressions;

namespace ReflexCoreAgent.Applications
{
    public class SimpleThaiTimeParser : ITimeParser
    {
        public ParsedTime? Parse(string userInput)
        {
            var date = ParseDateFromText(userInput);
            var time = ParseTimeFromText(userInput);

            if (date == null || time == null)
                return null;

            var (hour, minute) = time.Value;
            var start = new DateTime(date.Value.Year, date.Value.Month, date.Value.Day, hour, minute, 0);
            var end = start.AddMinutes(30);

            return new ParsedTime
            {
                Start = start,
                End = end
            };
        }

        public static (int hour, int minute)? ParseTimeFromText(string userInput)
        {
            userInput = userInput.Trim().Replace("น.", "")
                                 .Replace("โมงเช้า", "โมง")
                                 .Replace("โมงเย็น", "โมง")
                                 .Replace("โมงตรง", "โมง");

            var thaiToDigit = new Dictionary<string, int>
        {
            { "ศูนย์", 0 }, { "หนึ่ง", 1 }, { "สอง", 2 }, { "สาม", 3 }, { "สี่", 4 }, { "ห้า", 5 },
            { "หก", 6 }, { "เจ็ด", 7 }, { "แปด", 8 }, { "เก้า", 9 }, { "สิบ", 10 }, { "สิบเอ็ด", 11 },
            { "สิบสอง", 12 }, { "สิบสาม", 13 }, { "สิบสี่", 14 }, { "สิบห้า", 15 }, { "สิบหก", 16 },
            { "สิบเจ็ด", 17 }, { "สิบแปด", 18 }, { "สิบเก้า", 19 }, { "ยี่สิบ", 20 }, { "ยี่สิบเอ็ด", 21 },
            { "ยี่สิบสอง", 22 }, { "ยี่สิบสาม", 23 }, { "ยี่สิบสี่", 24 }, { "สามสิบ", 30 }, { "สามสิบห้า", 35 },
            { "สี่สิบ", 40 }, { "สี่สิบห้า", 45 }, { "ห้าสิบ", 50 }, { "ห้าสิบห้า", 55 }
        };

            foreach (var kv in thaiToDigit)
                userInput = userInput.Replace(kv.Key, kv.Value.ToString());

            var match = Regex.Match(userInput, @"(\d{1,2})[:.](\d{1,2})");
            if (match.Success)
                return (int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value));

            match = Regex.Match(userInput, @"(\d{1,2})\s*ทุ่ม");
            if (match.Success)
                return (18 + int.Parse(match.Groups[1].Value), 0);

            match = Regex.Match(userInput, @"ตี\s*(\d{1,2})");
            if (match.Success)
                return (int.Parse(match.Groups[1].Value), 0);

            bool half = userInput.Contains("ครึ่ง");
            match = Regex.Match(userInput, @"(\d{1,2})\s*โมง\s*(\d{1,2})?");
            if (match.Success)
            {
                int hour = int.Parse(match.Groups[1].Value);
                int minute = half ? 30 : (int.TryParse(match.Groups[2].Value, out var m) ? m : 0);
                if (1 <= hour && hour <= 4) return (hour + 6, minute);
                if (hour == 5 && userInput.Contains("เย็น")) return (17, minute);
                if (hour == 6 && userInput.Contains("เย็น")) return (18, minute);
                return (hour, minute);
            }

            var phrases = new Dictionary<string, (int, int)>
        {
            { "เที่ยงคืน", (0, 0) }, { "เที่ยง", (12, 0) }, { "เช้า", (8, 0) }, { "สาย", (10, 0) },
            { "บ่าย", (14, 0) }, { "เย็น", (17, 0) }, { "ค่ำ", (19, 0) }, { "ดึก", (22, 0) },
            { "พลบค่ำ", (18, 0) }, { "ย่ำรุ่ง", (5, 30) }, { "เช้ามืด", (4, 30) },
            { "ตอนเช้า", (8, 0) }, { "ตอนสาย", (10, 0) }, { "ตอนเที่ยง", (12, 0) },
            { "ตอนบ่าย", (14, 0) }, { "ตอนเย็น", (17, 0) }, { "ตอนค่ำ", (19, 0) }, { "ตอนดึก", (22, 0) }
        };

            foreach (var phrase in phrases)
                if (userInput.Contains(phrase.Key))
                    return phrase.Value;

            return null;
        }

        public static DateTime? ParseDateFromText(string userInput)
        {
            var today = DateTime.Today;

            var match = Regex.Match(userInput, @"วันที่\s*(\d{1,2})(?:\s*นี้)?");
            if (match.Success)
            {
                int day = int.Parse(match.Groups[1].Value);
                try
                {
                    var candidate = new DateTime(today.Year, today.Month, day);
                    if (candidate < today)
                        candidate = candidate.AddMonths(1);
                    return candidate;
                }
                catch { return null; }
            }

            return null;
        }

    }
}
