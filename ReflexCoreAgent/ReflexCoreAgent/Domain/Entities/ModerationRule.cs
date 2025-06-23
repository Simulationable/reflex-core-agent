using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ReflexCoreAgent.Domain.Entities
{
    public class ModerationRule
    {
        public Guid Id { get; set; }

        [Required]
        public string Keyword { get; set; } = string.Empty;

        [Required]
        public string ResponseMessage { get; set; } = "ขออภัยค่ะ ระบบไม่สามารถให้ข้อมูลในส่วนนี้ได้";
        public bool IsActive { get; set; } = true;
        public Guid? AgentId { get; set; }
        [JsonIgnore]
        public Agent? Agent { get; set; }
    }
}
