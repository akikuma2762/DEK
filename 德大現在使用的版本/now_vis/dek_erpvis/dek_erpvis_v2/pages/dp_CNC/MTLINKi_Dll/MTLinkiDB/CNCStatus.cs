using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTLinkiDB
{
    public enum CNCStatusCode { NONE, DISCONNECT, STOP, EMERGENCY, OPERATE, SUSPEND, ALARM, MANUAL, WARMUP };
    public class CNCStatus
    {
        private static Dictionary<CNCStatusCode, string> CNCStatus_name_list = new Dictionary<CNCStatusCode, string>()
        {
            { CNCStatusCode.NONE, "NONE"},
            { CNCStatusCode.DISCONNECT, "DISCONNECT"}, {CNCStatusCode.STOP,"STOP"},
            { CNCStatusCode.EMERGENCY,"EMERGENCY"}, { CNCStatusCode.OPERATE,"OPERATE"},
            { CNCStatusCode.SUSPEND, "SUSPEND"},{ CNCStatusCode.ALARM,"ALARM"},
            { CNCStatusCode.MANUAL,"MANUAL"}, { CNCStatusCode.WARMUP,"WARMUP"}
        };

        public static string toString(CNCStatusCode status)
        {
            try
            {
                return CNCStatus_name_list[status];
            }
            catch { }
            return "NONE";
        }

        public static CNCStatusCode toCNCStatusCode(string status_string)
        {
            try
            {
                status_string = status_string.ToUpper();
                return CNCStatus_name_list.FirstOrDefault(x => x.Value == status_string).Key;
            }
            catch { }
            return CNCStatusCode.NONE;
        }
    }
}
