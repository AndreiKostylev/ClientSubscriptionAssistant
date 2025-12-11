using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientSubscriptionAssistant.Models.DTO
{
    public class UpdateSubscriptionDTO
    {
        public string? Name { get; set; }
        public decimal? Price { get; set; }
        public string? BillingCycle { get; set; }
        public bool? IsActive { get; set; }
    }
}
