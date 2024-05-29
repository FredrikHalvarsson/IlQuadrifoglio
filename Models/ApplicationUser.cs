﻿using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace IlQuadrifoglio.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Display(Name = "Förnamn")]
        [StringLength(50, ErrorMessage = "Name can't be longer than 50 characters")]
        [JsonIgnore]
        public string? FirstName { get; set; }

        [Display(Name = "Efternamn")]
        [StringLength(50, ErrorMessage = "Name can't be longer than 50 characters")]
        [JsonIgnore]
        public string? LastName { get; set; }

        //[JsonIgnore]
        public virtual ICollection<Address>? Addresses { get; set; }

        //[JsonIgnore]
        public List<Order>? Orders { get; set; }

        public ApplicationUser()
        {
            Orders = new List<Order>();

            Orders.Add(new Order
            {
                FkCustomerId = this.Id
            });
        }
    }
}
