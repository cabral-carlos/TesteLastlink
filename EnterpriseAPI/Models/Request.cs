using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace EnterpriseAPI.Models
{
    public class Request
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        public Type Type { get; set; }
        [Required]
        public DateTime Date {get;set;}
        [Required]
        public decimal GrossValue { get; set; }
        [Required]
        public decimal Fee { get; set; }
        [Required]
        public decimal NetValue { get; set; }
        [Required]
        public Status Status { get; set; } = Status.Pending;
        [Required]
        public DateTime CreatedAt { get; set; }
        [Required]
        public DateTime UpdatedAt { get; set; }
        public int CreatorId { get; set; }
        [Required]
        public Creator Creator { get; set; }
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Status
    {
        [EnumMember(Value = "Pending")]
        Pending,
        [EnumMember(Value = "Approved")]
        Approved,
        [EnumMember(Value = "Denied")]
        Denied
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Type
    {
        [EnumMember(Value = "Regular")]
        Regular,
        [EnumMember(Value = "Anticipation")]
        Anticipation
    }
}
