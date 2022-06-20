using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeautySalon.BLL.DTO
{
    public class MaterialDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? Volume { get; set; }
        public string Number { get; set; }
        public decimal? TotalAmount { get; set; }
        public decimal? GramAmount { get; set; }
        public decimal? Price { get; set; }
        public decimal? PriceGram { get; set; }
        public string ManufacturerName { get; set; }
        public string CategoryName { get; set; }
        public int? ManufacturerId { get; set; }
        public int? CategoryId { get; set; }

    }

}
