using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientSubscriptionAssistant.Models
{
    public class Subscription
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime NextPaymentDate { get; set; }
        public string BillingCycle { get; set; } = "monthly";
        public bool IsActive { get; set; } = true;


        public int UserId { get; set; }
        public int CategoryId { get; set; }
        public int ServiceId { get; set; }

 
        public User User { get; set; }
        public Category Category { get; set; }
        public Service Service { get; set; }
        public List<Payment> Payments { get; set; } = new();

    
        public bool IsExpiring => IsActive && NextPaymentDate <= DateTime.Now.AddDays(7);
        public string StatusColor => IsActive ? (IsExpiring ? "Orange" : "Green") : "Gray";
        public string StatusText => IsActive ? (IsExpiring ? "Скоро оплата" : "Активна") : "Неактивна";
    }
}
