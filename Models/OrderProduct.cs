﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace IlQuadrifoglio.Models
{
    public class OrderProduct
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderProductId { get; set; }

        public int Quantity { get; set; }
        [ForeignKey("Order")]
        public int FkOrderId { get; set; }
        [JsonIgnore]
        public Order? Order { get; set; }

        [ForeignKey("Product")]
        public int FkProductId { get; set; }
        //[JsonIgnore]
        public Product? Product { get; set; }

        //[JsonIgnore]
        public IEnumerable<Ingredient>? Ingredients { get; set; }
    }
}
