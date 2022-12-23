using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dekERP_dll.dekErp
{
    public class ProductionProgress
    {
        public string FactoryFloor { get; set; }
        public string Account { get; set; }
        public string Date_Start { get; set; }
        public string Date_End { get; set; }
        public string Condition { get; set; }
        public DataTable FactoryDataTable { get; set; }
        public DataTable StaffDataTable { get; set; }
        public DataTable ErrorUnsolvedDataTable { get; set; }
        public DataTable CapacitydDataTable { get; set; }
    }
}
