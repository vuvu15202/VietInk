using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace VietInkWebApp.Entities
{
    [BindProperties]
    public partial class Order
    {
        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int OrderId { get; set; }
        public int? UserId { get; set; }
        public DateTime? OrderDate { get; set; }
        public DateTime? RequiredDate { get; set; }
        public DateTime? ShippedDate { get; set; }
        public decimal? Freight { get; set; }
        [Required(ErrorMessage = "Mục địa chỉ không được để trống.")]
        [StringLength(255, ErrorMessage = "Name cannot be longer than 50 characters.")]
        public string? ShipAddress { get; set; }
        [Required(ErrorMessage = "Mục Thành Phố không được để trống.")]
        [StringLength(50, ErrorMessage = "Name cannot be longer than 50 characters.")]
        public string? ShipCity { get; set; }
        [Required(ErrorMessage = "Mục Huyện không được để trống.")]
        [StringLength(50, ErrorMessage = "Name cannot be longer than 50 characters.")]
        public string? ShipRegion { get; set; }
        public string? ShipPostalCode { get; set; }
        public string? ShipCountry { get; set; }

        [Required(ErrorMessage = "Mục Số điện thoại không được để trống.")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Phone number must be exactly 10 characters long.")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Số điện thoại chưa đúng định dạng.")]
        public string? PhoneNumber { get; set; }
        public string? Note { get; set; }
        public double? Discount { get; set; }

        [AllowNull]
        public virtual User? User { get; set; }
        [AllowNull]
        [JsonIgnore]
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
