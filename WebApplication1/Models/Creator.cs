using System;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Creator
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
    }
}
