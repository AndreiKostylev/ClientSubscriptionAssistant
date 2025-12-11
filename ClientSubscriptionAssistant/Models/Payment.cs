using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientSubscriptionAssistant.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public bool IsSuccessful { get; set; } = true;
        public int SubscriptionId { get; set; }
        public Subscription Subscription { get; set; }
        public string StatusIcon => IsSuccessful ? "check_circle.png" : "error.png";
        public string StatusColor => IsSuccessful ? "Green" : "Red";
    }
}
