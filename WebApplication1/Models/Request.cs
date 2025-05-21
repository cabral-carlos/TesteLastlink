using System;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
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
