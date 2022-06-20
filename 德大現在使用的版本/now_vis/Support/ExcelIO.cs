using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
//using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;
using System.Runtime.InteropServices;
using OfficeOpenXml;

namespace Support
{
    /// <summary>
    /// 
    /// </summary>
    public class ExcelIO
    {
        string filename;
        ExcelPackage package = null;
        ExcelWorkbook work_book = null;
        ExcelWorksheet work_sheet = null;
        //----------------------------
        public ExcelIO(string excel_檔名, string excel_工作表名稱 = "")
        {
            //excel = Excel_Init();
            filename = excel_檔名.Trim();
            if (filename != "" && File.Exists(filename))
            {
                FileInfo file_info = new FileInfo(filename);
                package = new ExcelPackage(file_info);
                ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
                work_book = package.Workbook;
            }
            if (work_book==null)
            {
                package = new ExcelPackage();
                work_book = package.Workbook;
            }
            if (excel_工作表名稱 != "")
            {
                if (!Sheet_Select(excel_工作表名稱)) //直接指定工作表名稱
                    Sheet_New(excel_工作表名稱);
            }
            else work_sheet = work_book.Worksheets[1];
        }

        public void Close()
        {
            package.Dispose();
            //確認已經沒有excel工作再回收
            GC.Collect();
        }
        
        public bool Save()
        {
            Column_AutoFit();
            return SaveAs(this.filename);
        }

        public bool SaveAs(string file)
        {
            string ext = Path.GetExtension(file);
            if (ext.ToLower()!=".xlsx")
                file =Path.ChangeExtension(file, ".xlsx");
            if (File.Exists(file) && 
                Utils.MesgBox.YesNo("Excel 另存新檔", "檔案已經存在，是否要覆蓋該檔案?") != true)
                return false;
            FileInfo file_info = new FileInfo(file);
            package.SaveAs(file_info);
            this.filename = file;
            return File.Exists(file);
        }

        #region [Excel.WorkSheet]

        private ExcelWorksheet getSheet(string sheet_name)
        {
            try { return work_book.Worksheets[sheet_name]; }
            catch { }
            return null;
        }

        public bool Sheet_Select(string sheet_name)
        {
            ExcelWorksheet sheet = getSheet(sheet_name);
            if (sheet != null)
            {
                work_sheet =sheet;
                return true;
            }
            return false;
        }
        public bool Sheet_Rename(string old_name, string new_name)
        {
            ExcelWorksheet sheet = getSheet(old_name);
            if (sheet == null) return false;
            sheet.Name = new_name;
            return true;
        }
        public bool Sheet_New(string sheet_name = "")
        {
            ExcelWorksheet sheet;
            if (sheet_name != "")
            {
                sheet = getSheet(sheet_name);
                if (sheet != null)
                {
                    work_sheet = sheet;
                    return true;
                }
            }
            work_sheet = work_book.Worksheets.Add(sheet_name);
            return true;
        }

        public bool Sheet_Delete(string sheet_name)
        {
            ExcelWorksheet sheet = getSheet(sheet_name);
            if (sheet==null || work_sheet==sheet) return false;
            work_book.Worksheets.Delete(sheet);
            return true;
        }

        #endregion [Excel.WorkSheet]
        public void Column_AutoFit()
        {
            string addr = work_sheet.Dimension.Address;
            //ExcelRange range = work_sheet.Cells["A1:"+ addr];
            ExcelRange range = work_sheet.Cells[addr];//20180802 code by john
            //work_sheet.Cells[addr].AutoFitColumns();
            range.AutoFitColumns();

            //ExcelRange range = work_sheet.Cells[1, 1, 100, 100];
            //.AutoFitColumns();
            //work_sheet.Cells.AutoFitColumns();
            /*
            Excel.Worksheet sheet = (Excel.Worksheet)work_book.ActiveSheet;
            int total_row = sheet.Cells.Rows.Count;
            int total_col = sheet.Cells.Columns.Count;
            Excel.Range wRange = sheet.Range[sheet.Cells[1, 1], sheet.Cells[total_row, total_col]];
            wRange.Columns.AutoFit();
            */
        }

        //----------------------------------------------------
        #region [Excel.Range] 
        public ExcelRange GetRange(int row = 1, int col = 1, int row_count = 1, int col_count = 1)
        {
            if (row_count < 1) row_count = 1;
            if (col_count < 1) col_count = 1;
            ExcelRange range = work_sheet.Cells[row, col, row + row_count - 1, col + col_count - 1];
            return range;
        }

         public ExcelRange GetRange(string field_name_start = "A1", string field_name_end = "")
         {
            if (field_name_end != "") field_name_start += ":" + field_name_end;
            return work_sheet.Cells[field_name_start];
         }

        /*
         public void Column_NumberFormat(string field_str, string format_text)
         {
             ExcelRange formatRange = GetRange(field_str);
             formatRange.forma = format_text;
         }
         */

         #endregion

        //--------------------------------
        public string toString(Object obj)
        {
            if (obj == null) return "";
            try { return obj.ToString(); }
            catch { return ""; }
        }
        //--------------------------------------
        public void SetValue(int row, int col, string text)
        {
            work_sheet.Cells[row, col].Value = text;
        }

        public string GetValue(int row, int col)
        {
            try { return toString(work_sheet.Cells[row, col].Value); }
            catch { return ""; }
        }

        public void SetFormula(string start_field, string end_field, string formula)
        {
            GetRange(start_field, end_field).Formula = formula;
        }


        public void SetValue(string start_field, string end_field, string text)
        {
            GetRange(start_field, end_field).Value = text;
        }

        public void SetValue(string field, string text)
        {
            work_sheet.Cells[field].Value = text;
        }
        
        public string GetValue(string field = "A1")
        {
            return toString(work_sheet.Cells[field].Value);
        }

        public object[] GetObject(int row = 0, int col = 0, int col_count = 1)
        {
            ExcelRange range = GetRange(row, col, 1, col_count);
            object[] tab = new string[col_count];
            int index = 0;
            foreach (ExcelRange item in range)
            {
                tab[index++] = (object)item.Value;
            }
            return tab;
        }
        public string [] GetValue(int row = 0, int col = 0, int col_count = 1)
        {
            ExcelRange range = GetRange(row, col, 1, col_count);
            string[] tab = new string[col_count];
            int index = 0;
            foreach (ExcelRange item in range)
            {
                tab[index++] = toString(item.Value);
            }
            return tab;
        }
    }
}
