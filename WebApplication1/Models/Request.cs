using System;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Request
    {
        [Key]
        public int Id { get; set; }
        public Type Type { get; set; }
        public DateTime Date {get;set;}
        public decimal GrossValue { get; set; }
        public decimal Fee { get; set; }
        public decimal NetValue { get; set; }
        public Status Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int CreatorId { get; set; }
        [Required]
        public Creator Creator { get; set; }
    }


    public enum Status
    {
        Pending,
        Approved,
        Denied
    }

    public enum Type
    {
        Regular,
        Anticipation
    }
}
