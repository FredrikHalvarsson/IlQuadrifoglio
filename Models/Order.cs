﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace IlQuadrifoglio.Models
{
    public enum Status
    {
        Unsent,
        Mottagen,
        Tillagas,
        Levereras,
        Levererad

    }
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderId { get; set; }

        public Status OrderStatus { get; set; } = 0;
        public DateTime OrderTime { get; set; } = DateTime.Now;
        public DateTime EstimatedDelivery => OrderTime.AddMinutes(40);

        [ForeignKey("Customer")]
        public string FkCustomerId { get; set; } = string.Empty;
        [JsonIgnore]
        public ApplicationUser? Customer { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<OrderProduct>? OrderProducts { get; set; } = new List<OrderProduct>();
    }
}
