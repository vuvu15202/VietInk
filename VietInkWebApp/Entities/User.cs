using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace VietInkWebApp.Entities
{
    [BindProperties]
    public partial class User
    {
        public User()
        {
            Orders = new HashSet<Order>();
        }

        public int UserId { get; set; }
        [AllowNull]
        public string UserName { get; set; } = null!;
        [Required(ErrorMessage = "Mục Tên không được để trống.")]
        [StringLength(50, ErrorMessage = "Name cannot be longer than 50 characters.")]
        public string FirstName { get; set; } = null!;
        [Required(ErrorMessage = "Mục Họ không được để trống.")]
        [StringLength(50, ErrorMessage = "Name cannot be longer than 50 characters.")]
        public string LastName { get; set; } = null!;

        public string Password { get; set; } = null!;
        [Required(ErrorMessage = "Mục Email không được để trống.")]
        [EmailAddress(ErrorMessage = "Địa chỉ Email chưa chính xác.")]
        public string Email { get; set; } = null!;
        
        //public string Phone { get; set; }
        //[Required(ErrorMessage = "Mục Số điện thoại không được để trống.")]
        //[StringLength(10, MinimumLength = 10, ErrorMessage = "Phone number must be exactly 10 characters long.")]
        //[RegularExpression(@"^\d{10}$", ErrorMessage = "Invalid phone number.")]
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }

        public int Role { get; set; }
        [JsonIgnore]

        public virtual ICollection<Order> Orders { get; set; }
    }
}
