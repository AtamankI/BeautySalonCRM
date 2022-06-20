using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeautySalon.BLL.DTO
{
    public class MasterDTO
    {
        public int Id { get; set; }
        public string Lastname { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public DateTime? HireDate { get; set; }
        public DateTime? RetireDate { get; set; }
        public string Category { get; set; }
        public int? SalaryPercent { get; set; }
        public int? CategoryId { get; set; }
        public int? SalaryTypeId { get; set; }


    }
}
