﻿using System.ComponentModel.DataAnnotations;

namespace PatientManagement.API.Models
{
    public class Condition
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public required string Name { get; set; }

        [StringLength(255)]
        public string? Description { get; set; }
    }
}
