using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeautySalon.BLL.DTO
{
    public class ServiceDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string HallType { get; set; }
        public int? HallId { get; set; }
        public string CategoryName { get; set; }
        public int? CategoryId { get; set; }
        public decimal Price { get; set; }
        public int Avaliability { get; set; }
        public bool IsAvaliable { get; set; }
    }

}
