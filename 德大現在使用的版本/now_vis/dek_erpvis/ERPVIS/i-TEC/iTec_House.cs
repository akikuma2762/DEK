using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPVIS.i_TEC
{
    class iTec_House : IdepHouse
    {
        const string DateFormat = "yyyyMMdd";
        IniManager iniManager = new IniManager("D:/DATAWINCommand.ini");

        public DataTable stockanalysis(string day,dekModel model)
        {
            string sqlcmd = Get_stockanalysis(day,  model);
            DataTable dt = iTechDB.Get_DataTable(sqlcmd);
            return dt;
        }

        public DataTable stockanalysis(DateTime day, dekModel model)
        {
            return stockanalysis(day.ToString(DateFormat),  model);
        }

        //----------------------------------------SQL指令區--------------------------------------------------
        string Get_stockanalysis(string day, dekModel model)
        {
            string SQLCommand = iniManager.ReadIniFile("DATAWIN", "stockanalysis", "");
            if (SQLCommand != "")
                SQLCommand = string.Format(SQLCommand, day);
            return SQLCommand;
        }
    }
}
