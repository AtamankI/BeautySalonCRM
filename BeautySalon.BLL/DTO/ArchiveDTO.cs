using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeautySalon.BLL.DTO
{
    public class ArchiveDTO
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string MasterLastname { get; set; }
        public string MasterName { get; set; }
        public int MasterId { get; set; }
        public int TotalCustomers { get; set; }
        public decimal Salary { get; set; }
        public decimal Profit { get; set; }
    }
}
