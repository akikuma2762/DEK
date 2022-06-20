using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LNCcomm;

namespace Support
{
    public class LNC_CSV
    {
     
        public string _IPAddress { get; set; }      //IP設定
        public bool _IsConnected { get; set; }      //連線狀態
        

        public LNC_CSV()
        {
            Init("192.168.1.100");
        }
        public LNC_CSV(string mach_ip)
        {
            Init(mach_ip);
        }

        private void Init(string mach_ip)
        {
            _IPAddress = mach_ip;
            _IsConnected = false;
        }

        public bool Start()
        {
            if (!Open()) return false;
            _IsConnected = true;
            System.Threading.Thread.Sleep(1000);  //20181019wswang--
            SensorEnabled(true);
            return true;
        }
        //終止事件(TIMER使用)
        public bool Stop()
        {
            SensorEnabled(false);     //20181019wswang--
            Close();
            return true;
        }

        //取得控制器狀態(暫時應該沒用到)
        public bool GetStausMessage(out string mesg)
        {
            mesg = "Not Opened";
            if (_IsConnected != true) return false;
            bool ok = false;
            byte commSts = 0, sensorSTS = 0;
            int watchdogCnt = 0;
            short rc = CLNCc.lnc_get_status(gNid, ref commSts, ref sensorSTS, ref watchdogCnt);
            //rc          = 取得控制器與感測器的狀態
            //gNid        = 連線設備，目前只支援一台(預設0)
            //commSts     = 與控制器的通訊狀態
            //sensorSTS   = 感測器狀態
            //watchdogCnt = 當控制器的主程序正常運行時此參數的數值將持續累加
            switch (commSts)
            {
                case CLNCc.LNC_COMM_STATE_DISCONNECT:   //斷線
                    mesg = "Disconnect";
                    break;
                case CLNCc.LNC_COMM_STATE_CONNECTING:   //連線中
                    mesg = "Connecting";
                    break;
                case CLNCc.LNC_COMM_STATE_FAIL:         //失敗
                    mesg = "Fail";
                    break;
                case CLNCc.LNC_COMM_STATE_OK:           //已連線
                    mesg = "Connected";
                    if (sensorSTS == CLNCc.LNC_SENSOR_ONLINE)
                    {
                        ok = true;
                        mesg += ",Sensor Online, WD=" + watchdogCnt.ToString();
                    }
                    else
                    {
                        mesg = ",Sensor Offline";
                    }
                    break;
                case CLNCc.LNC_COMM_STATE_NORESPONSE:   //無回應
                    mesg = "No Response";
                    break;
                default:                                //未知(沒有定義)
                    mesg = "Unknown";
                    break;
            }
            mesg = commSts.ToString() + " : " + mesg;
            return ok;
        }
        //數值歸零
        public bool SetZero()
        {
            if (_IsConnected != true) return false;
            short rc = 0;
            rc = CLNCc.lnc_svi_set_zero(gNid);
            //rc = 感測器歸零。執行函式後會以目前感測器的固定方式作為基準, 將各軸數值設定為 0
            return true;
        }
        //數值清除
        public bool ClearZero()
        {
            if (_IsConnected != true) return false;
            short rc = 0;
            rc = CLNCc.lnc_svi_clear_zero(gNid);
            //rc = 清除感測器歸零設定值
            return true;
        }

        //------------------------------------------------------------------
        ushort gNid = 0;

        //設置IP等連線資訊
        private bool Open()
        {
            //short ret = CLNCc.lnc_connect(gNid, _IPAddress, 0);
            short ret = CLNCc.lnc_connect(gNid, _IPAddress, 0);
            //lnc_connect = 與控制器建立連線
            //ip = IFC1800 預設值為"192.168.1.100", 請注意 PC 應與 IFC1800 設置在相同網段, 如"192.168.1.x"
            //0  = 連線超時等待時間, 單位: millisecond, 若設定 0 表示不等待連線成功, 立即返回函式
            return ret == 0;
        }

        private void Close()
        {
            if (_IsConnected != true) return;
            CLNCc.lnc_disconnect(gNid); //lnc_disconnect = 與控制器斷開連線
            _IsConnected = false;
        }

        uint TDData_Size = 0;
        short[] TDData_Array = null;
        //取得記憶體儲存時域資料、儲存長度
        public short[] DoDataCollect_TimeDomainXYZ(ref uint data_length)
        {
            uint numTD = 0;
            data_length = 0;
            if (_IsConnected)
            {
                short rc = 0;
                //string mesg;
                //bool ok = GetStausMessage(out mesg);
                rc = CLNCc.lnc_svi_get_td_data_length(gNid, ref data_length);
                //rc       = 取得目前資料緩衝區內存放的時域資料長度
                //gNid     = 連線設備，目前只支援一台(預設0)
                //TDLength = 資料的長度
                if (data_length != 0)
                {
                    if (data_length > TDData_Size)
                    {
                        TDData_Size = data_length;
                        TDData_Array = new short[TDData_Size];
                    }
                    rc = CLNCc.lnc_svi_get_td_data(gNid, data_length, ref TDData_Array[0], ref numTD);
                    return TDData_Array;
                    //rc         = 取得目前資料緩衝區內存放的時域資料
                    //TDLength   = 預計取得的長度，由於一組資料包含三軸數值, 所以此參數應設定為 3 的倍數
                    //parrTDData = 時域資料, 單位: mG, 此參數需輸入包含 length 個 short 項目的陣列指標
                    //numTD      = 實際取得的資料長度
                    /*
                    for (i = 0; i < numTD; i += 3)
                    {
                        //txtTDX.Text = parrTDData[i].ToString();
                        //txtTDY.Text = parrTDData[i + 1].ToString();
                        //txtTDZ.Text = parrTDData[i + 2].ToString();
                    }
                    */
                }
            }
            return null;
        }

        float[] arrFDData = new float[CLNCc.LNC_FD_DATA_LENGTH_1D66K];
        //取得記憶體儲存頻域資料、長度
        public float[] DoDataCollect_FreqDomain()
        {
            if (_IsConnected)
            {
                short rc = CLNCc.lnc_svi_get_fd_data(gNid, ref arrFDData[0]);
                //rc        = 取得目前資料緩衝區存放的頻域資料
                //arrFDData = 頻域資料, 單位: dB, 此參數需輸入包含 830 個 float 項目的陣列指標
                if (rc == CLNCc.LNC_ERR_NO_ERROR)   //LNC_ERR_NO_ERROR = 函式執行成功
                {
                    /*
                    float max = 0;
                    int maxFq = 0;
                    for (int i = 0; i < CLNCc.LNC_FD_DATA_LENGTH_1D66K; i++)    //數量 = 830個
                    {
                        if (arrFDData[i] > max)  //max = 0
                        {
                            max = arrFDData[i];
                            maxFq = i + 1;
                        }
                    }
                    */
                    //txtFDfq.Text = maxFq.ToString();        //計數最大值出現位置
                    //txtFDvalue.Text = max.ToString("0.02"); //待確認
                    return arrFDData;
                }
            }
            return null;
        }
        //資料接收開、關
        public void SensorEnabled(bool Set)
        {
            if (_IsConnected != true) return;
            short rc = 0;
            if (Set)
            {
                rc = CLNCc.lnc_svi_enable(gNid, 1);
            }
            else
            {
                rc = CLNCc.lnc_svi_enable(gNid, 0);   //20181019--
            }
        }
    }
}
