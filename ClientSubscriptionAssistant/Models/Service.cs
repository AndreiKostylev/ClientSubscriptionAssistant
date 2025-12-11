using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientSubscriptionAssistant.Models
{
    public class Service
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LogoUrl { get; set; }
        public string LogoPath { get; set; } // Локальный путь к логотипу
        public decimal BasePrice { get; set; }
        public List<Subscription> Subscriptions { get; set; } = new();
    }
}
