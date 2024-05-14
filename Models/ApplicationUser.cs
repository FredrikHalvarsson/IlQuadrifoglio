﻿using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace IlQuadrifoglio.Models
{
    public class ApplicationUser :IdentityUser
    {

        [Required]
        [StringLength(50, ErrorMessage ="Name can't be longer than 50 characters")]
        public string FirstName { get; set; }

        [StringLength(50, ErrorMessage = "Name can't be longer than 50 characters")]
        public string LastName { get; set; }

        [StringLength(50, ErrorMessage = "Address can't be longer than 50 characters")]
        public string Address { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IEnumerable<Order>? Orders { get; set; }
    }
}
