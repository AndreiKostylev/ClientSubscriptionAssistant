using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientSubscriptionAssistant.Models.DTO
{
    public class UserStatisticsDTO
    {
        public int TotalSubscriptions { get; set; }
        public int ActiveSubscriptions { get; set; }
        public decimal TotalMonthlyCost { get; set; }
        public decimal TotalYearlyCost { get; set; }
        public int CategoriesCount { get; set; }
        public int ServicesCount { get; set; }
        public DateTime? FirstSubscriptionDate { get; set; }
        public DateTime? LastPaymentDate { get; set; }
        public decimal TotalSpent { get; set; }

        // Самые популярные категории
        public Dictionary<string, int> TopCategories { get; set; } = new();

        // График расходов по месяцам
        public Dictionary<string, decimal> MonthlySpending { get; set; } = new();
    }
}
