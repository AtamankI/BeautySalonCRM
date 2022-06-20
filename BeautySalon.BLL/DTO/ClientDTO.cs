using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeautySalon.BLL.DTO
{
    public class ClientDTO
    {
        public int Id { get; set; }
        public string Lastname { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int NumberOfVisits { get; set; }
        public string Notes { get; set; }
        public DateTime? LastVisit { get; set; }
        public int InArchive { get; set; }
        public bool IsInArhive { get; set; }

    }

}
