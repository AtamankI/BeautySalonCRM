using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeautySalon.BLL.DTO
{
    public class VisitDTO
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int MastedId { get; set; }
        public string MasterLastname { get; set; }
        public string MasterName { get; set; }
        public int ClientId { get; set; }
        public string ClientLastname { get; set; }
        public string ClientName { get; set; }
        public int ServiceId { get; set; }
        public string Service { get; set; }
        public int PaymentTypeId { get; set; }
        public string PaymentType { get; set; }
        public decimal ServicePrice { get; set; }
        public decimal Amount { get; set; }
        public decimal AdditionalCost { get; set; }
        public decimal MaterialPrice { get; set; }
        public decimal Profit { get; set; }
        public decimal Salary { get; set; }
        public decimal Tips { get; set; }
    }

}
