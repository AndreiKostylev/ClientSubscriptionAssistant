using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientSubscriptionAssistant.Models.DTO
{
    public class SubscriptionDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime NextPaymentDate { get; set; }
        public string BillingCycle { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public int CategoryId { get; set; }
        public CategoryDTO? Category { get; set; }
        public int ServiceId { get; set; }
        public ServiceDTO? Service { get; set; }
    }
}
