using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeautySalon.BLL.DTO
{
    public class BookingDTO
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Time { get; set; }
        public string MasterLastname { get; set; }
        public string MasterName { get; set; }
        public int? MasterId { get; set; }
        public string ClientLastname { get; set; }
        public string ClientName { get; set; }
        public string ClientPhone { get; set; }
        public int? ClientId { get; set; }
        public string Service { get; set; }
        public int? ServiceID { get; set; }
        public string Notes { get; set; }

    }

}
