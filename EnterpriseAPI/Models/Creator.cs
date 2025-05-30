﻿using System;
using System.ComponentModel.DataAnnotations;

namespace EnterpriseAPI.Models
{
    public class Creator
    {
        [Key]
        [Required]
        public int Id { get; set; }
        public string Name { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
        [Required]
        public bool IsActive { get; set; }
    }
}
