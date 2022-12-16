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
        public DataTable FactoryDataTable { get; set; }
        public DataTable WorkerDataTable { get; set; }
        public DataTable ErrorUsolvedDataTable { get; set; }
        public DataTable CapacitydDataTable { get; set; }
    }
}
