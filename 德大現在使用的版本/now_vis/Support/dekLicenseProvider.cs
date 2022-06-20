
#define LICENSE_CHECK

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Support
{

    public class dekLicenseProvider : LicenseProvider
    {
        public dekLicenseProvider()
        { }

        public override License GetLicense(LicenseContext context, Type type, object instance, bool allowExceptions)
        {
            //type: type of the component requesting the license
            //instance: instance of the component requesting the license
            //allowExceptions: should throw an LicenseException if license can not be granted
#if DEBUG!=true 
            if (context.UsageMode == LicenseUsageMode.Designtime)
            {
                // 只單純的檢查元件所在的路徑是否有一個 dekLicense.txt 檔案,
                // 有的話就表示合法授權.
                //----------------------------------------------
                try
                {
                    //得到 Support 目錄
                    string assemblyname = Assembly.GetExecutingAssembly().GetName().Name; //Support
                    Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(assemblyname + ".SolutionPath.txt");
                    StreamReader sr = new StreamReader(stream, System.Text.Encoding.Default);
                    string project_path = sr.ReadLine().Trim();
                    sr.Close();
                    stream.Close();
                    string fname = Path.Combine(project_path, "dekLicense.txt");
                    if (File.Exists(fname))
                    {
                        //check file contain if valid or not

                        return new dekLicense();
                    }
                }
                catch  {  }
                if (allowExceptions)
                {
                    throw new Exception("您尚未取得使用此元件的合法授權，請向德科智能科技洽詢!");
                }
                return null;
            }
            else // 不需要檢查授權
#endif
            {
                return new dekLicense();
            }
        }
    }

    public class dekLicense : License
    {
        public override string LicenseKey
        {
            get
            {
                // 你應該修改這個實作，以提供應用程式唯一的授權碼
                return "";
            }
        }
        public dekLicense()
        {

        }
        public override void Dispose()
        {

        }
    }
}
