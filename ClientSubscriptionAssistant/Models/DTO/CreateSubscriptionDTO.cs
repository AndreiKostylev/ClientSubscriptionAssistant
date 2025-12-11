using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientSubscriptionAssistant.Models.DTO
{
    public class CreateSubscriptionDTO
    {
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public DateTime StartDate { get; set; }
        public string BillingCycle { get; set; } = "monthly";
        public int CategoryId { get; set; }
        public int ServiceId { get; set; }
    }
}
