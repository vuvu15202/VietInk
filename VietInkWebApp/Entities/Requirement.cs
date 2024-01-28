using System;
using System.Collections.Generic;

namespace VietInkWebApp.Entities
{
    public partial class Requirement
    {
        public int RequirementId { get; set; }
        public string CustomerName { get; set; } = null!;
        public string? Email { get; set; }
        public string PhoneNumber { get; set; } = null!;
        public string Note { get; set; } = null!;
        public DateTime? CreateDate { get; set; }
        public string? Status { get; set; }
    }
}
